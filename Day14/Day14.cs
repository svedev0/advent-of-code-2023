namespace advent_of_code_2023;

public class Day14
{
	public static void SolvePart1()
	{
		char[][] input = [.. File.ReadAllLines("Day14\\input.txt")
			.Select(s => s.ToCharArray())];

		int result = SlideNorth(input)
			.Select((row, idx) =>
				row.Count(c => c == 'O') * (input.Length - idx))
			.Sum();

		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		char[][] input = [.. File.ReadAllLines("Day14\\input.txt")
			.Select(s => s.ToCharArray())];

		Dictionary<string, int> cache = [];
		int cycle = 1;
		int oneBillion = 1_000_000_000;

		while (cycle <= oneBillion)
		{
			input = SlideNorth(input);
			input = SlideWest(input);
			input = SlideSouth(input);
			input = SlideEast(input);

			string curr = string.Join(string.Empty, input.SelectMany(c => c));

			if (cache.TryGetValue(curr, out var cached))
			{
				int remaining = oneBillion - cycle - 1;
				int loop = cycle - cached;

				int loopRemaining = remaining % loop;
				cycle = oneBillion - loopRemaining - 1;
			}

			cache[curr] = cycle++;
		}

		int result = input
			.Select((row, idx) =>
				row.Count(c => c == 'O') * (input.Length - idx))
			.Sum();

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static char[][] SlideNorth(char[][] input)
	{
		for (int row = 1; row < input.Length; row++)
		{
			for (int col = 0; col < input[row].Length; col++)
			{
				if (input[row][col] != 'O')
				{
					continue;
				}

				int prev = 1;
				while (input[row - prev][col] == '.')
				{
					input[row - prev][col] = 'O';
					input[row - prev + 1][col] = '.';
					prev++;

					if (row - prev < 0)
					{
						break;
					}
				}
			}
		}
		return input;
	}

	private static char[][] SlideEast(char[][] input)
	{
		for (int row = 0; row < input.Length; row++)
		{
			for (int col = input[row].Length - 2; col >= 0; col--)
			{
				if (input[row][col] != 'O')
				{
					continue;
				}

				int prev = 1;
				while (input[row][col + prev] == '.')
				{
					input[row][col + prev] = 'O';
					input[row][col + prev - 1] = '.';
					prev++;

					if (col + prev >= input[row].Length)
					{
						break;
					}
				}
			}
		}
		return input;
	}

	private static char[][] SlideSouth(char[][] input)
	{
		for (int row = input.Length - 2; row >= 0; row--)
		{
			for (int col = 0; col < input[row].Length; col++)
			{
				if (input[row][col] != 'O')
				{
					continue;
				}

				int prev = 1;
				while (input[row + prev][col] == '.')
				{
					input[row + prev][col] = 'O';
					input[row + prev - 1][col] = '.';
					prev++;

					if (row + prev >= input.Length)
					{
						break;
					}
				}
			}
		}
		return input;
	}

	private static char[][] SlideWest(char[][] input)
	{
		for (int row = 0; row < input.Length; row++)
		{
			for (int col = 1; col < input[row].Length; col++)
			{
				if (input[row][col] != 'O')
				{
					continue;
				}

				int prev = 1;
				while (input[row][col - prev] == '.')
				{
					input[row][col - prev] = 'O';
					input[row][col - prev + 1] = '.';
					prev++;

					if (col - prev < 0)
					{
						break;
					}
				}
			}
		}
		return input;
	}
}
