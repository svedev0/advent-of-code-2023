using System.Drawing;

namespace advent_of_code_2023;

public class Day10
{
	struct Pipe(int ax, int ay, int bx, int by)
	{
		public int AX = ax;
		public int AY = ay;
		public int BX = bx;
		public int BY = by;
	}

	public static void SolvePart1()
	{
		string[] input = File.ReadAllLines("Day10\\input.txt");
		string[,] grid = new string[input.Length, input[0].Length];
		Point start = new();

		for (int y = 0; y < input.Length; y++)
		{
			string[] line = [.. input[y].Select(char.ToString)];
			for (int x = 0; x < line.Length; x++)
			{
				grid[y, x] = line[x];
				if (line[x] == "S")
				{
					start = new(x, y);
				}
			}
		}

		Dictionary<string, Pipe> pipeTypes = new()
		{
			["|"] = new(0, -1, 0, 1),
			["-"] = new(-1, 0, 1, 0),
			["L"] = new(0, -1, 1, 0),
			["J"] = new(0, -1, -1, 0),
			["7"] = new(-1, 0, 0, 1),
			["F"] = new(1, 0, 0, 1),
			["."] = new(0, 0, 0, 0),
		};

		List<Point> nextMoves =
		[
			new(start.X - 1, start.Y),
			new(start.X + 1, start.Y),
			new(start.X, start.Y - 1),
			new(start.X, start.Y + 1)
		];

		Point curr = new();
		for (int i = 0; i < nextMoves.Count; i++)
		{
			Point move = nextMoves[i];
			if (Connects(start.X, start.Y, move.X, move.Y, pipeTypes[grid[move.Y, move.X]]))
			{
				curr = move;
				break;
			}
		}

		Point prev = start;
		long steps = 0;

		int[,] path = new int[grid.GetLength(0), grid.GetLength(1)];
		string[,] pathStr = new string[grid.GetLength(0), grid.GetLength(1)];

		path[start.Y, start.X] = 1;
		pathStr[start.Y, start.X] = "7";

		while (curr.X != start.X || curr.Y != start.Y)
		{
			Pipe pipe = pipeTypes[grid[curr.Y, curr.X]];
			path[curr.Y, curr.X] = 1;
			pathStr[curr.Y, curr.X] = grid[curr.Y, curr.X];

			Point next;
			if ((curr.X + pipe.AX) == prev.X && (curr.Y + pipe.AY) == prev.Y)
			{
				next = new(curr.X + pipe.BX, curr.Y + pipe.BY);
			}
			else
			{
				next = new(curr.X + pipe.AX, curr.Y + pipe.AY);
			}

			prev = curr;
			curr = next;
			steps++;
		}

		double result = Math.Ceiling((float)steps / 2);
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		string[] input = File.ReadAllLines("Day10\\input.txt");
		string[,] grid = new string[input.Length, input[0].Length];
		Point start = new();

		for (int y = 0; y < input.Length; y++)
		{
			string[] line = [.. input[y].Select(char.ToString)];
			for (int x = 0; x < line.Length; x++)
			{
				grid[y, x] = line[x];
				if (line[x] == "S")
				{
					start = new(x, y);
				}
			}
		}

		Dictionary<string, Pipe> pipeTypes = new()
		{
			["|"] = new(0, -1, 0, 1),
			["-"] = new(-1, 0, 1, 0),
			["L"] = new(0, -1, 1, 0),
			["J"] = new(0, -1, -1, 0),
			["7"] = new(-1, 0, 0, 1),
			["F"] = new(1, 0, 0, 1),
			["."] = new(0, 0, 0, 0),
		};

		List<Point> nextMoves =
		[
			new(start.X - 1, start.Y),
			new(start.X + 1, start.Y),
			new(start.X, start.Y - 1),
			new(start.X, start.Y + 1)
		];

		Point curr = new();
		for (int i = 0; i < nextMoves.Count; i++)
		{
			Point move = nextMoves[i];
			if (Connects(start.X, start.Y, move.X, move.Y, pipeTypes[grid[move.Y, move.X]]))
			{
				curr = move;
				break;
			}
		}

		Point prev = start;
		long steps = 0;

		int[,] path = new int[grid.GetLength(0), grid.GetLength(1)];
		string[,] pathStr = new string[grid.GetLength(0), grid.GetLength(1)];

		path[start.Y, start.X] = 1;
		pathStr[start.Y, start.X] = "7";

		while (curr.X != start.X || curr.Y != start.Y)
		{
			Pipe pipe = pipeTypes[grid[curr.Y, curr.X]];
			path[curr.Y, curr.X] = 1;
			pathStr[curr.Y, curr.X] = grid[curr.Y, curr.X];

			Point next;
			if ((curr.X + pipe.AX) == prev.X && (curr.Y + pipe.AY) == prev.Y)
			{
				next = new(curr.X + pipe.BX, curr.Y + pipe.BY);
			}
			else
			{
				next = new(curr.X + pipe.AX, curr.Y + pipe.AY);
			}

			prev = curr;
			curr = next;
			steps++;
		}

		int resizeFactor = 3;
		int[,] resizedGrid = ScaleUp(pathStr, resizeFactor, pipeTypes);

		int totalContained = Search(resizedGrid) / resizeFactor;
		int[,] scaledDown = ScaleDown(resizedGrid, resizeFactor);

		int unvisited = 0;
		for (int y = 0; y < scaledDown.GetLength(0); y++)
		{
			for (int x = 0; x < scaledDown.GetLength(1); x++)
			{
				if (scaledDown[y, x] == 0)
				{
					unvisited++;
				}
			}
		}

		Console.WriteLine($"[Part 2] Answer: {unvisited}");
	}

