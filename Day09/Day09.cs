namespace advent_of_code_2023;

public class Day09
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day09\\input.txt")];
		List<List<int[]>> histories = [];

		foreach (string line in input)
		{
			List<int[]> seqs = [line.Split(" ").Select(int.Parse).ToArray()];
			while (seqs[^1].Any(val => val != 0))
			{
				seqs.Add(seqs[^1]
					.Skip(1)
					.Select((v, i) => v - seqs[^1][i])
					.ToArray());
			}
			histories.Add(seqs);
		}

		int result = histories.Sum(h => Extrapolate(1, h));
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day09\\input.txt")];
		List<List<int[]>> histories = [];

		foreach (string line in input)
		{
			List<int[]> seqs = [line.Split(" ").Select(int.Parse).ToArray()];
			while (seqs[^1].Any(val => val != 0))
			{
				seqs.Add(seqs[^1]
					.Skip(1)
					.Select((v, i) => v - seqs[^1][i])
					.ToArray());
			}
			histories.Add(seqs);
		}

		int result = histories.Sum(h => Extrapolate(2, h));
		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static int Extrapolate(int part, IList<int[]> sequences)
	{
		if (part == 1)
		{
			return sequences
				.Reverse()
				.Skip(1)
				.Aggregate(seed: 0, func: (n, seq) => n + seq[^1]);
		}
		else
		{
			return sequences
				.Reverse()
				.Skip(1)
				.Aggregate(seed: 0, func: (n, seq) => seq[0] - n);
		}
	}
}
