string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\21\\input.txt");
var width = lines[0].Length;

var answer = 1;
var remainingSteps = 64;

var startX = 0;
var startY = 0;
var map = new int[lines[0].Length, lines.Length];
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

var stepsToTake = new Queue<int>();
CountSteps(startY * width + startX);

while (stepsToTake.Any())
    CountSteps(stepsToTake.Dequeue());

for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[y].Length; x++)
        if (map[x, y] > 0 && map[x, y] <= remainingSteps && map[x, y] % 2 == 0)
            answer++;

Console.WriteLine(answer);

void CountSteps(int index)
{
    var y = (int)index / width;
    var x = index % width;
    var steps = map[x, y];

    if (steps > remainingSteps) return;

    if (y > 0 && lines[y - 1][x].Equals('.') && map[x, y - 1] < 0)
    {
        map[x, y - 1] = steps + 1;
        stepsToTake.Enqueue((y - 1) * width + x);
    }

    if (x < map.GetLength(0) - 1 && lines[y][x + 1].Equals('.') && map[x + 1, y] < 0)
    {
        map[x + 1, y] = steps + 1;
        stepsToTake.Enqueue(y * width + (x + 1));
    }

    if (y < map.GetLength(1) - 1 && lines[y + 1][x].Equals('.') && map[x, y + 1] < 0)
    {
        map[x, y + 1] = steps + 1;
        stepsToTake.Enqueue((y + 1) * width + x);
    }

    if (x > 0 && lines[y][x - 1].Equals('.') && map[x - 1, y] < 0)
    {
        map[x - 1, y] = steps + 1;
        stepsToTake.Enqueue(y * width + (x - 1));
    }
}