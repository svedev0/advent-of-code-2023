namespace advent_of_code_2023;

public class Day16
{
	enum Directions
	{
		None = 0,
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8
	}

	public static void SolvePart1()
	{
		char[][] input = [.. File.ReadAllLines("Day16\\input.txt")
			.Select(l => l.ToCharArray())];

		int result = CountTiles(input, 0, -1, Directions.Right);
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		char[][] input = [.. File.ReadAllLines("Day16\\input.txt")
			.Select(l => l.ToCharArray())];

		List<Task<int>> tasks = [];

		for (int i = 0; i < input.Length; i++)
		{
			int currRow = i;
			tasks.Add(Task.Run(() =>
				CountTiles(input, currRow, -1, Directions.Right)));
			tasks.Add(Task.Run(() =>
				CountTiles(input, currRow, input[currRow].Length, Directions.Left)));
		}

		for (int i = 0; i < input[0].Length; i++)
		{
			int currCol = i;
			tasks.Add(Task.Run(() =>
				CountTiles(input, -1, currCol, Directions.Down)));
			tasks.Add(Task.Run(() =>
				CountTiles(input, input.Length, currCol, Directions.Up)));
		}

		Task.WhenAll(tasks).Wait();
		int result = tasks.Max(t => t.Result);
		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static int CountTiles(char[][] map, int startRow, int startCol, Directions startDir)
	{
		Dictionary<(int row, int col), Directions> tiles = new()
		{
			[(startRow, startCol)] = Directions.None
		};

		Queue<(int Row, int Column, Directions Direction)> beams = new();
		beams.Enqueue((startRow, startCol, startDir));

		while (beams.TryDequeue(out var beam))
		{
			if (tiles.TryGetValue((beam.Row, beam.Column), out var tileDirections) &&
				tileDirections.HasFlag(beam.Direction))
			{
				continue;
			}

			tiles[(beam.Row, beam.Column)] = tileDirections | beam.Direction;

			var (row, col) = beam.Direction switch
			{
				Directions.Up => (beam.Row - 1, beam.Column),
				Directions.Down => (beam.Row + 1, beam.Column),
				Directions.Left => (beam.Row, beam.Column - 1),
				Directions.Right => (beam.Row, beam.Column + 1),
				_ => throw new Exception("Invalid direction")
			};

			if (row < 0 || row >= map.Length || col < 0 || col >= map[row].Length)
			{
				continue;
			}

			beam = beam with { Row = row, Column = col };

			switch (map[row][col])
			{
				case '/':
					beams.Enqueue(beam with
					{
						Direction = beam.Direction switch
						{
							Directions.Up => Directions.Right,
							Directions.Down => Directions.Left,
							Directions.Left => Directions.Down,
							Directions.Right => Directions.Up,
							_ => throw new Exception("Invalid direction")
						}
					});
					break;
				case '\\':
					beams.Enqueue(beam with
					{
						Direction = beam.Direction switch
						{
							Directions.Up => Directions.Left,
							Directions.Down => Directions.Right,
							Directions.Left => Directions.Up,
							Directions.Right => Directions.Down,
							_ => throw new Exception("Invalid direction")
						}
					});
					break;
				case '-' when beam.Direction is Directions.Up or Directions.Down:
					beams.Enqueue(beam with { Direction = Directions.Left });
					beams.Enqueue(beam with { Direction = Directions.Right });
					break;
				case '|' when beam.Direction is Directions.Left or Directions.Right:
					beams.Enqueue(beam with { Direction = Directions.Up });
					beams.Enqueue(beam with { Direction = Directions.Down });
					break;
				default:
					beams.Enqueue(beam);
					break;
			}
		}

		return tiles.Count - 1;
	}
}
