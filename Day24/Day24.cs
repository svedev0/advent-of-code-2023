using Microsoft.Z3;

namespace advent_of_code_2023;

public class Day24
{
	record Position(double X, double Y, double Z = 0)
	{
		public Position Move(double x, double y, double z)
		{
			return new(X + x, Y + y, Z + z);
		}

		public bool InBounds(double min, double max)
		{
			return X >= min && X <= max && Y >= min && Y <= max;
		}
	}

	record Hail(Position Position, Position Velocity, double Slope = 0, double Intersect = 0)
	{
		public Hail Move()
		{
			return this with
			{
				Position = Position.Move(Velocity.X, Velocity.Y, Velocity.Z)
			};
		}
	}

	public static void SolvePart1()
	{
		string[] input = File.ReadAllLines("Day24\\input.txt");
		List<Hail> hails = GetHails(input);

		long min = 200_000_000_000_000;
		long max = 400_000_000_000_000;

		int result = 0;
		HashSet<Hail> visited = [];
		const int b = -1;

		foreach (var first in hails)
		{
			visited.Add(first);
			double a1 = first.Slope;
			double c1 = first.Intersect;
			double x1 = first.Position.X;
			double y1 = first.Position.Y;
			double vx1 = first.Velocity.X;
			double vy1 = first.Velocity.Y;

			var notVisited = hails.Where(h => !visited.Contains(h));
			
			foreach (var second in notVisited)
			{
				double a2 = second.Slope;
				double c2 = second.Intersect;
				double x2 = second.Position.X;
				double y2 = second.Position.Y;
				double vx2 = second.Velocity.X;
				double vy2 = second.Velocity.Y;

				if (a1 == a2)
				{
					continue;
				}

				var x = (b * c2 - b * c1) / (a1 * b - a2 * b);
				var y = (a2 * c1 - a1 * c2) / (a1 * b - a2 * b);

				if (x - x1 < 0 != vx1 < 0 ||
					y - y1 < 0 != vy1 < 0)
				{
					continue;
				}

				if (x - x2 < 0 != vx2 < 0 ||
					y - y2 < 0 != vy2 < 0)
				{
					continue;
				}

				if (new Position(x, y).InBounds(min, max))
				{
					result++;
				}
			}
		}

		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		string[] input = File.ReadAllLines("Day24\\input.txt");
		List<Hail> hails = GetHails(input);
		long result = Solve(hails);
		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static List<Hail> GetHails(string[] input)
	{
		List<Hail> hails = [];

		foreach (var line in input)
		{
			string[] split = line.Split('@',
				StringSplitOptions.RemoveEmptyEntries |
				StringSplitOptions.TrimEntries);

			double[] position = split[0]
				.Split(',',
					StringSplitOptions.RemoveEmptyEntries |
					StringSplitOptions.TrimEntries)
				.Select(double.Parse)
				.ToArray();

			double[] velocity = split[1]
				.Split(',',
					StringSplitOptions.RemoveEmptyEntries |
					StringSplitOptions.TrimEntries)
				.Select(double.Parse)
				.ToArray();

			Hail hail = new(
				new(position[0], position[1], position[2]),
				new(velocity[0], velocity[1], velocity[2])
			);

			Hail next = hail.Move();

			double slope = (next.Position.Y - hail.Position.Y) /
				(next.Position.X - hail.Position.X);
			double intercept = next.Position.Y - slope * next.Position.X;

			hails.Add(hail with { Slope = slope, Intersect = intercept });
		}

		return hails;
	}

	private static long Solve(List<Hail> hails)
	{
		var ctx = new Context();
		var solver = ctx.MkSolver();

		var x = ctx.MkIntConst("x");
		var y = ctx.MkIntConst("y");
		var z = ctx.MkIntConst("z");

		var vx = ctx.MkIntConst("vx");
		var vy = ctx.MkIntConst("vy");
		var vz = ctx.MkIntConst("vz");

		for (var i = 0; i < 3; i++)
		{
			var t = ctx.MkIntConst($"t{i}");
			var hail = hails[i];

			var px = ctx.MkInt(Convert.ToInt64(hail.Position.X));
			var py = ctx.MkInt(Convert.ToInt64(hail.Position.Y));
			var pz = ctx.MkInt(Convert.ToInt64(hail.Position.Z));

			var pvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
			var pvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
			var pvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

			var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx));
			var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy));
			var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz));

			var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx));
			var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy));
			var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz));

			solver.Add(t >= 0);
			solver.Add(ctx.MkEq(xLeft, xRight));
			solver.Add(ctx.MkEq(yLeft, yRight));
			solver.Add(ctx.MkEq(zLeft, zRight));
		}

		solver.Check();
		var model = solver.Model;

		var rx = model.Eval(x);
		var ry = model.Eval(y);
		var rz = model.Eval(z);

		return Convert.ToInt64(rx.ToString()) +
			Convert.ToInt64(ry.ToString()) +
			Convert.ToInt64(rz.ToString());
	}
}
