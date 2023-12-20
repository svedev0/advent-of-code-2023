namespace advent_of_code_2023;

public class Day20
{
	enum PulseType { None, Low, High }

	interface IModule
	{
		string Name { get; set; }
		Dictionary<string, PulseType> Targets { get; set; }
		void Process(PulseType pulse);
	}

	class Transmitter : IModule
	{
		public Transmitter(string name, List<string> targets)
		{
			Name = name;
			foreach (var target in targets)
			{
				Targets[target] = PulseType.Low;
			}
		}
		public string Name { get; set; }
		public Dictionary<string, PulseType> Targets { get; set; } = new();
		public virtual void Process(PulseType pulse)
		{
			foreach (var name in Targets.Keys)
			{
				Targets[name] = pulse;
			}
		}
	}

	class FlipFlop(string name, List<string> targets) : Transmitter(name, targets)
	{
		public bool PoweredOn { get; set; }
		public override void Process(PulseType pulse)
		{
			var output = PulseType.None;
			if (pulse == PulseType.Low)
			{
				output = PoweredOn ? PulseType.Low : PulseType.High;
				PoweredOn = !PoweredOn;
			}
			foreach (var targetName in Targets.Keys)
			{
				Targets[targetName] = output;
			}
		}
	}

	class Conjunction(string name, List<string> targets) : Transmitter(name, targets)
	{
		public Dictionary<string, PulseType> Inputs { get; set; } = [];
		public override void Process(PulseType pulse)
		{
			foreach (var name in Targets.Keys)
			{
				Targets[name] = Inputs.Values.All(x => x == PulseType.High) ? PulseType.Low : PulseType.High;
			}
		}
	}

	public static void SolvePart1()
	{
		List<string> input = [.. File.ReadAllLines("Day20\\input.txt")];
		Dictionary<string, IModule> modules = [];

		foreach (var line in input)
		{
			string[] split = line.Split(" -> ");
			string name = split[0];
			List<string> targets = [.. split[1].Split(',',
				StringSplitOptions.RemoveEmptyEntries |
				StringSplitOptions.TrimEntries)];

			if (name == "broadcaster")
			{
				modules[name] = new Transmitter(name, targets);
			}
			else if (name.StartsWith('%'))
			{
				modules[name[1..]] = new FlipFlop(name[1..], targets);
			}
			else if (name.StartsWith('&'))
			{
				modules[name[1..]] = new Conjunction(name[1..], targets);
			}
		}

		foreach (var module in modules.Values)
		{
			if (module is not Conjunction conjunction)
			{
				continue;
			}

			List<IModule> inputs = modules.Values
				.Where(m => m.Targets.ContainsKey(module.Name))
				.ToList();

			foreach (var inputModule in inputs)
			{
				conjunction.Inputs[inputModule.Name] = PulseType.Low;
			}
		}

		int lowPulses = 0;
		int highPulses = 0;
		Queue<(IModule Module, PulseType PulseType)> queue = new();
		PulseType currentPulse = PulseType.Low;
		int count = 0;

		IModule rxSource = modules.Values.First(m => m.Targets.ContainsKey("rx"));
		List<IModule> rxSourceInputs = modules.Values.Where(m =>
			m.Targets.ContainsKey(rxSource.Name)).ToList();
		Dictionary<string, (int Count, bool Done)> rxSourceInputCounts = [];

		rxSourceInputs.ForEach(rxs => rxSourceInputCounts[rxs.Name] = (0, false));

		long result = 0;

		while (true)
		{
			if (count == 1000)
			{
				result = lowPulses * highPulses;
				break;
			}

			count++;

			queue.Enqueue((modules["broadcaster"], currentPulse));

			if (currentPulse == PulseType.Low)
			{
				lowPulses++;
			}
			else
			{
				highPulses++;
			}

			while (queue.TryDequeue(out var current))
			{
				current.Module.Process(current.PulseType);

				foreach (var target in current.Module.Targets.Keys)
				{
					var targetPulse = current.Module.Targets[target];

					switch (targetPulse)
					{
						case PulseType.Low:
							lowPulses++;
							break;
						case PulseType.High:
							highPulses++;
							break;
						default:
							break;
					}

					if (targetPulse != PulseType.None)
					{
						if (!modules.ContainsKey(target))
						{
							continue;
						}

						if (modules[target] is Conjunction conjunction)
						{
							conjunction.Inputs[current.Module.Name] = targetPulse;
						}

						queue.Enqueue((modules[target], targetPulse));
					}

					if (queue.Count == 0)
					{
						currentPulse = current.PulseType == PulseType.Low ?
							PulseType.High :
							PulseType.Low;
					}
				}
			}
		}

		Console.WriteLine($"[Part 1] Answer: {result}");
	}

