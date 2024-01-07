string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\17\\input.txt");
int width = lines[0].Length;

int answer = int.MaxValue;
var map = new CityBlock[width, lines.Length];
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < width; x++)
    {
        var heatLoss = int.Parse(lines[y][x].ToString());
        map[x, y] = new CityBlock(x, y, heatLoss);
    }
}

var endNode = map[map.GetLength(0) - 1, map.GetLength(1) - 1];
var junctionQueue = new Queue<double>();

ProcessJunction(200000); // start at 0,0 to the right with 0 cost
ProcessJunction(300000); // start at 0,0 downwards with 0 cost

while (junctionQueue.Any())
    ProcessJunction(junctionQueue.Dequeue());

Console.WriteLine(answer);

void ProcessJunction(double state)
{
    // extract all variables from our state 'object'
    var cost = (int)Math.Round((state - (int)state) * 10000);
    var position = (int)state;

    var direction = position / 100000;
    position -= (direction * 100000);

    var y = position / width;
    var x = position % width;

    CityBlock currentBlock = map[x, y];

    if (currentBlock == endNode)
    {
        if (cost < answer)
            answer = cost;
        return;
    }

    // we've been here before, but via a better (lower cost) route
    if (currentBlock.visited[direction] && currentBlock.cost[direction] < cost)
        return;

    currentBlock.visited[direction] = true;
    currentBlock.cost[direction] = cost;

    int left, right;
    var deltaX = 0;
    var deltaY = 0;

    if (direction == 0) // up
    {
        left = 3;
        right = 1;
        deltaX = 1;
    }
    else if (direction == 1) // right
    {
        left = 0;
        right = 2;
        deltaY = 1;
    }
    else if (direction == 2) // down
    {
        left = 1;
        right = 3;
        deltaX = -1;
    }
    else // left
    {
        left = 2;
        right = 0;
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
            junctionQueue.Enqueue((y - (deltaY * steps)) * width + x - (deltaX * steps) + left * 100000 + leftCost / 10000d);
        }

        if (x + (deltaX * steps) >= 0 && x + (deltaX * steps) < map.GetLength(0) &&
            y + (deltaY * steps) >= 0 && y + (deltaY * steps) < map.GetLength(1))
        {
            rightCost += map[x + (deltaX * steps), y + (deltaY * steps)].heatLoss;
            junctionQueue.Enqueue((y + (deltaY * steps)) * width + x + (deltaX * steps) + right * 100000 + rightCost / 10000d);
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