string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\20\\input.txt");

var pulseQueue = new Queue<Pulse>();
var modules = new Dictionary<string, Module>();
var finalMachineName = "rx";

long answer = 1;

// read input
foreach (var line in lines)
{
    var module = new Module(line);
    modules.Add(module.name, module);
}

// register inputs for all modules
foreach (var module in modules.Values)
    foreach (var otherModule in modules.Values)
        foreach (var dest in otherModule.destinations)
            if (dest.Equals(module.name))
                module.RegisterInput(otherModule);

// create memory store to keep track of the last pulse for each input
foreach (var module in modules.Values)
    if (module.type == ModuleType.Conjuction)
        module.CreateMemoryStore();

// part 2: find the last node (module) that leads to the final machine
var lastModule = new Module();
foreach (var module in modules.Values)
    foreach (var dest in module.destinations)
        if (dest.Equals(finalMachineName))
            lastModule = module;

// find the cycle size for each module
var buttonPressed = 0;
while (true)
{
    // press the button
    modules["broadcaster"].ProcessPulse(pulseQueue, modules, new Pulse("button", "broadcaster", 0), ++buttonPressed);

    // handle all of the pulses in sequence using a queue
    while (pulseQueue.Any())
    {
        var pulse = pulseQueue.Dequeue();
        if (modules.ContainsKey(pulse.to))
            modules[pulse.to].ProcessPulse(pulseQueue, modules, pulse, buttonPressed);
    }

    // check if all cycles are discovered
    var allCyclesFound = true;
    foreach (var module in modules.Values)
    {
        if (module.type == ModuleType.FlipFlop || module.type == ModuleType.Conjuction)
            if (module.cycleSize == 0)
            {
                allCyclesFound = false;
                break;
            }
    }

    // stop pressing the button once we have found all of the cycle sizes
    if (allCyclesFound)
        break;
}

// find the first common point for all last cycles in the module chain (the least common multiple)
// as those modules all have to be sending a high pulse at the same time in order for the last module to output a low pulse
var primeFactorLists = new List<List<int>>();
var highestInputValue = 0;
foreach (var inputModule in lastModule.inputs)
{
    var cycleSize = modules[inputModule].cycleSize;
    primeFactorLists.Add(FindPrimeFactors(cycleSize));
    if (cycleSize > highestInputValue)
        highestInputValue = cycleSize;
}

var mergedPrimeFactors = new List<int>();
for (var prime = highestInputValue; prime > 1; prime--)
{
    var occurrences = 0;
    foreach (var primeList in primeFactorLists)
    {
        var count = primeList.Where(x => x.Equals(prime)).Count();
        if (count > occurrences)
            occurrences = count;
    }
    if (occurrences > 0)
        mergedPrimeFactors.Add(prime);
}

foreach (var prime in mergedPrimeFactors)
    answer *= prime;

Console.WriteLine(answer);

List<int> FindPrimeFactors(int value)
{
    var primeFactors = new List<int>();
    for (var division = 2; value > 1; division++)
    {
        if (value % division == 0)
        {
            while (value % division == 0)
            {
                value /= division;
                primeFactors.Add(division);
            }
        }
    }
    primeFactors.Sort();
    primeFactors.Reverse();
    return primeFactors;
}

class Module
{
    public string name;
    public ModuleType type;
    public List<string> destinations;

    public List<string> inputs;

    public bool onOff;
    public Dictionary<string, int> memory;

    public int lastOutput;
    public int cycleSize;

    public Module() { }

    public Module(string input)
    {
        switch (input[0])
        {
            case '%': type = ModuleType.FlipFlop; break;
            case '&': type = ModuleType.Conjuction; break;
            case 'b': type = ModuleType.Broadcast; break;
        }

        if (type == ModuleType.Broadcast)
            name = input.Split(' ')[0];
        else
            name = input.Split(' ')[0][1..];

        destinations = new List<string>();
        foreach (var x in input.Substring(input.IndexOf('>') + 1).Split(','))
            destinations.Add(x.Trim());

        inputs = new List<string>();
    }

    public void RegisterInput(Module module)
    {
        inputs.Add(module.name);
    }

    public void CreateMemoryStore()
    {
        memory = new Dictionary<string, int>();
        foreach (var input in inputs)
            memory.Add(input, 0);
    }

    public void ProcessPulse(Queue<Pulse> queue, Dictionary<string, Module> modules, Pulse pulse, int buttonPressed)
    {
        if (type == ModuleType.Broadcast)
        {
            SendPulse(queue, modules, pulse.value);
        }
        else if (type == ModuleType.FlipFlop)
        {
            if (pulse.value == 1)
                return;

            int output = 0;
            if (!onOff)
                output = 1;

            onOff = !onOff;

            if (lastOutput != output && cycleSize == 0)
                cycleSize = buttonPressed;
            lastOutput = output;
            
            SendPulse(queue, modules, output);
        }
        else if (type == ModuleType.Conjuction)
        {
            memory[pulse.from] = pulse.value;

            var allHigh = true;
            foreach (var mem in memory.Values)
                if (mem == 0)
                    allHigh = false;

            var output = 0;
            if (!allHigh)
                output = 1;

            if (lastOutput != output && cycleSize == 0)
                cycleSize = buttonPressed;
            lastOutput = output;

            SendPulse(queue, modules, output);
        }
    }

    private void SendPulse(Queue<Pulse> queue, Dictionary<string, Module> modules, int pulse)
    {
        foreach (var dest in destinations)
            queue.Enqueue(new Pulse(this.name, dest, pulse));
    }
}

class Pulse
{
    public string from;
    public string to;
    public int value;

    public Pulse(string from, string to, int value)
    {
        this.from = from;
        this.to = to;
        this.value = value;
    }
}

enum ModuleType
{
    Broadcast,
    FlipFlop,
    Conjuction
}