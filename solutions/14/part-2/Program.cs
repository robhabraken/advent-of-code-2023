string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\14\\input.txt");

// reading the map
var rocks = new char[lines[0].Length, lines.Length];
for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[y].Length; x++)
        rocks[x, y] = lines[y][x];

// state storage
var snapshots = new List<string>();
var weights = new List<int>();

var firstRecurringSnapshot = string.Empty;
var indexOfRecurringSnapshot = 0;
var patternSize = 0;

// cycle rolling the rocks until we've foud a recurring pattern
for (var index = 0; patternSize == 0; index++)
{
    // roll the platform in each direction once
    RollNorth();
    RollWest();
    RollSouth();
    RollEast();

    // store the weight after each cycle for later reference
    weights.Add(CountWeights());

    // make a snapshot and check if we already found a recurring pattern, if not, keep looking
    // otherwise check if we found the current pattern for the second time to determine the pattern size
    var snapshot = GetSnapshot();
    if (indexOfRecurringSnapshot == 0)
    {
        // if we've already seen this arrangement of rocks, we've found our pattern, so store the end point
        // else, just add the curring snapshot to the list
        if (snapshots.Contains(snapshot))
        {
            firstRecurringSnapshot = snapshot;
            indexOfRecurringSnapshot = index;
        }
        else
            snapshots.Add(snapshot);
    }
    else
        if (snapshot == firstRecurringSnapshot)
            patternSize = index - indexOfRecurringSnapshot;
}

// we've found a recurring pattern, so calculate the starting point of the pattern
// then, get the amount of cycles left from there to the target number of cycles
// by getting the modulo of dividing that by the pattern size, we know how many cycles from the start of the pattern our target state is
// now we only have to add that to the start of the recurring pattern
var startingPoint = indexOfRecurringSnapshot - patternSize;
var cyclesLeft = 1000000000 - startingPoint;
var indexFromStartPoint = cyclesLeft % patternSize - 1;
var indexToGet = startingPoint + indexFromStartPoint;

// retrieve the weight we're looking for from our stored list
Console.WriteLine(weights[indexToGet]);

string GetSnapshot()
{
    var result = string.Empty;
    for (var y = 0; y < rocks.GetLength(1); y++)
        for (var x = 0; x < rocks.GetLength(0); x++)
            result += rocks[x, y];
    return result;
}

int CountWeights()
{
    var count = 0;
    for (var y = 0; y < rocks.GetLength(1); y++)
        for (var x = 0; x < rocks.GetLength(0); x++)
            if (rocks[x, y].Equals('O'))
                count += rocks.GetLength(1) - y;
    return count;
}

void RollNorth()
{
    for (var y = 0; y < rocks.GetLength(1); y++)
    {
        for (var x = 0; x < rocks.GetLength(0); x++)
        {
            if (rocks[x, y].Equals('.'))
            {
                for (var yd = y + 1; yd < rocks.GetLength(1); yd++)
                {
                    if (rocks[x, yd].Equals('O'))
                    {
                        rocks[x, yd] = '.';
                        rocks[x, y] = 'O';
                        break;
                    }
                    else if (rocks[x, yd].Equals('#'))
                    {
                        break;
                    }
                }
            }
        }
    }
}

void RollSouth()
{
    for (var y = rocks.GetLength(1) - 1; y >= 0; y--)
    {
        for (var x = 0; x < rocks.GetLength(0); x++)
        {
            if (rocks[x, y].Equals('.'))
            {
                for (var yd = y - 1; yd >= 0; yd--)
                {
                    if (rocks[x, yd].Equals('O'))
                    {
                        rocks[x, yd] = '.';
                        rocks[x, y] = 'O';
                        break;
                    }
                    else if (rocks[x, yd].Equals('#'))
                    {
                        break;
                    }
                }
            }
        }
    }
}

void RollWest()
{
    for (var x = 0; x < rocks.GetLength(0); x++)
    {
        for (var y = 0; y < rocks.GetLength(1); y++)
        {
            if (rocks[x, y].Equals('.'))
            {
                for (var xd = x + 1; xd < rocks.GetLength(0); xd++)
                {
                    if (rocks[xd, y].Equals('O'))
                    {
                        rocks[xd, y] = '.';
                        rocks[x, y] = 'O';
                        break;
                    }
                    else if (rocks[xd, y].Equals('#'))
                    {
                        break;
                    }
                }
            }
        }
    }
}

void RollEast()
{
    for (var x = rocks.GetLength(0) - 1; x >= 0; x--)
    {
        for (var y = 0; y < rocks.GetLength(1); y++)
        {
            if (rocks[x, y].Equals('.'))
            {
                for (var xd = x - 1; xd >= 0; xd--)
                {
                    if (rocks[xd, y].Equals('O'))
                    {
                        rocks[xd, y] = '.';
                        rocks[x, y] = 'O';
                        break;
                    }
                    else if (rocks[xd, y].Equals('#'))
                    {
                        break;
                    }
                }
            }
        }
    }
}