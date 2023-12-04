namespace advent_of_code_2023;

public class Day04
{
	struct Card
	{
		public int Number { get; set; }
		public List<int> Winning { get; set; }
		public List<int> Actual { get; set; }
	}

	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day04\\input.txt")];
		List<Card> cards = [];

		for (int i = 0; i < input.Count; i++)
		{
			int cardNum = int.Parse(input[i].Split(':')[0].Split(" ").Last());

			string[] rowData = input[i].Split(':')[1].Split(" | ");
			string winningNumsStr = rowData[0].Replace("  ", " ").Trim();
			string actualNumsStr = rowData[1].Replace("  ", " ").Trim();

			cards.Add(new()
			{
				Number = cardNum,
				Winning = [.. winningNumsStr.Split(" ").Select(int.Parse)],
				Actual = [.. actualNumsStr.Split(" ").Select(int.Parse)],
			});
		}

		int totalScore = 0;
		foreach (Card card in cards)
		{
			IEnumerable<int> intersecting = card.Actual.Intersect(card.Winning);
			int score = 0;
			foreach (int s in intersecting)
			{
				score = score == 0 ? 1 : score * 2;
			}
			totalScore += score;
		}

		Console.WriteLine($"[Part 1] Answer: {totalScore}");
	}
}
