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

		Console.WriteLine($"Sum of possible games: {possibleGames.Sum()}");
	}
}
