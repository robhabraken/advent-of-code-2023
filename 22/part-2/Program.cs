string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\22\\input.txt");

var gridSize = new Coord();
var bricks = new List<Brick>();
Brick?[,,] grid;

long answer = 0;

// read input, create brick objects and determine grid size
foreach (string line in lines)
{
    var brick = new Brick(line);
    bricks.Add(brick);

    if (brick.coords[1].x > gridSize.x) gridSize.x = brick.coords[1].x;
    if (brick.coords[1].y > gridSize.y) gridSize.y = brick.coords[1].y;
    if (brick.coords[1].z > gridSize.z) gridSize.z = brick.coords[1].z;
}

// sort the bricks along the z-axis, makes the rest of the code a lot easier!
bricks.Sort();

// create grid and place bricks in grid
grid = new Brick[++gridSize.x, ++gridSize.y, ++gridSize.z + 1];
foreach (var brick in bricks)
    for (var x = brick.coords[0].x; x <= brick.coords[1].x; x++)
        for (var y = brick.coords[0].y; y <= brick.coords[1].y; y++)
            for (var z = brick.coords[0].z; z <= brick.coords[1].z; z++)
                grid[x, y, z] = brick;

// shift down bricks
ShiftDownBricks(grid, bricks);

// count number of bricks that would fall for each brick if removed
foreach (var brick in bricks)
    answer += TryRemoveBrick(grid, bricks, brick);

Console.WriteLine(answer);

int TryRemoveBrick(Brick?[,,] grid, List<Brick> bricks, Brick brickToRemove)
{
    // make a deep copy of the brick list without the brick we would like to remove
    var testBricks = new List<Brick>();
    foreach (var brick in bricks)
        if (brick != brickToRemove)
            testBricks.Add(new Brick(brick));

    // build up a test grid for this new brick list
    var testGrid = new Brick?[grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)];
    foreach (var brick in testBricks)
        for (var x = brick.coords[0].x; x <= brick.coords[1].x; x++)
            for (var y = brick.coords[0].y; y <= brick.coords[1].y; y++)
                for (var z = brick.coords[0].z; z <= brick.coords[1].z; z++)
                    testGrid[x, y, z] = brick;

    return ShiftDownBricks(testGrid, testBricks);
}

// shifts down all bricks that are up in the air, returning the number of fallen blocks
int ShiftDownBricks(Brick?[,,] grid, List<Brick> bricks)
{
    var counter = 0;
    foreach (var brick in bricks)
    {
        // shift by one along z-axis until not possible anymore
        bool shiftedDown;
        bool brickFell = false;
        do
        {
            shiftedDown = false;
            if (brick.coords[0].z > 1)
            {
                // is the space beneath me empty?
                bool inTheAir = true;
                for (var x = brick.coords[0].x; x <= brick.coords[1].x; x++)
                    for (var y = brick.coords[0].y; y <= brick.coords[1].y; y++)
                        if (grid[x, y, brick.coords[0].z - 1] != null)
                        {
                            inTheAir = false;
                            break;
                        }
                // if so, shift down along the z-axis
                if (inTheAir)
                {
                    // first update the grid
                    for (var x = brick.coords[0].x; x <= brick.coords[1].x; x++)
                        for (var y = brick.coords[0].y; y <= brick.coords[1].y; y++)
                            for (var z = brick.coords[0].z; z <= brick.coords[1].z; z++)
                            {
                                grid[x, y, z] = null;
                                grid[x, y, z - 1] = brick;
                            }
                    // then update the brick's coordinates
                    for (var i = 0; i < 2; i++)
                        brick.coords[i].z--;
                    shiftedDown = true;
                    brickFell = true;
                }
            }
        } while (shiftedDown);

        // if this brick fell down, regardless of how many steps, count this brick
        if (brickFell)
            counter++;
    }

    return counter;
}

class Brick : IComparable
{
    public Coord[] coords;

    public Brick(string input)
    {
        coords = new Coord[2];
        for (var i = 0; i < coords.Length; i++)
            coords[i] = new Coord(input.Split('~')[i]);
    }

    public Brick(Brick copyFrom)
    {
        coords = new Coord[2];
        for (var i = 0; i < coords.Length; i++)
            coords[i] = new Coord(copyFrom.coords[i].x, copyFrom.coords[i].y, copyFrom.coords[i].z);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Brick otherBrick)
            throw new NullReferenceException();

        if (this.coords[0].z < otherBrick.coords[0].z)
            return -1;
        else if (this.coords[0].z == otherBrick.coords[0].z)
            return 0;
        else
            return 1;
    }
}

class Coord
{
    public int x;
    public int y;
    public int z;

    public Coord()
    {
        this.x = 0;
        this.y = 0;
        this.z = 0;
    }

    public Coord(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Coord(string input)
    {
        this.x = int.Parse(input.Split(',')[0]);
        this.y = int.Parse(input.Split(',')[1]);
        this.z = int.Parse(input.Split(',')[2]);
    }
}