string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

long answer = 0;

// read input
var hailstones = new List<Hailstone>();
foreach (var line in lines)
    hailstones.Add(new Hailstone(line));

// iterate over all possible combinations and count future intersections
for (var a = 0; a < hailstones.Count; a++)
    for (var b = a; b < hailstones.Count; b++)
        if (a != b && hailstones[a].IntersectsWith(hailstones[b], 200000000000000, 400000000000000))
                answer++;

Console.WriteLine(answer);

class Hailstone
{
    public Coord position;
    public Coord velocity;
    public Coord newPosition;

    public Hailstone(string input)
    {
        position = new Coord(input.Split('@')[0]);
        velocity = new Coord(input.Split('@')[1]);
        newPosition = position.Add(velocity);
    }

    public bool IntersectsWith(Hailstone other, double minValue, double maxValue)
    {
        double slopeA = (newPosition.y - position.y) / (newPosition.x - position.x);
        double slopeB = (other.newPosition.y - other.position.y) / (other.newPosition.x - other.position.x);

        double yInterceptA = position.y - (slopeA * position.x);
        double yInterceptB = other.position.y - (slopeB * other.position.x);

        double x = (yInterceptB - yInterceptA) / (slopeA - slopeB);
        double y = slopeA * x + yInterceptA;

        return IsInFuture(x, y) && other.IsInFuture(x, y) && x >= minValue && x <= maxValue && y >= minValue && y <= maxValue;
    }

    public bool IsInFuture(double x, double y)
    {
        return ((velocity.x > 0 && x > position.x) || (velocity.x < 0 && x < position.x)) &&
               ((velocity.y > 0 && y > position.y) || (velocity.y < 0 && y < position.y));
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

    public Coord Add(Coord other)
    {
        return new Coord(x + other.x, y + other.y, z + other.z);
    }
}