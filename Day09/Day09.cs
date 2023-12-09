namespace advent_of_code_2023;

public class Day09
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day09\\input.txt")];
		/*
		List<int[]> sequences = [];

		foreach (string line in input)
		{
			sequences.Add([.. line.Split(" ").Select(int.Parse)]);
		}

		int ef = sequences.Sum(
			sequences
			.Reverse()
			.Skip(1)
			.Aggregate(seed: 0, func: (n, seq) => n + seq[^1]));
		*/

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

		int result = histories.Sum(h => Extrapolate(h));
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	private static int Extrapolate(IList<int[]> sequences)
	{
		return sequences
			.Reverse()
			.Skip(1)
			.Aggregate(seed: 0, func: (n, seq) => n + seq[^1]);
	}
}
