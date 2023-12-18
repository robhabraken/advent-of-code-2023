string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

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
        map[x, y].connections = new List<CityBlock>();

        if (y > 0)
            map[x, y].connections.Add(map[x, y - 1]);
        if (x < map.GetLength(0) - 1)
            map[x, y].connections.Add(map[x + 1, y]);
        if (y < map.GetLength(1) - 1)
            map[x, y].connections.Add(map[x, y + 1]);
        if (x > 0)
            map[x, y].connections.Add(map[x - 1, y]);
    }
}

var startNode = blocks[0];
startNode.cost = 0;

var endNode = blocks[^1];

var priorityQueue = new List<CityBlock>();
priorityQueue.Add(startNode);
var startToEndCost = int.MaxValue;
do
{
    priorityQueue = priorityQueue.OrderBy(x => x.cost.Value).ToList();
    var block = priorityQueue.First();
    priorityQueue.Remove(block);

    foreach (var connection in block.connections.OrderBy(x => x.heatLoss))
    {
        if (connection.visited)
            continue;

        if (connection.cost == null ||
            block.cost + connection.heatLoss < connection.cost && block.cost + connection.heatLoss < startToEndCost)
        {
            connection.cost = block.cost + connection.heatLoss;
            connection.nearestToStart = block;
            if (!priorityQueue.Contains(connection))
                priorityQueue.Add(connection);
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

for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        if (shortestPath.Contains(map[x, y]))
            Console.Write("X");
        else
            Console.Write(".");
    }
    Console.WriteLine();
}

// Console.WriteLine(answer);

void BuildShortestPath(List<CityBlock> blocks, CityBlock block)
{
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

    public int? cost;
    public bool visited;

    public List<CityBlock> connections;
    public CityBlock nearestToStart;

    public CityBlock(int x, int y, int heatLoss)
    {
        this.x = x;
        this.y = y;
        this.heatLoss = heatLoss;
    }
}