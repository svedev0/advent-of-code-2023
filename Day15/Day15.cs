namespace advent_of_code_2023;

public class Day15
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllText("Day15\\input.txt")
			.Split(',')];
		int result = input.Sum(Hash);
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllText("Day15\\input.txt")
			.Split(',')];

		List<List<(string label, int power)>> boxes = [];

		for (int i = 0; i < 256; i++)
		{
			boxes.Add([]);
		}

		foreach (string seg in input)
		{
			char operatorChar = seg.Contains('=') ? '=' : '-';

			string label = seg[..seg.IndexOfAny(['-', '='])];
			int power = operatorChar == '=' ? (seg[^1] - '0') : -1;
			int hashIdx = Hash(label);

			int index = boxes[hashIdx].FindIndex(x => x.label == label);

			if (operatorChar == '-' && index != -1)
			{
				boxes[hashIdx].RemoveAt(index);
			}

			if (operatorChar == '=' && index == -1)
			{
				boxes[hashIdx].Add((label, power));
			}

			if (operatorChar == '=' && index != -1)
			{
				boxes[hashIdx][index] = (label, power);
			}
		}

		int result = boxes.Select((box, boxIdx) =>
			box.Select((lens, lensIdx) =>
				(1 + boxIdx) * (1 + lensIdx) * lens.power)
				.Sum())
			.Sum();

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static int Hash(string input)
	{
		return input.Aggregate(0, (x, y) => (x + y) * 17 % 256);
	}
}
