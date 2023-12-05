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

	public static void SolvePart2()
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

		List<(long from, long to)> ranges = [];

		for (int i = 0; i < seeds.Count; i += 2)
		{
			ranges.Add((from: seeds[i], to: seeds[i] + seeds[i + 1] - 1));
		}

		foreach (var map in maps)
		{
			List<(long from, long to, long adj)> orderedMap = [..
				map.OrderBy(x => x.from)];
			List<(long from, long to)> newRanges = [];

			foreach (var r in ranges)
			{
				var range = r;
				foreach (var (from, to, adj) in orderedMap)
				{
					if (range.from < from)
					{
						newRanges.Add((range.from, Math.Min(range.to, from - 1)));
						range.from = from;
						if (range.from >= range.to)
						{
							break;
						}
					}
					if (range.from <= to)
					{
						newRanges.Add((range.from + adj, Math.Min(range.to, to) + adj));
						range.from = to + 1;
						if (range.from >= range.to)
						{
							break;
						}
					}
				}
				if (range.from < range.to)
				{
					newRanges.Add(range);
				}
			}

			ranges = newRanges;
		}

		long lowestLocation = ranges.Min(r => r.from);
		Console.WriteLine($"[Part 2] Answer: {lowestLocation}");
	}
}
