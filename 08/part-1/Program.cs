string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 0;
string instructions = string.Empty;
var steps = new Dictionary<string, NetworkNode>();
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(instructions))
        instructions = line;
    else if (!string.IsNullOrEmpty(line))
        steps.Add(line[..3], new NetworkNode(line));
}

string iamHere = "AAA";
int instructionIndex = 0;
while (!iamHere.Equals("ZZZ", StringComparison.InvariantCultureIgnoreCase))
{
    if (instructionIndex == instructions.Length)
        instructionIndex = 0;

    var destination = steps[iamHere];
    if (instructions[instructionIndex].Equals('L'))
        iamHere = destination.Left;
    else
        iamHere = destination.Right;

    instructionIndex++;
    answer++;
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