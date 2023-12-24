string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 0;

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

// setup connections between tiles, excluding forest tiles as a possible destination
// and only adding one connection for slopes to always go downhill
for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        if (map[x, y].type == TileType.Path)
        {
            if (y > 0 && map[x, y - 1].type != TileType.Forest)
                map[x, y].connections.Add(new Connection(map[x, y - 1], 1));
            if (x < map.GetLength(0) - 1 && map[x + 1, y].type != TileType.Forest)
                map[x, y].connections.Add(new Connection(map[x + 1, y], 1));
            if (y < map.GetLength(1) - 1 && map[x, y + 1].type != TileType.Forest)
                map[x, y].connections.Add(new Connection(map[x, y + 1], 1));
            if (x > 0 && map[x - 1, y].type != TileType.Forest)
                map[x, y].connections.Add(new Connection(map[x - 1, y], 1));
        }
        else if (map[x, y].type != TileType.Forest)
        {
            if (map[x, y].type == TileType.SlopeUp)
                map[x, y].connections.Add(new Connection(map[x, y - 1], 1));
            else if (map[x, y].type == TileType.SlopeRight)
                map[x, y].connections.Add(new Connection(map[x + 1, y], 1));
            else if (map[x, y].type == TileType.SlopeDown)
                map[x, y].connections.Add(new Connection(map[x, y + 1], 1));
            else if (map[x, y].type == TileType.SlopeLeft)
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
    var steps = 0;
    for (var i = 0; i < hike.Count - 1; i++)
        foreach (var connection in hike[i].connections)
            if (connection.tile == hike[i + 1])
                steps += connection.length;

    if (steps > answer && hike.Contains(tiles[^2]))
        answer = steps;
}

Console.WriteLine(answer);

void CollapseCorridors()
{
    bool didCollapse;
    do
    {
        didCollapse = false;
        foreach (var tile in tiles)
        {
            if (tile.type != TileType.Forest && tile.connections.Count == 2)
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
                didCollapse = true;
            }
        }
    } while (didCollapse);
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
    public char symbol;
    public TileType type;

    public List<Connection> connections;

    public Tile(int x, int y, char symbol)
    {
        this.x = x;
        this.y = y;

        this.symbol = symbol;
        if (symbol.Equals('.'))
            type = TileType.Path;
        else if (symbol.Equals('^'))
            type = TileType.SlopeUp;
        else if (symbol.Equals('>'))
            type = TileType.SlopeRight;
        else if (symbol.Equals('v'))
            type = TileType.SlopeDown;
        else if (symbol.Equals('<'))
            type = TileType.SlopeLeft;
        else if (symbol.Equals('#'))
            type = TileType.Forest;

        if (type != TileType.Forest)
            connections = new List<Connection>();
    }
}

public enum TileType
{
    Path,
    SlopeUp,
    SlopeRight,
    SlopeDown,
    SlopeLeft,
    Forest
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