string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var pulseQueue = new Queue<Pulse>();
var modules = new Dictionary<string, Module>();

var answer = 0;
var pulseCount = new int[2];

// read input
foreach (var line in lines)
{
    var module = new Module(line);
    modules.Add(module.name, module);
}

// register inputs for all conjuction modules
foreach (var module in modules.Values)
    if (module.type == ModuleType.Conjuction)
        foreach (var otherModule in modules.Values)
            foreach (var dest in otherModule.destinations)
                if (dest.Equals(module.name))
                    module.RegisterInput(otherModule);

// create memory store to keep track of the last pulse for each input
foreach (var module in modules.Values)
    if (module.type == ModuleType.Conjuction)
        module.CreateMemoryStore();

// for part 1 I guess I can just press the button a 1000 times
for (var i = 0; i < 1000; i++)
{
    // press the button
    modules["broadcaster"].ProcessPulse(pulseQueue, modules, new Pulse("button", "broadcaster", 0));
    pulseCount[0]++;

    // handle all of the pulse in sequence using a queue
    while (pulseQueue.Any())
    {
        var pulse = pulseQueue.Dequeue();
        if (modules.ContainsKey(pulse.to))
            modules[pulse.to].ProcessPulse(pulseQueue, modules, pulse);
    }
}

// count the number of low and high pulses
foreach (var module in modules.Values)
    for (var i = 0; i < 2; i++)
        pulseCount[i] += module.pulseCount[i];

answer = pulseCount[0] * pulseCount[1];

Console.WriteLine(answer);

class Module
{
    public string name;
    public ModuleType type;
    public List<string> destinations;

    public List<string> inputs;

    public bool onOff;
    public Dictionary<string, int> memory;

    public int[] pulseCount;

    public Module(string input)
    {
        switch(input[0])
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
        pulseCount = new int[2];
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

    public void ProcessPulse(Queue<Pulse> queue, Dictionary<string, Module> modules, Pulse pulse)
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

            SendPulse(queue, modules, output);
        }
    }

    private void SendPulse(Queue<Pulse> queue, Dictionary<string, Module> modules, int pulse)
    {
        foreach (var dest in destinations)
        {
            //var pulseName = "-low";
            //if (pulse == 1)
            //    pulseName = "-high";
            //Console.WriteLine(name + " " + pulseName + "-> " + dest);

            pulseCount[pulse]++;

            queue.Enqueue(new Pulse(this.name, dest, pulse));
        }
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