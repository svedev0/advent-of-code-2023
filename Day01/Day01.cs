using System.Text.RegularExpressions;

namespace advent_of_code_2023;

public class Day01
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day01\\input.txt")];
		List<int> lineValues = [];

		for (int i = 0; i < input.Count; i++)
		{
			char[] chars = input[i].ToCharArray();
			List<char> nums = chars.Where(char.IsDigit).ToList();

			var lineValue = string.Join("", nums.First(), nums.Last());
			lineValues.Add(int.Parse(lineValue));

			// See the value of each line:
			// Console.WriteLine("Line {0,4}: {1}", i + 1, lineValue);
		};

		Console.WriteLine($"[Part 1] Sum of lines: {lineValues.Sum()}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day01\\input.txt")];
		List<int> lineValues = [];

		Dictionary<string, int> numStrings = new()
		{
			{ "one", 1 },
			{ "two", 2 },
			{ "three", 3 },
			{ "four", 4 },
			{ "five", 5 },
			{ "six", 6 },
			{ "seven", 7 },
			{ "eight", 8 },
			{ "nine", 9 },
		};

		for (int i = 0; i < input.Count; i++)
		{
			string line = input[i];
			char[] chars = line.ToCharArray();
			List<int> nums = [];

			string pattern = @"one|two|three|four|five|six|seven|eight|nine|\d";

			for (int j = 0; j < chars.Length; j++)
			{
				foreach (var pat in pattern.Split('|'))
				{
					// The lines contain overlapping numbers (e.g. fiveight)
					if (!Regex.IsMatch(line[j..], "^" + pat))
					{
						continue;
					}

					var match = Regex.Match(line[j..], $"^{pat}").Value;
					if (numStrings.TryGetValue(match, out int numValue))
					{
						nums.Add(numValue);
					}
					else
					{
						nums.Add(int.Parse(match));
					}
				}
			}

			var lineValue = string.Join("", nums.First(), nums.Last());
			lineValues.Add(int.Parse(lineValue));

			// See the value of each line:
			// Console.WriteLine("Line {0,4}: {1}", i + 1, lineValue);
		}

		Console.WriteLine($"[Part 2] Sum of lines: {lineValues.Sum()}");
	}
}
