using System.Diagnostics;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

long answer = 0;

var s = new Stopwatch();
s.Start();

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

        if (map[x, y].isPath)
        {
            if (y > 0 && map[x, y - 1].isPath)
                map[x, y].connections.Add(map[x, y - 1]);
            if (x < map.GetLength(0) - 1 && map[x + 1, y].isPath)
                map[x, y].connections.Add(map[x + 1, y]);
            if (y < map.GetLength(1) - 1 && map[x, y + 1].isPath)
                map[x, y].connections.Add(map[x, y + 1]);
            if (x > 0 && map[x - 1, y].isPath)
                map[x, y].connections.Add(map[x - 1, y]);
        }
    }
}

var possibleHikes = new List<List<Tile>>();

var startTile = tiles[1];
var startPath = new List<Tile>();
possibleHikes.Add(startPath);
TracePath(startTile, startPath);

var longestHike = new List<Tile>();
foreach (var hike in possibleHikes)
    if (hike.Count > longestHike.Count && hike.Contains(tiles[^2]))
        longestHike = hike;

for (var y = 0; y < map.GetLength(1); y++)
{
    for (var x = 0; x < map.GetLength(0); x++)
    {
        if (longestHike.Contains(map[x, y]))
            Console.Write('O');
        else if (map[x, y].isPath)
            Console.Write('.');
        else
            Console.Write('#');
    }
    Console.WriteLine();
}
Console.WriteLine();

longestHike.Remove(startTile);
answer = longestHike.Count;

Console.WriteLine(answer);
File.AppendAllText("..\\..\\..\\..\\output.txt", $"{s.ElapsedMilliseconds / 1000}: answer is {answer}");

void TracePath(Tile tile, List<Tile> path)
{
    path.Add(tile);
    if (s.ElapsedMilliseconds % 60000 < 10)
    { File.AppendAllText("..\\..\\..\\..\\output.txt", $"{s.ElapsedMilliseconds / 1000}: hikes found up until now: {possibleHikes.Count}\n"); }

    for (var i = 0; i < tile.connections.Count; i++)
    {
        var newPath = new List<Tile>(path);
        possibleHikes.Add(newPath);
        if (!newPath.Contains(tile.connections[i]))
            TracePath(tile.connections[i], newPath);
    }
}

public class Tile
{
    public int x;
    public int y;
    public bool isPath;

    public List<Tile> connections;

    public Tile(int x, int y, char symbol)
    {
        this.x = x;
        this.y = y;
        this.isPath = !symbol.Equals('#');
    }
}