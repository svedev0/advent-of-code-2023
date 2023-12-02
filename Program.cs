namespace advent_of_code_2023;

public class Program
{
	private static readonly int YEAR = 2023;
	private static readonly int DAY = 2;

	public static void Main(string[] _)
	{
		Welcome();

		switch (DAY)
		{
			case 1:
				Day01.SolvePart1();
				Day01.SolvePart2();
				break;
			case 2:
				Day02.SolvePart1();
				Day02.SolvePart2();
				break;
			default:
				Console.WriteLine("Invalid day");
				break;
		}
	}

	private static void Welcome()
	{
		string messsage = $"""
			===== Advent of Code {YEAR} =====

			Day {DAY}:

			""";
		Console.WriteLine(messsage);
	}
}
