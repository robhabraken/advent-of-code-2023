string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 1;
var times = new List<int>();
var distances = new List<int>();
foreach (var line in lines)
{
   if (line.StartsWith("Time", StringComparison.InvariantCultureIgnoreCase))
        foreach(var element in line.Replace("Time:", "", StringComparison.InvariantCultureIgnoreCase).Split(' '))
            if(!string.IsNullOrEmpty(element))
                times.Add(int.Parse(element));
    if (line.StartsWith("Distance", StringComparison.InvariantCultureIgnoreCase))
        foreach (var element in line.Replace("Distance:", "", StringComparison.InvariantCultureIgnoreCase).Split(' '))
            if (!string.IsNullOrEmpty(element))
                distances.Add(int.Parse(element));
}

for (var i = 0; i < times.Count; i++)
{
    var count = 0;
    for (var buttonPressed = 0; buttonPressed < times[i]; buttonPressed++)
        if (buttonPressed * (times[i] - buttonPressed) > distances[i])
            count++;
    answer *= count;
}

Console.WriteLine(answer);