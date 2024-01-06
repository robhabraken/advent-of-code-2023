string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\17\\input.txt");

int answer = int.MaxValue;
var map = new CityBlock[lines[0].Length, lines.Length];
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[0].Length; x++)
    {
        var heatLoss = int.Parse(lines[y][x].ToString());
        map[x, y] = new CityBlock(x, y, heatLoss);
    }
}

var endNode = map[map.GetLength(0) - 1, map.GetLength(1) - 1];
var junctionQueue = new Queue<Tuple<int, int, Direction, int>>();

ProcessJunction(0, 0, Direction.Right, 0);
ProcessJunction(0, 0, Direction.Down, 0);

while (junctionQueue.Any())
{
    var junction = junctionQueue.Dequeue();
    ProcessJunction(junction.Item1, junction.Item2, junction.Item3, junction.Item4);
}

Console.WriteLine(answer);

void ProcessJunction(int x, int y, Direction direction, int cost)
{
    CityBlock currentBlock = map[x, y];

    if (currentBlock == endNode)
    {
        if (cost < answer)
            answer = cost;
        return;
    }

    // we've been here before, but via a better (lower cost) route
    if (currentBlock.visited[(int)direction] && currentBlock.cost[(int)direction] < cost)
        return;

    currentBlock.visited[(int)direction] = true;
    currentBlock.cost[(int)direction] = cost;

    Direction left;
    Direction right;
    var deltaX = 0;
    var deltaY = 0;

    if (direction == Direction.Up)
    {
        left = Direction.Left;
        right = Direction.Right;
        deltaX = 1;
    }
    else if (direction == Direction.Right)
    {
        left = Direction.Up;
        right = Direction.Down;
        deltaY = 1;
    }
    else if (direction == Direction.Down)
    {
        left = Direction.Right;
        right = Direction.Left;
        deltaX = -1;
    }
    else
    {
        left = Direction.Down;
        right = Direction.Up;
        deltaY = -1;
    }

    var leftCost = cost;
    var rightCost = cost;
    for (var steps = 1; steps <= 3; steps++)
    {
        if (x - (deltaX * steps) >= 0 && x - (deltaX * steps) < map.GetLength(0) &&
            y - (deltaY * steps) >= 0 && y - (deltaY * steps) < map.GetLength(1))
        {
            leftCost += map[x - (deltaX * steps), y - (deltaY * steps)].heatLoss;
            junctionQueue.Enqueue(Tuple.Create(x - (deltaX * steps), y - (deltaY * steps), left, leftCost));
        }

        if (x + (deltaX * steps) >= 0 && x + (deltaX * steps) < map.GetLength(0) &&
            y + (deltaY * steps) >= 0 && y + (deltaY * steps) < map.GetLength(1))
        {
            rightCost += map[x + (deltaX * steps), y + (deltaY * steps)].heatLoss;
            junctionQueue.Enqueue(Tuple.Create(x + (deltaX * steps), y + (deltaY * steps), right, rightCost));
        }
    }
}

public class CityBlock
{
    public int x;
    public int y;
    public int heatLoss;

    public bool[] visited;
    public int[] cost;

    public CityBlock(int x, int y, int heatLoss)
    {
        this.x = x;
        this.y = y;
        this.heatLoss = heatLoss;
        visited = new bool[4];
        cost = new int[4];
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}