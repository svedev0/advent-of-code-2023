namespace advent_of_code_2023;

public class Day03
{
	struct Number
	{
		public int Value { get; set; }
		public (int Row, int Col) Start { get; set; }
		public (int Row, int Col) End { get; set; }
	}

	struct Symbol
	{
		public char Value { get; set; }
		public (int Row, int Col) Pos { get; set; }
	}

	private static bool IsAdjacent(Number num, Symbol sym)
	{
		return Math.Abs(
			sym.Pos.Row - num.Start.Row) <= 1 &&
			sym.Pos.Col >= num.Start.Col - 1 &&
			sym.Pos.Col <= num.End.Col + 1;
	}

	public static void SolvePart1()
	{
		string[] input = File.ReadAllLines("Day03\\input.txt");

		List<Number> nums = [];
		List<Symbol> syms = [];

		for (int row = 0; row < input.Length; row++)
		{
			Number currNum = new();
			List<int> digits = [];

			for (int col = 0; col < input[row].Length; col++)
			{
				if (input[row][col] == '.')
				{
					continue;
				}

				if (int.TryParse($"{input[row][col]}", out var digit))
				{
					digits.Add(digit);

					if (digits.Count == 1)
					{
						currNum.Start = (row, col);
					}

					while (col < input[row].Length - 1 &&
						int.TryParse($"{input[row][col + 1]}", out digit))
					{
						digits.Add(digit);
						col++;
					}

					currNum.End = (row, col);
					currNum.Value = int.Parse(string.Join("", digits));
					nums.Add(currNum);
					currNum = new Number();
					digits.Clear();
				}
				else
				{
					syms.Add(new Symbol
					{
						Value = input[row][col],
						Pos = (row, col)
					});
				}
			}
		}

		int sum = nums
			.Where(num => syms.Any(sym => IsAdjacent(num, sym)))
			.Sum(num => num.Value);

		Console.WriteLine($"[Part 1] Answer: {sum}");
	}
}
