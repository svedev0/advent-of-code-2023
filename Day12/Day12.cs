namespace advent_of_code_2023;

public class Day12
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day12\\input.txt")];

		long result = 0;
		Dictionary<string, long> cache = [];

		foreach (var line in input.Select(l => l.Split(' ')))
		{
			string springs = line[0];
			List<int> groups = [.. line[1].Split(',').Select(int.Parse)];

			result += Calculate(cache, springs, groups);
		}

		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day12\\input.txt")];

		long result = 0;
		Dictionary<string, long> cache = [];

		foreach (var line in input.Select(l => l.Split(' ')))
		{
			string springs = string.Join('?', Enumerable.Repeat(line[0], 5));

			List<int> groups = [..line[1].Split(',').Select(int.Parse)];
			groups = [.. Enumerable.Repeat(groups, 5).SelectMany(g => g)];

			result += Calculate(cache, springs, groups);
		}

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static long Calculate(Dictionary<string, long> cache, string springs, List<int> groups)
	{
		string key = $"{springs},{string.Join(',', groups)}";

		if (cache.TryGetValue(key, out long value))
		{
			return value;
		}

		value = GetCount(cache, springs, groups);
		cache[key] = value;
		return value;
	}

	private static long GetCount(Dictionary<string, long> cache, string springs, List<int> groups)
	{
		while (true)
		{
			if (groups.Count == 0)
			{
				return springs.Contains('#') ? 0 : 1;
			}

			if (string.IsNullOrEmpty(springs))
			{
				return 0;
			}

			if (springs.StartsWith('.'))
			{
				springs = springs.Trim('.');
				continue;
			}

			if (springs.StartsWith('?'))
			{
				long result1 = Calculate(cache, "." + springs[1..], groups);
				long result2 = Calculate(cache, "#" + springs[1..], groups);
				return result1 + result2;
			}

			if (springs.StartsWith('#'))
			{
				if (groups.Count == 0)
				{
					return 0;
				}

				if (springs.Length < groups[0])
				{
					return 0;
				}

				if (springs[..groups[0]].Contains('.'))
				{
					return 0;
				}

				if (groups.Count > 1)
				{
					if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
					{
						return 0;
					}

					springs = springs[(groups[0] + 1)..];
					groups = groups[1..];
					continue;
				}

				springs = springs[groups[0]..];
				groups = groups[1..];
				continue;
			}
		}
	}
}
