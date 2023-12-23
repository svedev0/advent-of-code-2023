namespace advent_of_code_2023;

public class Day22
{
	record Range(int Begin, int End);
	record Block(Range X, Range Y, Range Z)
	{
		public int Top => Z.End;
		public int Bottom => Z.Begin;
	}
	record Supports(
		Dictionary<Block, HashSet<Block>> BlocksAbove,
		Dictionary<Block, HashSet<Block>> BlocksBelow
	);

	public static void SolvePart1()
	{
		string input = File.ReadAllText("Day22\\input.txt");
		int result = Explode(input).Count(x => x == 0);
		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		string input = File.ReadAllText("Day22\\input.txt");
		int result = Explode(input).Sum();
		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static Block[] ParseBlocks(string input)
	{
		return (
			from line in input.Split('\n')
			let numbers = line.Split(',', '~').Select(int.Parse).ToArray()
			select new Block(
				X: new Range(numbers[0], numbers[3]),
				Y: new Range(numbers[1], numbers[4]),
				Z: new Range(numbers[2], numbers[5])
			)
		).ToArray();
	}

	private static Block[] Fall(Block[] blocks)
	{
		blocks = blocks.OrderBy(block => block.Bottom).ToArray();

		for (int i = 0; i < blocks.Length; i++)
		{
			int newBottom = 1;
			for (int j = 0; j < i; j++)
			{
				if (IntersectsXY(blocks[i], blocks[j]))
				{
					newBottom = Math.Max(newBottom, blocks[j].Top + 1);
				}
			}
			int fall = blocks[i].Bottom - newBottom;
			blocks[i] = blocks[i] with
			{
				Z = new Range(blocks[i].Bottom - fall, blocks[i].Top - fall)
			};
		}
		return blocks;
	}

	private static Supports GetSupports(Block[] blocks)
	{
		Dictionary<Block, HashSet<Block>> blocksAbove = blocks
			.ToDictionary(b => b, _ => new HashSet<Block>());
		Dictionary<Block, HashSet<Block>> blocksBelow = blocks
			.ToDictionary(b => b, _ => new HashSet<Block>());
		
		for (int i = 0; i < blocks.Length; i++)
		{
			for (int j = i + 1; j < blocks.Length; j++)
			{
				bool zNeighbours = blocks[j].Bottom == 1 + blocks[i].Top;
				
				if (zNeighbours && IntersectsXY(blocks[i], blocks[j]))
				{
					blocksBelow[blocks[j]].Add(blocks[i]);
					blocksAbove[blocks[i]].Add(blocks[j]);
				}
			}
		}

		return new Supports(blocksAbove, blocksBelow);
	}

	private static bool Intersects(Range r1, Range r2) =>
		r1.Begin <= r2.End && r2.Begin <= r1.End;

	private static bool IntersectsXY(Block b1, Block b2) =>
		Intersects(b1.X, b2.X) && Intersects(b1.Y, b2.Y);

	private static IEnumerable<int> Explode(string input)
	{
		Block[] blocks = Fall(ParseBlocks(input));
		Supports supports = GetSupports(blocks);

		foreach (var desintegratedBlock in blocks)
		{
			var q = new Queue<Block>();
			q.Enqueue(desintegratedBlock);

			HashSet<Block> falling = [];
			
			while (q.TryDequeue(out var block))
			{
				falling.Add(block);

				var blocksStartFalling =
					from blockT in supports.BlocksAbove[block]
					where supports.BlocksBelow[blockT].IsSubsetOf(falling)
					select blockT;

				foreach (var blockT in blocksStartFalling)
				{
					q.Enqueue(blockT);
				}
			}

			yield return falling.Count - 1;
		}
	}
}
