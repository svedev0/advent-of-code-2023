namespace advent_of_code_2023;

public class Day23
{
	record Direction(int Row, int Col)
	{
		public static Direction Up = new(-1, 0);
		public static Direction Down = new(1, 0);
		public static Direction Left = new(0, -1);
		public static Direction Right = new(0, 1);
	}

	record Position(int Row, int Col)
	{
		public Position Move(Direction dir) =>
			new (Row + dir.Row, Col + dir.Col);
	}

	public static void SolvePart1()
	{
		string[] input = File.ReadAllLines("Day23\\input.txt");
		Position start = new(0, 1);
		Position end = new(input.Length - 1, input[0].Length - 2);

		var graph = BuildGraph(input);
		int result = FindLongestPath(graph, [], start, end);
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		string[] input = File.ReadAllLines("Day23\\input.txt");
		Position start = new(0, 1);
		Position end = new(input.Length - 1, input[0].Length - 2);

		var graph = BuildGraph(input, true);
		int result = FindLongestPath(graph, [], start, end);
		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static Dictionary<Position, HashSet<Position>> BuildGraph(
		string[] input,
		bool part2 = false)
	{
		Dictionary<Position, HashSet<Position>> graph = [];

		for (var row = 0; row < input.Length; row++)
		{
			var line = input[row];

			for (var col = 0; col < line.Length; col++)
			{
				if (line[col] == '#')
				{
					continue;
				}

				var pos = new Position(row, col);
				graph[pos] = [];

				if (!part2)
				{
					switch (line[col])
					{
						case '>':
							graph[pos].Add(pos.Move(Direction.Right));
							continue;
						case 'v':
							graph[pos].Add(pos.Move(Direction.Down));
							continue;
						case '<':
							graph[pos].Add(pos.Move(Direction.Left));
							continue;
					}
				}

				if (row > 0 && input[row - 1][col] != '#')
				{
					graph[pos].Add(pos.Move(Direction.Up));
				}

				if (row < input.Length - 1 && input[row + 1][col] != '#')
				{
					graph[pos].Add(pos.Move(Direction.Down));
				}

				if (col > 0 && input[row][col - 1] != '#')
				{
					graph[pos].Add(pos.Move(Direction.Left));
				}

				if (col < line.Length - 1 && input[row][col + 1] != '#')
				{
					graph[pos].Add(pos.Move(Direction.Right));
				}
			}
		}

		return graph;
	}

	private static int FindLongestPath(
		Dictionary<Position, HashSet<Position>> graph,
		HashSet<Position> visited,
		Position current,
		Position end)
	{
		int result = 0;

		if (current == end)
		{
			return visited.Count;
		}

		foreach (Position neighbor in graph[current])
		{
			if (!visited.Add(neighbor))
			{
				continue;
			}

			int length = FindLongestPath(graph, visited, neighbor, end);
			result = Math.Max(result, length);
			visited.Remove(neighbor);
		}

		return result;
	}
}
