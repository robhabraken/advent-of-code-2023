string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\21\\input.txt");
var width = lines[0].Length;

long answer = 0;
long remainingSteps = 26501365;

var stepsToReachBorder = 65;
long mapReach = (remainingSteps - stepsToReachBorder) / width;

var startX = 0;
var startY = 0;

var map = new int[lines[0].Length, lines.Length];
var stepsToTake = new Queue<int>();

var fullOddMapCount = TotalMapCount(false);
var fullEvenMapCount = TotalMapCount(true);
var oddCornerMapCount = TotalMapCount(false, stepsToReachBorder);
var evenCornerMapCount = TotalMapCount(true, stepsToReachBorder);

if (mapReach % 2 == 0)
{
    answer += fullEvenMapCount * mapReach * mapReach;
    answer += mapReach * evenCornerMapCount;

    answer += fullOddMapCount * (long)Math.Pow(mapReach + 1, 2);
    answer -= (mapReach + 1) * oddCornerMapCount;
}
else
{
    answer += fullOddMapCount * mapReach * mapReach;
    answer += mapReach * oddCornerMapCount;

    answer += fullEvenMapCount * (long)Math.Pow(mapReach + 1, 2);
    answer -= (mapReach + 1) * evenCornerMapCount;
}

Console.WriteLine(answer);

int TotalMapCount(bool even, int minStepCount = int.MinValue)
{
    var count = 0;
    InitializeMap();

    stepsToTake.Clear();
    CountSteps(startY * width + startX);

    while (stepsToTake.Any())
        CountSteps(stepsToTake.Dequeue());

    for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
            if (map[x, y] >= 0 && map[x, y] > minStepCount &&
               ((even && map[x, y] % 2 == 0) || (!even && map[x, y] % 2 == 1)))
                count++;

    return count;
}
void InitializeMap()
{
    for (var y = 0; y < lines.Length; y++)
    {
        for (var x = 0; x < lines[y].Length; x++)
        {
            map[x, y] = -1;
            if (lines[y][x].Equals('S'))
            {
                map[x, y] = 0;
                startX = x;
                startY = y;
            }
        }
    }
}

void CountSteps(int index)
{
    var y = (int)index / width;
    var x = index % width;
    var steps = map[x, y];

    if (y > 0 && !lines[y - 1][x].Equals('#') && map[x, y - 1] < 0)
    {
        map[x, y - 1] = steps + 1;
        stepsToTake.Enqueue((y - 1) * width + x);
    }

    if (x < map.GetLength(0) - 1 && !lines[y][x + 1].Equals('#') && map[x + 1, y] < 0)
    {
        map[x + 1, y] = steps + 1;
        stepsToTake.Enqueue(y * width + (x + 1));
    }

    if (y < map.GetLength(1) - 1 && !lines[y + 1][x].Equals('#') && map[x, y + 1] < 0)
    {
        map[x, y + 1] = steps + 1;
        stepsToTake.Enqueue((y + 1) * width + x);
    }

    if (x > 0 && !lines[y][x - 1].Equals('#') && map[x - 1, y] < 0)
    {
        map[x - 1, y] = steps + 1;
        stepsToTake.Enqueue(y * width + (x - 1));
    }
}