string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\17\\input.example");

long answer = 0;
var blocks = new List<CityBlock>();
var map = new CityBlock[lines[0].Length, lines.Length];
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[0].Length; x++)
    {
        var heatLoss = int.Parse(lines[y][x].ToString());
        var cityBlock = new CityBlock(x, y, heatLoss);
        blocks.Add(cityBlock);
        map[x, y] = cityBlock;
    }
}

for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        map[x, y].connections = new List<Connection>();

        if (y > 0)
            map[x, y].connections.Add(new Connection(map[x, y - 1], Direction.Up));

        if (x < map.GetLength(0) - 1)
            map[x, y].connections.Add(new Connection(map[x + 1, y], Direction.Right));

        if (y < map.GetLength(1) - 1)
            map[x, y].connections.Add(new Connection(map[x, y + 1], Direction.Down));

        if (x > 0)
            map[x, y].connections.Add(new Connection(map[x - 1, y], Direction.Left));
    }
}

var startNode = blocks[0];
startNode.cost = 0;
startNode.heatLoss = 0;

var endNode = blocks[^1];

foreach (var block in blocks)
    block.SetStraightLineDistanceTo(endNode);

var priorityQueue = new List<CityBlock>();
priorityQueue.Add(startNode);

// keep track of the directions found per block
var directionLog = new Dictionary<CityBlock, Direction>();
var consecutiveCounts = new Dictionary<CityBlock, int>();

do
{
    priorityQueue = priorityQueue.OrderBy(x => x.cost.Value).ThenBy(x => x.heatLoss).ThenBy(x => x.straightLineDistanceToEnd).ToList();
    var block = priorityQueue.First();
    priorityQueue.Remove(block);

    if (block.x == 11 && block.y == 7 && block.heatLoss == 5)
        ;

    foreach (var connection in block.connections.OrderBy(x => x.block.heatLoss))
    {
        if (connection.block.visited)
            continue;

        if (connection.block.cost == null || block.cost + connection.block.heatLoss < connection.block.cost)
        {
            // skip if we're going in the same direction for three consecutive steps
            if (consecutiveCounts.ContainsKey(block) && consecutiveCounts[block] > 3)
                continue;

            connection.block.cost = block.cost + connection.block.heatLoss;
            connection.block.nearestToStart = block;

            if (!priorityQueue.Contains(connection.block))
                priorityQueue.Add(connection.block);

            // dijkstra modification: add direction for this connection to the current block we're evaluating
            if (directionLog.ContainsKey(connection.block))
                directionLog[connection.block] = connection.direction;
            else
                directionLog.Add(connection.block, connection.direction);

            // and count the consecutive steps in the same direction for each block
            if (directionLog.ContainsKey(block) && directionLog[block] != connection.direction)
                consecutiveCounts[connection.block] = 1;
            else if (!consecutiveCounts.ContainsKey(block))
                consecutiveCounts.Add(connection.block, 1);
            else
                consecutiveCounts.Add(connection.block, consecutiveCounts[block] + 1);
        }
    }    
    block.visited = true;

    if (block == endNode)
        break;

} while (priorityQueue.Any());

var shortestPath = new List<CityBlock>();
shortestPath.Add(endNode);
BuildShortestPath(shortestPath, endNode);
shortestPath.Reverse();

// temp display route over original map
var target = new List<Tuple<int, int>>();
target.Add(Tuple.Create(0, 0));
target.Add(Tuple.Create(1, 0));
target.Add(Tuple.Create(2, 0));
target.Add(Tuple.Create(2, 1));
target.Add(Tuple.Create(3, 1));
target.Add(Tuple.Create(4, 1));
target.Add(Tuple.Create(5, 1));
target.Add(Tuple.Create(5, 0));
target.Add(Tuple.Create(6, 0));
target.Add(Tuple.Create(7, 0));
target.Add(Tuple.Create(8, 0));
target.Add(Tuple.Create(8, 1));
target.Add(Tuple.Create(8, 2));
target.Add(Tuple.Create(9, 2));
target.Add(Tuple.Create(10, 2));
target.Add(Tuple.Create(10, 3));
target.Add(Tuple.Create(10, 4));
target.Add(Tuple.Create(11, 4));
target.Add(Tuple.Create(11, 5));
target.Add(Tuple.Create(11, 6));
target.Add(Tuple.Create(11, 7));
target.Add(Tuple.Create(12, 7));
target.Add(Tuple.Create(12, 8));
target.Add(Tuple.Create(12, 9));
target.Add(Tuple.Create(12, 10));
target.Add(Tuple.Create(11, 10));
target.Add(Tuple.Create(11, 11));
target.Add(Tuple.Create(11, 12));
target.Add(Tuple.Create(12, 12));

for (var y = 0; y < map.GetLength(1); y++)
{
    for (var x = 0; x < map.GetLength(0); x++)
    {
        if (target.Contains(Tuple.Create(x, y)))
            Console.BackgroundColor = ConsoleColor.Yellow;
        else
            Console.BackgroundColor = ConsoleColor.Black;

        if (shortestPath.Contains(map[x, y]))
            Console.ForegroundColor = ConsoleColor.Red;
        else
            Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(map[x, y].heatLoss);
    }
    Console.WriteLine();
}
Console.BackgroundColor = ConsoleColor.Black;
Console.WriteLine();

// templ display cost matrix
for (var y = 0; y < map.GetLength(1); y++)
{
    for (var x = 0; x < map.GetLength(0); x++)
    {
        if (shortestPath.Contains(map[x, y]))
            Console.ForegroundColor = ConsoleColor.Green;
        else
            Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(map[x, y].cost + "\t");
    }
    Console.WriteLine();
}
Console.WriteLine();

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine(answer);

void BuildShortestPath(List<CityBlock> blocks, CityBlock block)
{         
    answer += block.heatLoss;

    if (block.nearestToStart == null)
        return;

    blocks.Add(block.nearestToStart);
    BuildShortestPath(blocks, block.nearestToStart);
}

public class CityBlock
{
    public int x;
    public int y;
    public int heatLoss;
    public double straightLineDistanceToEnd;

    public int? cost;
    public bool visited;

    public List<Connection> connections;
    public CityBlock nearestToStart;

    public CityBlock(int x, int y, int heatLoss)
    {
        this.x = x;
        this.y = y;
        this.heatLoss = heatLoss;
    }

    public void SetStraightLineDistanceTo(CityBlock end)
    {
        straightLineDistanceToEnd = Math.Sqrt(Math.Pow(x - end.x, 2) + Math.Pow(y - end.y, 2));
    }
}

public class Connection
{
    public CityBlock block;
    public Direction direction;

    public Connection(CityBlock block, Direction direction)
    {
        this.block = block;
        this.direction = direction;
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}