string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\08\\input.txt");

long answer = 0;
string instructions = string.Empty;
var steps = new Dictionary<string, NetworkNode>();
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(instructions))
        instructions = line;
    else if (!string.IsNullOrEmpty(line))
        steps.Add(line[..3], new NetworkNode(line));
}

var startingPoints = new List<string>();
foreach (var step in steps.Keys)
{
    if (step.EndsWith('A'))
        startingPoints.Add(step);
}

var iamAlsoHere = new string[startingPoints.Count];
var breakpoints = new int[startingPoints.Count];
for (var i = 0; i < iamAlsoHere.Length; i++)
{
    iamAlsoHere[i] = startingPoints[i];

    int instructionIndex = 0;
    while (true)
    {
        if (instructionIndex == instructions.Length)
            instructionIndex = 0;

        var destination = steps[iamAlsoHere[i]];
        if (instructions[instructionIndex].Equals('L'))
            iamAlsoHere[i] = destination.Left;
        else
            iamAlsoHere[i] = destination.Right;

        instructionIndex++;
        breakpoints[i]++;

        // find breakpoint for this starting point (how many steps it takes to get to a Z for this starting point)
        if (iamAlsoHere[i].EndsWith('Z'))
            break;
    }
}

// find the least common multiple
long lcm = breakpoints[0];
while(true)
{
    var found = true;
    for (var i = 1; i < breakpoints.Length; i++)
    {
        if (lcm % breakpoints[i] > 0)
        {
            found = false;
            break;
        }
    }

    if (found)
    {
        answer = lcm;
        break;
    }

    lcm += breakpoints[0];
}

Console.WriteLine(answer);

class NetworkNode
{
    public string Left;
    public string Right;

    public NetworkNode(string input)
    {
        var destinations = input.Substring(input.IndexOf('(') + 1, 8).Split(',');
        Left = destinations[0].Trim();
        Right = destinations[1].Trim();
    }
}