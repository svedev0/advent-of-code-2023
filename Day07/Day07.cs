namespace advent_of_code_2023;

public class Day07
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day07\\input.txt")];
		List<(string cards, int bid)> hands = input
			.Select(x => x.Split(' '))
			.Select(h => (h[0], int.Parse(h[1])))
			.ToList();
		string cardOrder = "AKQJT98765432";

		var rankedHands = hands
			.OrderBy(h =>
				GetStrength(h.cards) << 20 | BreakTie(h.cards, cardOrder))
			.Select((h, i) => h.bid * (hands.Count - i));
		int result = rankedHands.Sum();

		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day07\\input.txt")];
		List<(string cards, int bid)> hands = input
			.Select(x => x.Split(' '))
			.Select(h => (h[0], int.Parse(h[1])))
			.ToList();
		string cardOrder = "AKQT98765432J";

		var rankedHands = hands
			.OrderBy(h =>
				h.cards.Min(c =>
					GetStrength(h.cards.Replace('J', c))) << 20 |
					BreakTie(h.cards, cardOrder))
			.Select((h, i) => h.bid * (hands.Count - i));
		int result = rankedHands.Sum();

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static int GetStrength(string cards)
	{
		int[] group = [.. cards.GroupBy(c => c)
			.Select(group => group.Count())
			.OrderBy(value => value)];
		return group.Length * 5 - group[^1];
	}

	private static int BreakTie(string cards, string cardOrder)
	{
		return cards.Aggregate(0, (a, c) =>
			a << 4 | cardOrder.IndexOf(c));
	}
}
