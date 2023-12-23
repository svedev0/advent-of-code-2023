namespace advent_of_code_2023;

public class Program
{
	private static readonly int YEAR = 2023;
	private static readonly int DAY = 23;

	public static void Main(string[] _)
	{
		Timer timer = new();
		timer.Start();

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
			case 9:
				Day09.SolvePart1();
				Day09.SolvePart2();
				break;
			case 10:
				Day10.SolvePart1();
				Day10.SolvePart2();
				break;
			case 11:
				Day11.SolvePart1();
				Day11.SolvePart2();
				break;
			case 12:
				Day12.SolvePart1();
				Day12.SolvePart2();
				break;
			case 13:
				Day13.SolvePart1();
				Day13.SolvePart2();
				break;
			case 14:
				Day14.SolvePart1();
				Day14.SolvePart2();
				break;
			case 15:
				Day15.SolvePart1();
				Day15.SolvePart2();
				break;
			case 16:
				Day16.SolvePart1();
				Day16.SolvePart2();
				break;
			case 17:
				Day17.SolvePart1();
				Day17.SolvePart2();
				break;
			case 18:
				Day18.SolvePart1();
				Day18.SolvePart2();
				break;
			case 19:
				Day19.SolvePart1();
				Day19.SolvePart2();
				break;
			case 20:
				Day20.SolvePart1();
				Day20.SolvePart2();
				break;
			case 21:
				Day21.SolvePart1();
				Day21.SolvePart2();
				break;
			case 22:
				Day22.SolvePart1();
				Day22.SolvePart2();
				break;
			case 23:
				Day23.SolvePart1();
				Day23.SolvePart2();
				break;
			default:
				throw new Exception("Invalid day");
		}

		timer.Stop();
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

public class Timer
{
	private DateTime StartTime { get; set; }
	private DateTime EndTime { get; set; }

	public void Start()
	{
		StartTime = DateTime.Now;
	}

	public string Stop()
	{
		EndTime = DateTime.Now;
		double resultSeconds = (EndTime - StartTime).TotalSeconds;
		TimeSpan time = TimeSpan.FromSeconds(resultSeconds);

		string result;
		if (resultSeconds <= 60)
		{
			result = string.Format("{0:D2} seconds", time.Seconds);
		}
		else
		{
			result = string.Format("{0:D2} minutes {1:D2} seconds", time.Minutes, time.Seconds);
		}

		Console.WriteLine($"The program finished in {result}");
		return result;
	}
}