	static int Search(int[,] grid)
	{
		int height = grid.GetLength(0);
		int width = grid.GetLength(1);

		bool[,] visited = new bool[height, width];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				visited[y, x] = grid[y, x] == 1;
			}
		}

		Queue<Point> queue = new();
		queue.Enqueue(new(0, 0));
		queue.Enqueue(new(0, height - 1));
		queue.Enqueue(new(width - 1, 0));
		queue.Enqueue(new(width - 1, height - 1));

		while (queue.Count > 0)
		{
			Point curr = queue.Dequeue();
			if (curr.X < 0 || curr.X >= width || curr.Y < 0 || curr.Y >= height || visited[curr.Y, curr.X])
			{
				continue;
			}

			visited[curr.Y, curr.X] = true;
			queue.Enqueue(new(curr.X - 1, curr.Y));
			queue.Enqueue(new(curr.X + 1, curr.Y));
			queue.Enqueue(new(curr.X, curr.Y - 1));
			queue.Enqueue(new(curr.X, curr.Y + 1));
		}

		int unvisited = 0;
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				bool cellVisited = visited[y, x];
				if (!cellVisited)
				{
					unvisited++;
				}
				grid[y, x] = cellVisited ? 1 : 0;
			}
		}

		return unvisited;
	}

	static int[,] ScaleUp(string[,] input, int resizeFactor, Dictionary<string, Pipe> pipeTypes)
	{
		int[,] resized = new int[
			input.GetLength(0) * resizeFactor,
			input.GetLength(1) * resizeFactor];
		
		for (int x = 0; x < resized.GetLength(0); x++)
		{
			for (int y = 0; y < resized.GetLength(1); y++)
			{
				resized[x, y] = 0;
			}
		}

		for (int x = 0; x < input.GetLength(0); x++)
		{
			for (int y = 0; y < input.GetLength(1); y++)
			{
				if (input[x, y] == null)
				{
					continue;
				}

				Pipe pipe = pipeTypes[input[x, y]];

				for (int i = 0; i < resizeFactor; i++)
				{
					resized[
						(x * resizeFactor) + (pipe.AY * i),
						(y * resizeFactor) + (pipe.AX * i)] = 1;
				}

				for (int i = 0; i < resizeFactor; i++)
				{
					resized[
						(x * resizeFactor) + (pipe.BY * i),
						(y * resizeFactor) + (pipe.BX * i)] = 1;
				}
			}
		}

		return resized;
	}

	static int[,] ScaleDown(int[,] input, int shrinkFactor)
	{
		int[,] resized = new int[
			input.GetLength(0) / shrinkFactor,
			input.GetLength(1) / shrinkFactor];

		for (int x = 0; x < resized.GetLength(0); x++)
		{
			for (int y = 0; y < resized.GetLength(1); y++)
			{
				resized[x, y] = 0;
			}
		}

		for (int x = 0; x < resized.GetLength(0); x++)
		{
			for (int y = 0; y < resized.GetLength(1); y++)
			{
				resized[x, y] = input[x * shrinkFactor, y * shrinkFactor];
			}
		}

		return resized;
	}

	static bool Connects(int originX, int originY, int targetX, int targetY, Pipe pipe)
	{
		bool side1Connects =
			(targetX + pipe.AX) == originX &&
			(targetY + pipe.AY) == originY;
		bool side2Connects =
			(targetX + pipe.BX) == originX &&
			(targetY + pipe.BY) == originY;
		return side1Connects || side2Connects;
	}
}
