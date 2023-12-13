using System.Text;

namespace advent_of_code_2023;

public class Day13
{
	private static List<string> maps = [];
	private static Dictionary<int, (int num, bool isRow)> mirrors = [];

	public static void SolvePart1()
	{
		string input = File.ReadAllText("Day13\\input.txt");
		maps = [.. input
			   .Split(["\r\n\r\n", "\r\r", "\n\n"], StringSplitOptions.None)
			   .Where(s => !string.IsNullOrWhiteSpace(s))
			   .Select(s => s.Trim())];

		int sum = 0;

		for (int i = 0; i < maps.Count; i++)
		{
			string block = maps[i];
			FindReflection(block, i, out int result);
			sum += result;
		}

		Console.WriteLine($"[Part 1] Answer: {sum}");
	}

	public static void SolvePart2()
	{
		string input = File.ReadAllText("Day13\\input.txt");
		maps = [.. input
			   .Split(["\r\n\r\n", "\r\r", "\n\n"], StringSplitOptions.None)
			   .Where(s => !string.IsNullOrWhiteSpace(s))
			   .Select(s => s.Trim())];

		int sum = 0;
		
		for (int j = 0; j < maps.Count; j++)
		{
			string block = maps[j];

			for (int i = 0; i < block.Length; i++)
			{
				if (!".#".Contains(block[i]))
				{
					continue;
				}

				StringBuilder sb = new(block);
				sb[i] = sb[i] == '.' ? '#' : '.';

				if (FindReflection(sb.ToString(), j, out int res))
				{
					sum += res;
					break;
				}

			}
		}
		
		Console.WriteLine($"[Part 2] Answer: {sum}");
	}

	private static bool FindReflection(string block, int Id, out int result)
	{
		List<string> rows = [.. block
			   .Split(["\r\n", "\r", "\n"], StringSplitOptions.None)
			   .Where(s => !string.IsNullOrWhiteSpace(s))
			   .Select(s => s.Trim())];

		List<string> cols = [.. SplitIntoColumns(block)];
		
		for (int i = 1; i < rows.Count; i++)
		{
			if (rows.Take(i).Reverse().Zip(rows.Skip(i)).All(x => x.First == x.Second))
			{
				if (mirrors.TryGetValue(Id, out (int num, bool isRow) x))
				{
					if (x.isRow && x.num == i)
					{
						continue;
					}
				}

				mirrors[Id] = (i, true);
				result = i * 100;
				return true;
			}
		}

		for (int i = 1; i < cols.Count; i++)
		{
			if (cols.Take(i).Reverse().Zip(cols.Skip(i)).All(x => x.First == x.Second))
			{
				if (mirrors.TryGetValue(Id, out (int num, bool isRow) x))
				{
					if (!x.isRow && x.num == i)
					{
						continue;
					}

				}

				mirrors[Id] = (i, false);
				result = i;
				return true;
			}
		}

		result = 0;
		return false;
	}

	private static string[] SplitIntoColumns(string input)
	{
		List<string> rows = [.. input
			   .Split(["\r\n", "\r", "\n"], StringSplitOptions.None)
			   .Where(s => !string.IsNullOrWhiteSpace(s))
			   .Select(s => s)];

		int colsCount = rows.Max(x => x.Length);
		string[] result = new string[colsCount];
		
		for (int i = 0; i < colsCount; i++)
		{
			StringBuilder sb = new();
			foreach (var row in rows)
			{
				try
				{
					sb.Append(row[i]);
				}
				catch (IndexOutOfRangeException)
				{
					sb.Append(' ');
				}
			}
			result[i] = sb.ToString();
		}

		return result;
	}
}
