string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.example");

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
grid = new Brick[++gridSize.x, ++gridSize.y, ++gridSize.z];
foreach (var brick in bricks)
    for (var x = brick.coords[0].x; x <= brick.coords[1].x; x++)
        for (var y = brick.coords[0].y; y <= brick.coords[1].y; y++)
            for (var z = brick.coords[0].z; z <= brick.coords[1].z; z++)
                grid[x, y, z] = brick;

// shift down bricks
foreach (var brick in bricks)
{
    // shift by one along z-axis until not possible anymore
    bool shiftedDown;
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

                Console.WriteLine($"Shifted {brick.coords[0].x},{brick.coords[0].y},{brick.coords[0].z}-{brick.coords[1].x},{brick.coords[1].y},{brick.coords[1].z}");
            }
        }
    } while (shiftedDown);
}

Console.WriteLine(answer);

class Brick : IComparable
{
    public Coord[] coords;

    public Brick(string input)
    {
        coords = new Coord[2];
        for (var i  = 0; i < coords.Length; i++)
            coords[i] = new Coord(input.Split('~')[i]);
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