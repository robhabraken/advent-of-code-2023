string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 0;
var digplan = new List<Input>();

// read input
foreach (var line in lines)
{
    digplan.Add(new Input()
    {
        direction = line.Split(' ')[0],
        steps = int.Parse(line.Split(' ')[1]),
        color = line.Split(' ')[2][1..^1]
    });
}

// discover grid size
int width = 0, height = 0, minWidth = int.MaxValue, maxWidth = 0, minHeight = int.MaxValue, maxHeight = 0;
foreach (var input in digplan)
{
    if (input.direction.Equals("U"))
        height -= input.steps;
    else if (input.direction.Equals("R"))
        width += input.steps;
    else if (input.direction.Equals("D"))
        height += input.steps;
    else if (input.direction.Equals("L"))
        width -= input.steps;

    if (width < minWidth) minWidth = width;
    if (width > maxWidth) maxWidth = width;
    if (height < minHeight) minHeight = height;
    if (height > maxHeight) maxHeight = height;
}

// determine dimensions and starting point (adding an extra tile around the grid for future flood fill)
var startingPoint = new Coordinate(Math.Abs(minWidth) + 1, Math.Abs(minHeight) + 1);
width = Math.Abs(minWidth) + maxWidth + 3;
height = Math.Abs(minHeight) + maxHeight + 3;

// build up grid and trace path
var grid = new char[width, height];
grid[startingPoint.x, startingPoint.y] = '#';
foreach (var input in digplan)
{
    int deltaX = 0, deltaY = 0;
    if (input.direction.Equals("U"))
        deltaY = -1;
    else if (input.direction.Equals("R"))
        deltaX = +1;
    else if (input.direction.Equals("D"))
        deltaY = +1;
    else if (input.direction.Equals("L"))
        deltaX = -1;

    for (var i = 0; i < input.steps; i++)
    {
        startingPoint.x += deltaX;
        startingPoint.y += deltaY;
        grid[startingPoint.x, startingPoint.y] = '#';
    }
}

// flood fill around path
FloodFill(new Coordinate(0, 0));

// count all paths and inner grid tiles
for (var y = 0; y < height; y++)
    for (var x = 0; x < width; x++)
        if (grid[x, y] == '\0' || grid[x, y] == '#')
            answer++;

Console.WriteLine(answer);

void FloodFill(Coordinate start)
{
    var queue = new Queue<Coordinate>();
    Fill(start, queue);

    while (queue.Count > 0)
    {
        var point = queue.Dequeue();

        if (point.y > 0)
            Fill(new Coordinate(point.x, point.y - 1), queue);

        if (point.x < grid.GetLength(0) - 1)
            Fill(new Coordinate(point.x + 1, point.y), queue);

        if (point.y < grid.GetLength(1) - 1)
            Fill(new Coordinate(point.x, point.y + 1), queue);

        if (point.x > 0)
            Fill(new Coordinate(point.x - 1, point.y), queue);
    }
}

void Fill(Coordinate point, Queue<Coordinate> queue)
{
    if (grid[point.x, point.y] == '#' || grid[point.x, point.y] == 'O')
        return;

    grid[point.x, point.y] = 'O';
    queue.Enqueue(point);
}

struct Input
{
    public string direction;
    public int steps;
    public string color;
}

struct Coordinate
{
    public int x;
    public int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
