namespace advent_of_code_2023;

public class Day17
{
	struct Path(Position position, Direction direction, int lineLength)
	{
		public readonly Position Position = position;
		public readonly Direction Direction = direction;
		public readonly int LineLength = lineLength;
		public int Heat { get; set; }
	}

	readonly struct Position(int row, int col)
	{
		public readonly int Row = row;
		public readonly int Col = col;

		public Position Move(Direction dir)
		{
			return new Position(Row + dir.Row, Col + dir.Col);
		}
	}

	record Direction(int Row, int Col)
	{
		public readonly int Row = Row;
		public readonly int Col = Col;

		public Direction TurnLeft()
		{
			return new Direction(-Col, Row);
		}

		public Direction TurnRight()
		{
			return new Direction(Col, -Row);
		}

		public static readonly Direction Up = new(-1, 0);
		public static readonly Direction Down = new(1, 0);
		public static readonly Direction Left = new(0, -1);
		public static readonly Direction Right = new(0, 1);
	}

	private static int[][] map = [];
	private static readonly PriorityQueue<Path, int> queue = new();
	private static readonly HashSet<string> visited = [];

	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day17\\input.txt")];
		map = [.. input.Select(s =>
			s.Select(c => int.Parse(c.ToString())).ToArray())];

		queue.Enqueue(new Path(new(0, 0), Direction.Right, 0), 0);

		int result = 0;

		while (queue.Count > 0)
		{
			Path path = queue.Dequeue();

			if (path.Position.Row == map.Length - 1 &&
				path.Position.Col == map[0].Length - 1)
			{
				result = path.Heat;
				break;
			}

			if (path.LineLength < 3)
			{
				TryMove(path, path.Direction);
			}

			TryMove(path, path.Direction.TurnLeft());
			TryMove(path, path.Direction.TurnRight());
		}

		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day17\\input.txt")];
		map = [.. input.Select(s =>
			s.Select(c => int.Parse(c.ToString())).ToArray())];

		queue.Clear();
		visited.Clear();
		
		queue.Enqueue(new Path(new(0, 0), Direction.Right, 0), 0);
		queue.Enqueue(new Path(new(0, 0), Direction.Down, 0), 0);
		
		int result = 0;

		while (queue.Count > 0)
		{
			var path = queue.Dequeue();
			if (path.Position.Row == map.Length - 1 &&
				path.Position.Col == map[0].Length - 1)
			{
				result = path.Heat;
				break;
			}

			if (path.LineLength < 10)
			{
				TryMove(path, path.Direction);
			}

			if (path.LineLength >= 4)
			{
				TryMove(path, path.Direction.TurnLeft());
				TryMove(path, path.Direction.TurnRight());
			}
		}

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static void TryMove(Path path, Direction dir)
	{
		Path newPath = new(
			path.Position.Move(dir), dir, dir == path.Direction ?
			path.LineLength + 1 : 1);

		if (newPath.Position.Row < 0 || newPath.Position.Row >= map.Length ||
			newPath.Position.Col < 0 || newPath.Position.Col >= map[0].Length)
		{
			return;
		}

		int[] ints = [
			newPath.Position.Row,
			newPath.Position.Col,
			newPath.Direction.Row,
			newPath.Direction.Col,
			newPath.LineLength
		];
		string key = string.Join(',', ints);

		if (visited.Contains(key))
		{
			return;
		}

		visited.Add(key);
		newPath.Heat = path.Heat +
			map[newPath.Position.Row][newPath.Position.Col];
		queue.Enqueue(newPath, newPath.Heat);
	}
}
