namespace advent_of_code_2023;

public class Day06
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day06\\input.txt")];

		string[] timesStr = input[0].Split(':')[1].Trim().Split(" ");
		string[] distsStr = input[1].Split(':')[1].Trim().Split(" ");
		List<int> times = timesStr.Where(w => !string.IsNullOrEmpty(w))
			.Select(t => int.Parse(t.Trim())).ToList();
		List<int> dists = distsStr.Where(w => !string.IsNullOrEmpty(w))
			.Select(d => int.Parse(d.Trim())).ToList();

		int marginOfError = 1;

		for (int i = 0; i < times.Count; i++)
		{
			int count = 0;
			for (int j = 0; j < times[i]; j++)
			{
				if (j * (times[i] - j) > dists[i])
				{
					count++;
				}
			}

			marginOfError *= count;
		}

		Console.WriteLine($"[Part 1] Answer: {marginOfError}");
	}
}
