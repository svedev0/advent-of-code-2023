using System.Text.RegularExpressions;

namespace advent_of_code_2023;

public class Day19
{
	enum Oprtr { LT, GT, Pipe }
	enum Field { X, M, A, S, Any }
	record Step(Field In, string Out, Oprtr Oprtr, int Val);
	record PRange(int Low, int High);

	public static void SolvePart1()
	{
		string nl = Environment.NewLine;
		string[] input = File.ReadAllText("Day19\\input.txt").Split(nl + nl);
		Dictionary<string, Step[]> rules = input[0]
			.Split(nl)
			.Select(ParseRule)
			.ToDictionary();
		List<Dictionary<Field, int>> parts = input[1]
			.Split(nl)
			.Select(GetPart)
			.ToList();

		int result = parts
			.Where(part => ParsePart(part, rules))
			.Select(part => part.Values.Sum())
			.Sum();
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		string nl = Environment.NewLine;
		string[] input = File.ReadAllText("Day19\\input.txt").Split(nl + nl);
		Dictionary<string, Step[]> rules = input[0]
			.Split(nl)
			.Select(ParseRule)
			.ToDictionary();

		var complete = new List<Dictionary<Field, PRange>>();
		var initial = new Dictionary<Field, PRange>()
		{
			{ Field.X, new PRange(1, 4000) },
			{ Field.M, new PRange(1, 4000) },
			{ Field.A, new PRange(1, 4000) },
			{ Field.S, new PRange(1, 4000) },
		};
		Queue<(Dictionary<Field, PRange>, string, int)> queue = new();
		queue.Enqueue((initial, "in", 0));

		while (queue.Count > 0)
		{
			var (parts, name, stepNum) = queue.Dequeue();
			var rule = rules[name];
			var step = rule[stepNum];
			
			foreach (var res in SplitRange(parts, step, name, stepNum))
			{
				if (res.Item2 == "A")
				{
					complete.Add(res.Item1);
				}
				else if (res.Item2 != "R")
				{
					queue.Enqueue(res);
				}
			}
		}

		ulong result = 0UL;

		foreach (var r in complete)
		{
			ulong a = Convert.ToUInt64(r[Field.X].High - r[Field.X].Low + 1);
			ulong b = Convert.ToUInt64(r[Field.M].High - r[Field.M].Low + 1);
			ulong c = Convert.ToUInt64(r[Field.A].High - r[Field.A].Low + 1);
			ulong d = Convert.ToUInt64(r[Field.S].High - r[Field.S].Low + 1);
			result += a * b * c * d;
		}

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static (string, Step[]) ParseRule(string line)
	{
		int braceIdx = line.IndexOf('{');
		string name = line[..braceIdx];
		var steps = new List<Step>();

		foreach (var part in line[(braceIdx + 1)..^1].Split(','))
		{
			int colonIdx = part.IndexOf(':');

			if (part.IndexOf('<') > -1)
			{
				Field field = ToField(part[0]);
				int val = int.Parse(part[(part.IndexOf('<') + 1).. colonIdx]);
				steps.Add(new Step(field, part[(colonIdx + 1)..], Oprtr.LT, val));
			}
			else if (part.IndexOf('>') > -1)
			{
				Field field = ToField(part[0]);
				int val = int.Parse(part[(part.IndexOf('>') + 1).. colonIdx]);
				steps.Add(new Step(field, part[(colonIdx + 1)..], Oprtr.GT, val));
			}
			else
			{
				steps.Add(new Step(Field.Any, part, Oprtr.Pipe, -1));
			}
		}

		return (name, steps.ToArray());
	}

	private static Field ToField(char c)
	{
		return c switch
		{
			'x' => Field.X,
			'm' => Field.M,
			'a' => Field.A,
			's' => Field.S,
			_ => throw new Exception()
		};
	}

	private static Dictionary<Field, int> GetPart(string line)
	{
		var match = Regex.Match(line, @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
		return new()
		{
			{ Field.X, int.Parse(match.Groups[1].Value) },
			{ Field.M, int.Parse(match.Groups[2].Value) },
			{ Field.A, int.Parse(match.Groups[3].Value) },
			{ Field.S, int.Parse(match.Groups[4].Value) },
		};
	}

	private static bool ParsePart(Dictionary<Field, int> part, Dictionary<string, Step[]> rules)
	{
		string initStage = "in";
		string acceptStage = "A";
		string rejectStage = "R";

		string currStage = initStage;
		do currStage = Classify(part, rules[currStage], 0);
		while (currStage != acceptStage && currStage != rejectStage);
		return currStage == acceptStage;
	}

	private static string Classify(Dictionary<Field, int> part, Step[] steps, int idx)
	{
		return steps[idx].Oprtr switch
		{
			Oprtr.LT => part[steps[idx].In] < steps[idx].Val ?
				steps[idx].Out :
				Classify(part, steps, idx + 1),
			Oprtr.GT => part[steps[idx].In] > steps[idx].Val ?
				steps[idx].Out :
				Classify(part, steps, idx + 1),
			Oprtr.Pipe => steps[idx].Out,
			_ => throw new Exception()
		};
	}

	private static List<(Dictionary<Field, PRange>, string, int)> SplitRange(Dictionary<Field, PRange> parts, Step step, string stepName, int stepNum)
	{
		var result = new List<(Dictionary<Field, PRange> parts, string, int)>();
		var pass = parts.ToDictionary(e => e.Key, e => e.Value);
		var fail = parts.ToDictionary(e => e.Key, e => e.Value);
		int lo, hi;

		switch (step.Oprtr)
		{
			case Oprtr.LT:
				lo = parts[step.In].Low;
				hi = parts[step.In].High;
				pass[step.In] = new PRange(lo, step.Val - 1);
				fail[step.In] = new PRange(step.Val, hi);
				result.Add((pass, step.Out, 0));
				result.Add((fail, stepName, stepNum + 1));
				break;
			case Oprtr.GT:
				lo = parts[step.In].Low;
				hi = parts[step.In].High;
				pass[step.In] = new PRange(step.Val + 1, hi);
				fail[step.In] = new PRange(lo, step.Val);
				result.Add((pass, step.Out, 0));
				result.Add((fail, stepName, stepNum + 1));
				break;
			case Oprtr.Pipe:
				result.Add((parts, step.Out, 0));
				break;
		}

		return result;
	}
}
