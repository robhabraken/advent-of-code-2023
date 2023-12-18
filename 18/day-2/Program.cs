using System.Reflection.Metadata;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

long answer = 0;

// read input
var digplan = new List<Input>();
foreach (var line in lines)
{
    digplan.Add(new Input()
    {
        direction = int.Parse(line.Split(' ')[2][^2..^1]),
        steps = Convert.ToInt32(line.Split(' ')[2][2..^2], 16)
    });
}

// create starting point
var startingPoint = new Coordinate(0, 0);

// create a polygon of the path
long perimeter = 0;
var points = new List<Coordinate>();
foreach (var input in digplan)
{
    if (input.direction == 3)
        startingPoint.y -= input.steps;
    else if (input.direction == 0)
        startingPoint.x += input.steps;
    else if (input.direction == 1)
        startingPoint.y += input.steps;
    else if (input.direction == 2)
        startingPoint.x -= input.steps;

    perimeter += input.steps;

    points.Add(new Coordinate(startingPoint.x, startingPoint.y));
}

// calculate area of polygon
long area = 0;
for (var i = 0; i < points.Count - 1; i++)
{
    area += ((points[i].x * points[i + 1].y) - (points[i + 1].x * points[i].y)) / 2;
}

// add area of perimeter to answer
answer = area + (perimeter / 2L) + 1;

Console.WriteLine(answer);

struct Input
{
    public int direction;
    public long steps;
}

struct Coordinate
{
    public long x;
    public long y;

    public Coordinate(long x, long y)
    {
        this.x = x;
        this.y = y;
    }
}
