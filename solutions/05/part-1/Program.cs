string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\05\\input.txt");

var answer = long.MaxValue;
var mapIndex = -1;
var seeds = new List<long>();
var maps = new List<Map>[7];
foreach (var line in lines)
{
    if (line.StartsWith("seeds", StringComparison.InvariantCultureIgnoreCase))
        foreach(var seed in line.Replace("seeds:", "", StringComparison.InvariantCultureIgnoreCase).Trim().Split(' '))
            seeds.Add(long.Parse(seed));
    else if (string.IsNullOrEmpty(line))
            mapIndex++;
    else if (char.IsDigit(line[0]))
    {
        if (maps[mapIndex] == null)
            maps[mapIndex] = new List<Map>();

        var numbers = line.Split(' ');
        maps[mapIndex].Add(new Map
        {
            SourceRangeStart = long.Parse(numbers[1]),
            DestinationRangeStart = long.Parse(numbers[0]),
            RangeLength = long.Parse(numbers[2])
        });
    }
}

foreach (var seed in seeds)
{
    var input = seed;
    for (var i = 0; i < maps.Length;  i++)
    {
        foreach (var map in maps[i])
        {
            if (input >= map.SourceRangeStart && input <= map.SourceRangeStart + map.RangeLength)
            {
                input = map.DestinationRangeStart + input - map.SourceRangeStart;
                break;
            }
        }
    }

    if (input < answer)
        answer = input;
}

Console.WriteLine(answer);

struct Map
{
    public long SourceRangeStart;
    public long DestinationRangeStart;
    public long RangeLength;
}