	public static void SolvePart2()
	{
		List<string> input = [.. File.ReadAllLines("Day20\\input.txt")];
		Dictionary<string, IModule> modules = [];

		foreach (var line in input)
		{
			string[] split = line.Split(" -> ");
			string name = split[0];
			List<string> targets = [.. split[1].Split(',',
				StringSplitOptions.RemoveEmptyEntries |
				StringSplitOptions.TrimEntries)];

			if (name == "broadcaster")
			{
				modules[name] = new Transmitter(name, targets);
			}
			else if (name.StartsWith('%'))
			{
				modules[name[1..]] = new FlipFlop(name[1..], targets);
			}
			else if (name.StartsWith('&'))
			{
				modules[name[1..]] = new Conjunction(name[1..], targets);
			}
		}

		foreach (var module in modules.Values)
		{
			if (module is not Conjunction conjunction)
			{
				continue;
			}

			List<IModule> inputs = modules.Values
				.Where(m => m.Targets.ContainsKey(module.Name))
				.ToList();

			foreach (var inputModule in inputs)
			{
				conjunction.Inputs[inputModule.Name] = PulseType.Low;
			}
		}

		int lowPulses = 0;
		int highPulses = 0;
		Queue<(IModule Module, PulseType PulseType)> queue = new();
		PulseType currentPulse = PulseType.Low;
		int count = 0;

		IModule rxSource = modules.Values.First(m => m.Targets.ContainsKey("rx"));
		List<IModule> rxSourceInputs = modules.Values.Where(m =>
			m.Targets.ContainsKey(rxSource.Name)).ToList();
		Dictionary<string, (int Count, bool Done)> rxSourceInputCounts = [];

		rxSourceInputs.ForEach(rxs => rxSourceInputCounts[rxs.Name] = (0, false));

		long result = 0;

		while (true)
		{
			count++;
			queue.Enqueue((modules["broadcaster"], currentPulse));

			if (currentPulse == PulseType.Low)
			{
				lowPulses++;
			}
			else
			{
				highPulses++;
			}

			if (rxSourceInputCounts.All(rxs => rxs.Value.Done))
			{
				result = LCM(rxSourceInputCounts.Select(rxs =>
					rxs.Value.Count).ToArray());
				break;
			}

			while (queue.TryDequeue(out var current))
			{
				current.Module.Process(current.PulseType);

				foreach (var target in current.Module.Targets.Keys)
				{
					var targetPulse = current.Module.Targets[target];

					if (target == "rx")
					{
						if (current.Module is not Conjunction conjunction)
						{
							continue;
						}

						var highInputs = conjunction.Inputs.Where(conjunctionInput =>
							conjunctionInput.Value == PulseType.High);

						foreach (var conjunctionInput in highInputs)
						{
							rxSourceInputCounts[conjunctionInput.Key] = (count, true);
						}
					}

					switch (targetPulse)
					{
						case PulseType.Low:
							lowPulses++;
							break;
						case PulseType.High:
							highPulses++;
							break;
						default:
							break;
					}

					if (targetPulse != PulseType.None)
					{
						if (!modules.ContainsKey(target))
						{
							continue;
						}

						if (modules[target] is Conjunction conjunction)
						{
							conjunction.Inputs[current.Module.Name] = targetPulse;
						}

						queue.Enqueue((modules[target], targetPulse));
					}

					if (queue.Count == 0)
					{
						currentPulse = current.PulseType == PulseType.Low ?
							PulseType.High :
							PulseType.Low;
					}
				}
			}
		}

		Console.WriteLine($"[Part 2] Answer: {result}");
	}

	private static long LCM(IEnumerable<int> numbers)
	{
		return numbers
			.Select(Convert.ToInt64)
			.Aggregate((a, b) => a * b / GCD(a, b));
	}

	private static long GCD(long a, long b)
	{
		return b == 0 ? a : GCD(b, a % b);
	}
}
