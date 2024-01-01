string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\24\\input.txt");

double searchRange = 1000;

// read input
var hailstones = new List<Hailstone>();
foreach (var line in lines)
    hailstones.Add(new Hailstone(line));

var rock = new Hailstone();

// find the required velocity of the rock by checking it against equally fast moving hailstones on each axis separately
// this doesn't work for the example input though, as that only has enough input data for this to work on the x-axis
var constantPairsX = new List<HailstonePair>();
var constantPairsY = new List<HailstonePair>();
var constantPairsZ = new List<HailstonePair>();

var xSortedList = hailstones.OrderBy(x => x.velocity.x).ToList<Hailstone>();
for (var i = 1; i < xSortedList.Count; i++)
    if (xSortedList[i].velocity.x == xSortedList[i - 1].velocity.x)
        constantPairsX.Add(new HailstonePair(Math.Abs(xSortedList[i].position.x - xSortedList[i - 1].position.x), xSortedList[i].velocity.x));

var ySortedList = hailstones.OrderBy(x => x.velocity.y).ToList<Hailstone>();
for (var i = 1; i < ySortedList.Count; i++)
    if (ySortedList[i].velocity.y == ySortedList[i - 1].velocity.y)
        constantPairsY.Add(new HailstonePair(Math.Abs(ySortedList[i].position.y - ySortedList[i - 1].position.y), ySortedList[i].velocity.y));

var zSortedList = hailstones.OrderBy(x => x.velocity.z).ToList<Hailstone>();
for (var i = 1; i < zSortedList.Count; i++)
    if (zSortedList[i].velocity.z == zSortedList[i - 1].velocity.z)
        constantPairsZ.Add(new HailstonePair(Math.Abs(zSortedList[i].position.z - zSortedList[i - 1].position.z), zSortedList[i].velocity.z));

for (var rockVelocity = -searchRange; rockVelocity <= searchRange && (rock.velocity.x == 0 || rock.velocity.y == 0 || rock.velocity.z == 0); rockVelocity++)
{
    var possible = true;
    foreach (var pair in constantPairsX)
        if (!pair.IsPossible(rockVelocity))
        {
            possible = false;
            break;
        }

    if (possible) rock.velocity.x = rockVelocity;

    possible = true;
    foreach (var pair in constantPairsY)
        if (!pair.IsPossible(rockVelocity))
        {
            possible = false;
            break;
        }

    if (possible) rock.velocity.y = rockVelocity;

    possible = true;
    foreach (var pair in constantPairsZ)
        if (!pair.IsPossible(rockVelocity))
        {
            possible = false;
            break;
        }

    if (possible) rock.velocity.z = rockVelocity;
}

// when we know the velocity, we can determine the starting position by intersecting its path with two random rocks
hailstones[0].PositionRock(hailstones[1], rock);

Console.WriteLine(rock.position.x + rock.position.y + rock.position.z);

record HailstonePair
{
    public double distance;
    public double velocity;

    public HailstonePair(double distance, double velocity)
    {
        this.distance = distance;
        this.velocity = velocity;
    }

    public bool IsPossible(double rockVelocity)
    {
        return distance % (rockVelocity - velocity) == 0;
    }
}

class Hailstone
{
    public Coord position;
    public Coord velocity;

    public Hailstone()
    {
        position = new Coord(0, 0, 0);
        velocity = new Coord(0, 0, 0);
    }

    public Hailstone(string input)
    {
        position = new Coord(input.Split('@')[0]);
        velocity = new Coord(input.Split('@')[1]);
    }
    
    public void PositionRock(Hailstone other, Hailstone rock)
    {
        double slopeA = (velocity.y - rock.velocity.y) / (velocity.x - rock.velocity.x);
        double slopeB = (other.velocity.y - rock.velocity.y) / (other.velocity.x - rock.velocity.x);

        double yInterceptA = position.y - (slopeA * position.x);
        double yInterceptB = other.position.y - (slopeB * other.position.x);

        rock.position.x = Math.Round((yInterceptB - yInterceptA) / (slopeA - slopeB));
        rock.position.y = Math.Round(slopeA * rock.position.x + yInterceptA);

        double time = (rock.position.x - position.x) / (velocity.x - rock.velocity.x);
        rock.position.z = Math.Round(position.z + (velocity.z - rock.velocity.z) * time);
    }
}

class Coord
{
    public double x;
    public double y;
    public double z;

    public Coord(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Coord(string input)
    {
        x = double.Parse(input.Split(',')[0].Trim());
        y = double.Parse(input.Split(',')[1].Trim());
        z = double.Parse(input.Split(',')[2].Trim());
    }
}