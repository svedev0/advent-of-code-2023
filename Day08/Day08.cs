namespace advent_of_code_2023;

public class Day08
{
	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day08\\input.txt")];
		char[] moves = input[0].ToCharArray();
		List<string> nodes = input.GetRange(2, input.Count - 2);
		List<(string curr, string l, string r)> parsedNodes = [];

		foreach (string node in nodes)
		{
			parsedNodes.Add(ParseNode(node));
		}

		string currPos = "AAA";
		int movesMade = 0;

		while (currPos != "ZZZ")
		{
			foreach (char mv in moves)
			{
				var (curr, l, r) = parsedNodes.First(w => w.curr == currPos);
				if (mv == 'L')
				{
					currPos = l;
					movesMade++;
				}
				else if (mv == 'R')
				{
					currPos = r;
					movesMade++;
				}
			}
		}

		Console.WriteLine($"[Part 1] Answer: {movesMade}");
	}

	private static (string curr, string l, string r) ParseNode(string node)
	{
		string cleanNode = node.Replace(" ", "");
		string curr = cleanNode.Split('=')[0];
		string l = cleanNode.Split('=')[1].Split(',')[0].Replace("(", "");
		string r = cleanNode.Split('=')[1].Split(',')[1].Replace(")", "");
		return (curr, l, r);
	}
}
