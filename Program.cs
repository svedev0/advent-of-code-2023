namespace advent_of_code_2023;

public class Program
{
	private static readonly int YEAR = 2023;
	private static readonly int DAY = 8;

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
			case 3:
				Day03.SolvePart1();
				Day03.SolvePart2();
				break;
			case 4:
				Day04.SolvePart1();
				Day04.SolvePart2();
				break;
			case 5:
				Day05.SolvePart1();
				Day05.SolvePart2();
				break;
			case 6:
				Day06.SolvePart1();
				Day06.SolvePart2();
				break;
			case 7:
				Day07.SolvePart1();
				Day07.SolvePart2();
				break;
			case 8:
				Day08.SolvePart1();
				Day08.SolvePart2();
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
