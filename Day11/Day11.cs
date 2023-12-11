namespace advent_of_code_2023;

public class Day11
{
	record Vector(long Row, long Col)
	{
		public Vector VectorTo(Vector next)
		{
			return new(next.Row - Row, next.Col - Col);
		}
		public long Steps { get; set; } = Math.Abs(Row) + Math.Abs(Col);
	}

	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day11\\input.txt")];

		int[] rows = [.. Enumerable.Range(0, input.Count)
			.Where(row => input[row].All(c => c == '.'))];
		int[] cols = [.. Enumerable.Range(0, input[0].Length)
			.Where(col => input.All(c => c[col] == '.'))];

		List<Vector> galaxies = [];
		for (int i = 0; i < input.Count; i++)
		{
			int rowOffset = rows.Count(r => r <= i);

			for (int j = 0; j < input[0].Length; j++)
			{
				if (input[i][j] != '#')
				{
					continue;
				}

				int colOffset = cols.Count(c => c <= j);
				galaxies.Add(new Vector(i + rowOffset, j + colOffset));
			}
		}

		(Vector vec1, Vector vec2)[] pairs = galaxies
			.SelectMany((gal1, i) => galaxies
				.Skip(i + 1)
				.Select(gal2 => (gal1, gal2)))
			.ToArray();

		long sum = pairs.Sum(p => p.vec1.VectorTo(p.vec2).Steps);
		Console.WriteLine($"[Part 1] Answer: {sum}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day11\\input.txt")];
		int[] rows = [.. Enumerable.Range(0, input.Count)
			.Where(row => input[row].All(c => c == '.'))];
		int[] cols = [.. Enumerable.Range(0, input[0].Length)
			.Where(col => input.All(c => c[col] == '.'))];

		int expansionRate = 1000000;

		List<Vector> galaxies = [];
		for (int i = 0; i < input.Count; i++)
		{
			int rowOffset = rows.Count(r => r <= i) * (expansionRate - 1);

			for (int j = 0; j < input[0].Length; j++)
			{
				if (input[i][j] != '#')
				{
					continue;
				}

				int colOffset = cols.Count(c => c <= j) * (expansionRate - 1);
				galaxies.Add(new Vector(i + rowOffset, j + colOffset));
			}
		}

		(Vector vec1, Vector vec2)[] pairs = galaxies
			.SelectMany((gal1, i) => galaxies
				.Skip(i + 1)
				.Select(gal2 => (gal1, gal2)))
			.ToArray();

		long sum = pairs.Sum(p => p.vec1.VectorTo(p.vec2).Steps);
		Console.WriteLine($"[Part 2] Answer: {sum}");
	}
}
