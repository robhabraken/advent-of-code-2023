string[] lines = File.ReadAllLines("..\\..\\..\\input.txt");

var answer = 0;
long time = 0;
long distance = 0;
foreach (var line in lines)
{
    if (line.StartsWith("Time", StringComparison.InvariantCultureIgnoreCase))
        time = long.Parse(line.Replace("Time:", "", StringComparison.InvariantCultureIgnoreCase).Replace(" ", ""));
    if (line.StartsWith("Distance", StringComparison.InvariantCultureIgnoreCase))
        distance = long.Parse(line.Replace("Distance:", "", StringComparison.InvariantCultureIgnoreCase).Replace(" ", ""));
}

for (var buttonPressed = 0; buttonPressed < time; buttonPressed++)
    if (buttonPressed * (time - buttonPressed) > distance)
        answer++;

Console.WriteLine(answer);