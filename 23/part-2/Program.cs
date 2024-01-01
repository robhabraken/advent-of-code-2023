string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\23\\input.txt");

long answer = 0;

// read input
var tiles = new List<Tile>();
var map = new Tile[lines[0].Length, lines.Length];
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[y].Length; x++)
    {
        var tile = new Tile(x, y, lines[y][x]);
        tiles.Add(tile);
        map[x, y] = tile;
    }
}

// setup connections between tiles that are part of the path
for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        if (map[x, y].isPath)
        {
            if (y > 0 && map[x, y - 1].isPath)
                map[x, y].connections.Add(new Connection(map[x, y - 1], 1));
            if (x < map.GetLength(0) - 1 && map[x + 1, y].isPath)
                map[x, y].connections.Add(new Connection(map[x + 1, y], 1));
            if (y < map.GetLength(1) - 1 && map[x, y + 1].isPath)
                map[x, y].connections.Add(new Connection(map[x, y + 1], 1));
            if (x > 0 && map[x - 1, y].isPath)
                map[x, y].connections.Add(new Connection(map[x - 1, y], 1));
        }
    }
}

// there's a lot of long corridors, let's collapse their connections to bring down the number of nodes by a lot
CollapseCorridors();

var possibleHikes = new List<List<Tile>>();

var startTile = tiles[1];
var startPath = new List<Tile>();
possibleHikes.Add(startPath);
TracePath(startTile, startPath);

foreach (var hike in possibleHikes)
{
    if (hike.Contains(tiles[^2]))
    {
        var steps = 0;
        for (var i = 0; i < hike.Count - 1; i++)
            foreach (var connection in hike[i].connections)
                if (connection.tile == hike[i + 1])
                    steps += connection.length;

        if (steps > answer)
            answer = steps;
    }
}

Console.WriteLine(answer);

void CollapseCorridors()
{
    bool ready;
    do
    {
        ready = true;
        foreach (var tile in tiles)
        {
            if (tile.isPath && tile.connections.Count == 2)
            {
                foreach (var leftConnection in tile.connections[0].tile.connections)
                {
                    if (leftConnection.tile == tile)
                    {
                        leftConnection.tile = tile.connections[1].tile;
                        leftConnection.length += tile.connections[1].length;
                    }
                }

                foreach (var rightConnection in tile.connections[1].tile.connections)
                {
                    if (rightConnection.tile == tile)
                    {
                        rightConnection.tile = tile.connections[0].tile;
                        rightConnection.length += tile.connections[0].length;
                    }
                }

                tile.connections = new List<Connection>();
                ready = false;
            }
        }
    } while (!ready);
}

void TracePath(Tile tile, List<Tile> path)
{
    path.Add(tile);
    for (var i = 0; i < tile.connections.Count; i++)
    {
        var newPath = new List<Tile>(path);
        possibleHikes.Add(newPath);
        if (!newPath.Contains(tile.connections[i].tile))
            TracePath(tile.connections[i].tile, newPath);
    }
}

public class Tile
{
    public int x;
    public int y;
    public bool isPath;
    public bool collapseVisited;

    public List<Connection> connections;

    public Tile(int x, int y, char symbol)
    {
        this.x = x;
        this.y = y;
        isPath = !symbol.Equals('#');

        if (isPath)
            connections = new List<Connection>();
    }
}

public class Connection
{
    public Tile tile;
    public int length;

    public Connection(Tile tile, int length)
    {
        this.tile = tile;
        this.length = length;
    }
}