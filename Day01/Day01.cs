namespace advent_of_code_2023;

public class Day01
{
	public static void Solve()
	{
		List<string> input = [.. File.ReadAllLines("Day01\\input.txt")];
		List<int> lineValues = [];

		for (int i = 0; i < input.Count; i++)
		{
			char[] chars = input[i].ToCharArray();
			List<char> nums = chars.Where(char.IsDigit).ToList();

			var lineValue = string.Join("", nums.First(), nums.Last());
			lineValues.Add(int.Parse(lineValue));

			// See the value of each line:
			// Console.WriteLine("Line {0,4}: {1}", i + 1, lineValue);
		};

		Console.WriteLine($"Sum of lines: {lineValues.Sum()}");
	}
}
