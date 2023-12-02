namespace advent_of_code_2023;

public class Day02
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day02\\input.txt")];
		Dictionary<string, int> maximums = new()
		{
			{ "red",   12 },
			{ "green", 13 },
			{ "blue",  14 },
		};
		List<int> possibleGames = [];

		for (int i = 0; i < input.Count; i++)
		{
			int gameNum = i + 1;
			bool gamePossible = true;

			string results = input[i].Split(": ")[1];
			List<string> rounds = [.. results.Split("; ")];
			
			foreach (var round in rounds)
			{
				foreach (var cubes in round.Split(", "))
				{
					int count = int.Parse(cubes.Split(" ")[0]);
					string colour = cubes.Split(" ")[1];

					if (count > maximums[colour])
					{
						gamePossible = false;
						continue;
					}
				}
			}

			if (gamePossible)
			{
				possibleGames.Add(gameNum);
			}
		}

		Console.WriteLine($"[Part 1] Answer: {possibleGames.Sum()}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day02\\input.txt")];
		List<int> gamePowers = [];

		for (int i = 0; i < input.Count; i++)
		{
			string results = input[i].Split(": ")[1];
			List<string> rounds = [.. results.Split("; ")];
			Dictionary<string, int> gameMax = new()
			{
				{ "red",   0 },
				{ "green", 0 },
				{ "blue",  0 },
			};

			foreach (var round in rounds)
			{
				foreach (var cubes in round.Split(", "))
				{
					int count = int.Parse(cubes.Split(" ")[0]);
					string colour = cubes.Split(" ")[1];

					gameMax[colour] = count > gameMax[colour] ?
						count : gameMax[colour];
				}
			}

			gamePowers.Add(gameMax["red"] * gameMax["green"] * gameMax["blue"]);
		}

		Console.WriteLine($"[Part 2] Answer: {gamePowers.Sum()}");
	}
}
