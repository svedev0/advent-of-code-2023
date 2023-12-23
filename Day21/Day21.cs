namespace advent_of_code_2023;

public class Day21
{
	enum Dir { N, S, E, W }

	record Coord(int X, int Y)
	{
		public Coord Move(Dir dir, int dist = 1)
		{
			return dir switch
			{
				Dir.N => new(X - dist, Y),
				Dir.S => new(X + dist, Y),
				Dir.E => new(X, Y + dist),
				Dir.W => new(X, Y - dist),
				_ => throw new Exception(),
			};
		}
	}

	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day21\\input.txt")];
		int gridSize = input[0].Length;

		Coord start = Enumerable.Range(0, gridSize)
			.SelectMany(i => Enumerable.Range(0, gridSize)
				.Where(j => input[i][j] == 'S')
				.Select(j => new Coord(i, j)))
			.Single();

		HashSet<Coord> moves = [start];

		for (var i = 0; i < 64; i++)
		{
			moves = new(moves
				.SelectMany(it =>
					new[] { Dir.N, Dir.S, Dir.E, Dir.W }
					.Select(dir => it.Move(dir)))
				.Where(dest =>
					dest.X >= 0 &&
					dest.Y >= 0 &&
					dest.X < gridSize &&
					dest.Y < gridSize &&
					input[dest.X][dest.Y] != '#'));
		}

		int result = moves.Count;
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day21\\input.txt")];
		int gridSize = input[0].Length;

		Coord start = Enumerable.Range(0, gridSize)
			.SelectMany(i => Enumerable.Range(0, gridSize)
				.Where(j => input[i][j] == 'S')
				.Select(j => new Coord(i, j)))
			.Single();

		int grids = 26501365 / gridSize;
		int rem = 26501365 % gridSize;

		List<int> sequence = [];
		HashSet<Coord> moves = [start];
		int steps = 0;

		for (int n = 0; n < 3; n++)
		{
			for (; steps < n * gridSize + rem; steps++)
			{
				moves = new(moves
					.SelectMany(it =>
						new[] { Dir.N, Dir.S, Dir.E, Dir.W }
						.Select(dir => it.Move(dir)))
					.Where(dest => input
						[((dest.X % 131) + 131) % 131]
						[((dest.Y % 131) + 131) % 131] != '#'));
			}

			sequence.Add(moves.Count);
		}

		int c = sequence[0];
		int ab = sequence[1] - c;
		int fourAPlusTwoB = sequence[2] - c;
		int twoA = fourAPlusTwoB - (2 * ab);
		int a = twoA / 2;
		int b = ab - a;

		long coeff(long n) => a * (n * n) + b * n + c;

		long result = coeff(grids);
		Console.WriteLine($"[Part 2] Answer: {result}");
	}
}
