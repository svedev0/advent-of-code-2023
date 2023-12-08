namespace advent_of_code_2023;

public class Day08
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day08\\input.txt")];
		char[] moves = input[0].ToCharArray();
		List<string> nodes = input.GetRange(2, input.Count - 2);
		List<(string curr, string l, string r)> parsedNodes = [];

		foreach (string node in nodes)
		{
			parsedNodes.Add(ParseNode(node));
		}

		string currPos = "AAA";
		int movesMade = 0;

		while (currPos != "ZZZ")
		{
			foreach (char mv in moves)
			{
				var (curr, l, r) = parsedNodes.First(w => w.curr == currPos);
				if (mv == 'L')
				{
					currPos = l;
					movesMade++;
				}
				else if (mv == 'R')
				{
					currPos = r;
					movesMade++;
				}
			}
		}

		Console.WriteLine($"[Part 1] Answer: {movesMade}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day08\\input.txt")];

		string moves = input[0];
		Dictionary<string, (string l, string r)> nodes = [];

		foreach (var node in input.Skip(2))
		{
			var (curr, l, r) = ParseNode(node);
			nodes[curr] = (l, r);
		}

		int currMove = 0;
		List<string> startNodes = [.. nodes.Keys.Where(key => key[2] == 'A')];
		long result = 1;

		foreach (var node in startNodes)
		{
			string next = node;
			long movesMade = 0;
			currMove = 0;

			while (next[2] != 'Z')
			{
				next = moves[currMove] == 'L' ? nodes[next].l : nodes[next].r;
				
				currMove++;
				movesMade++;

				if (currMove == moves.Length)
				{
					currMove = 0;
				}
			}

			var (greater, smaller) = result > movesMade ?
				(result, movesMade) :
				(movesMade, result);

			while (smaller != 0)
			{
				(greater, smaller) = (smaller, greater % smaller);
			}

			result *= (movesMade / greater);
		}

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static (string curr, string l, string r) ParseNode(string node)
	{
		string cleanNode = node.Replace(" ", "");
		string curr = cleanNode.Split('=')[0];
		string l = cleanNode.Split('=')[1].Split(',')[0].Replace("(", "");
		string r = cleanNode.Split('=')[1].Split(',')[1].Replace(")", "");
		return (curr, l, r);
	}
}
