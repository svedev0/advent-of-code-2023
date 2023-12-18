namespace advent_of_code_2023;

public class Day18
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day18\\input.txt")];

		List<(long Row, long Col)> polygon = [];
		(long Row, long Col) currPos = (0, 0);
		double circ = 0.0;

		foreach (string line in input)
		{
			polygon.Add(currPos);
			string[] move = line.Split(' ');

			int len = int.Parse(move[1]);
			currPos = move[0] switch
			{
				"R" => (currPos.Row, currPos.Col + len),
				"D" => (currPos.Row + len, currPos.Col),
				"L" => (currPos.Row, currPos.Col - len),
				"U" => (currPos.Row - len, currPos.Col),
				_ => throw new Exception("Unknown direction")
			};

			circ += len;
		}

		double result = Area(polygon) + circ / 2 + 1;
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day18\\input.txt")];

		List<(long Row, long Col)> polygon = [];
		(long Row, long Col) currPos = (0, 0);
		double circ = 0.0;

		foreach (string line in input)
		{
			polygon.Add(currPos);
			string[] move = line.Split(' ');
			string hex = move[2].TrimStart('(').TrimEnd(')');
			long len = long.Parse(hex[1..^1],
				System.Globalization.NumberStyles.HexNumber);

			currPos = hex.Last() switch
			{
				'0' => (currPos.Row, currPos.Col + len),
				'1' => (currPos.Row + len, currPos.Col),
				'2' => (currPos.Row, currPos.Col - len),
				'3' => (currPos.Row - len, currPos.Col),
				_ => throw new Exception("Unknown direction")
			};

			circ += len;
		}

		double result = Area(polygon) + circ / 2 + 1;
		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static double Area(List<(long Row, long Col)> polygon)
	{
		int n = polygon.Count;
		double area = 0.0;
		for (int i = 0; i < n - 1; i++)
		{
			area += polygon[i].Row *
				polygon[i + 1].Col - polygon[i + 1].Row *
				polygon[i].Col;
		}

		area = Math.Abs(area + polygon[n - 1].Row *
			polygon[0].Col - polygon[0].Row *
			polygon[n - 1].Col) / 2.0;
		return area;
	}
}
