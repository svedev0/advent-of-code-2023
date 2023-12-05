namespace advent_of_code_2023;

public class Day05
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day05\\input.txt")];

		List<long> seeds = [.. input[0].Split(' ').Skip(1).Select(long.Parse)];
		List<List<(long from, long to, long adj)>> maps = [];
		List<(long from, long to, long adj)>? currMap = null;

		foreach (string line in input.Skip(2))
		{
			if (line.EndsWith(':'))
			{
				currMap = [];
				continue;
			}
			else if (line.Length == 0 && currMap != null)
			{
				maps.Add(currMap!);
				currMap = null;
				continue;
			}

			long[] nums = line.Split(' ').Select(long.Parse).ToArray();
			currMap!.Add((nums[1], nums[1] + nums[2] - 1, nums[0] - nums[1]));
		}

		if (currMap != null)
		{
			maps.Add(currMap);
		}

		long lowestLocation = long.MaxValue;
		foreach (long seed in seeds)
		{
			long value = seed;
			foreach (var map in maps)
			{
				foreach (var (from, to, adj) in map)
				{
					if (value >= from && value <= to)
					{
						value += adj;
						break;
					}
				}
			}
			lowestLocation = Math.Min(lowestLocation, value);
		}

		Console.WriteLine($"[Part 1] Answer: {lowestLocation}");
	}
}
