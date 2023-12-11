string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 0;
var galaxies = new List<Galaxy>();

// read image and expand rows while doing so
var expandY = 0;
for (var y = 0; y < lines.Length; y++)
{
    var rowEmpty = true;
    for (var x = 0; x < lines[0].Length; x++)
    {
        if (lines[y][x].Equals('#'))
        {
            galaxies.Add(new Galaxy(x, y + expandY));
            rowEmpty = false;
        }
    }
    if (rowEmpty)
        expandY++;
}

// find columns to expand
var emptyColumns = new List<int>();
for (var x = 0; x < lines[0].Length; x++)
{
    var columnEmpty = true;
    foreach (var galaxy in  galaxies)
        if (galaxy.x == x)
            columnEmpty = false;

    if (columnEmpty)
        emptyColumns.Add(x);
}

// expand columns
for (var i = emptyColumns.Count - 1; i >= 0; i--)
    foreach (var galaxy in galaxies)
        if (galaxy.x > emptyColumns[i])
            galaxy.x++;

// calculate the sum of the shortest paths
for (var i = 0; i < galaxies.Count; i++)
    for (var j = i; j < galaxies.Count; j++)
        answer += galaxies[i].DistanceTo(galaxies[j]);

Console.WriteLine(answer);

class Galaxy
{
    public int x;
    public int y;

    public Galaxy(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int DistanceTo(Galaxy other)
    {
        return Math.Abs(this.x - other.x) + Math.Abs(this.y - other.y);
    }
}