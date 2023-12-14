string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

long answer = 0;

// reading the map
var rocks = new char[lines[0].Length, lines.Length];
for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[y].Length; x++)
        rocks[x, y] = lines[y][x];

// rolling the rocks
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

// counting the weights
for (var y = 0; y < rocks.GetLength(1); y++)
    for (var x = 0; x < rocks.GetLength(0); x++)
        if (rocks[x, y].Equals('O'))
            answer += rocks.GetLength(1) - y;

Console.WriteLine(answer);