using System.ComponentModel;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.example4");

var answer = 0;
var sketch = new Pipe[lines[0].Length, lines.Length];
Pipe startingPoint;
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[0].Length; x++)
    {
        var pipe = new Pipe(lines[y][x], x, y);
        sketch[x, y] = pipe;
        if (pipe.symbol.Equals('S'))
            startingPoint = pipe;
    }
}

// iteratively remove incorrectly connected pipes until we end up with the main loop only
var incorrectPipesFound = false;
do
{
    incorrectPipesFound = false;

    // find incorrectly connected pipes (but don't remove them yet as to not influence connection count on adjacent pipes)
    for (var y = 0; y < sketch.GetLength(1); y++)
    {
        for (var x = 0; x < sketch.GetLength(0); x++)
        {
            if (sketch[x, y].symbol.Equals('S') || sketch[x, y].symbol.Equals('.'))
                continue;

            var connections = 0;
            if (y > 0 && sketch[x, y].north && sketch[x, y - 1].south)
                connections++;
            if (x > 0 && sketch[x, y].west && sketch[x - 1, y].east)
                connections++;
            if (y < sketch.GetLength(1) - 1 && sketch[x, y].south && sketch[x, y + 1].north)
                connections++;
            if (x < sketch.GetLength(0) - 1 && sketch[x, y].east && sketch[x + 1, y].west)
                connections++;

            if (connections != 2)
            {
                sketch[x, y].ScheduleToRemove();
                incorrectPipesFound = true;
            }
        }
    }
    // remove all unconnected pipes marked as scheduled to remove
    for (var y = 0; y < sketch.GetLength(1); y++)
        for (var x = 0; x < sketch.GetLength(0); x++)
            sketch[x, y].CleanUp();

} while (incorrectPipesFound);

// print result
for (var y = 0; y < sketch.GetLength(1); y++)
{
    for (var x = 0; x < sketch.GetLength(0); x++)
    {
        Console.Write(sketch[x, y].symbol);
    }
    Console.WriteLine();
}

Console.WriteLine(answer);

class Pipe
{
    public char symbol;
    public int x;
    public int y;

    public bool north;
    public bool east;
    public bool south;
    public bool west;

    public bool scheduleToRemove;

    public Pipe(char symbol, int x, int y)
    {
        this.symbol = symbol;
        this.x = x;
        this.y = y;
        SetConnections();
    }

    public void ScheduleToRemove()
    {
        scheduleToRemove = true;
    }

    public void CleanUp()
    {
        if (scheduleToRemove)
        {
            symbol = '.';
            north = false;
            east = false;
            south = false;
            west = false;
        }
    }

    private void SetConnections() {
        switch (symbol)
        {
            case 'S':
                north = true;
                east = true;
                south = true;
                west = true;
                break;
            case 'F': 
                north = false;
                east = true;
                south = true;
                west = false;
                break;
            case '-':
                north = false;
                east = true;
                south = false;
                west = true;
                break;
            case '7':
                north = false;
                east = false;
                south = true;
                west = true;
                break;
            case '|':
                north = true;
                east = false;
                south = true;
                west = false;
                break;
            case 'J':
                north = true;
                east = false;
                south = false;
                west = true;
                break;
            case 'L':
                north = true;
                east = true;
                south = false;
                west = false;
                break;
        }
    }
}
