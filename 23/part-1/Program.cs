string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

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

// setup connections between tiles, excluding forest tiles as a possible destination
// and only adding one connection for slopes to always go downhill
for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        map[x, y].connections = new List<Tile>();

        if (map[x, y].type == TileType.Path)
        {
            if (y > 0 && map[x, y - 1].type != TileType.Forest)
                map[x, y].connections.Add(map[x, y - 1]);
            if (x < map.GetLength(0) - 1 && map[x + 1, y].type != TileType.Forest)
                map[x, y].connections.Add(map[x + 1, y]);
            if (y < map.GetLength(1) - 1 && map[x, y + 1].type != TileType.Forest)
                map[x, y].connections.Add( map[x, y + 1]);
            if (x > 0 && map[x - 1, y].type != TileType.Forest)
                map[x, y].connections.Add(map[x - 1, y]);
        }
        else if (map[x, y].type != TileType.Forest)
        {
            if (map[x, y].type == TileType.SlopeUp)
                map[x, y].connections.Add(map[x, y - 1]);
            else if (map[x, y].type == TileType.SlopeRight)
                map[x, y].connections.Add(map[x + 1, y]);
            else if (map[x, y].type == TileType.SlopeDown)
                map[x, y].connections.Add(map[x, y + 1]);
            else if (map[x, y].type == TileType.SlopeLeft)
                map[x, y].connections.Add(map[x - 1, y]);
        }
    }
}

var startNode = tiles[1];
var endNode = tiles[^2];

var possibleHikes = new List<List<Tile>>();
var path = new List<Tile>();
possibleHikes.Add(path);
TracePath(startNode, path);

var longestHike = new List<Tile>();
foreach (var hike in possibleHikes)
    if (hike.Count > longestHike.Count)
        longestHike = hike;

longestHike.Remove(startNode);
answer = longestHike.Count;

Console.WriteLine(answer);

void TracePath(Tile tile, List<Tile> path)
{
    path.Add(tile);
    if (tile != endNode && tile.type != TileType.Forest)
    {
        for (var i = 0; i < tile.connections.Count; i++)
        {
            var newPath = new List<Tile>(path);
            possibleHikes.Add(newPath);
            if (!newPath.Contains(tile.connections[i]))
                TracePath(tile.connections[i], newPath);
        }
    }
}

public class Tile
{
    public int x;
    public int y;
    public char symbol;
    public TileType type;

    public List<Tile> connections;

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