string[] rows = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\16\\input.txt");
string[] cols = new string[rows[0].Length];

var answer = 0;
var stable = 0;

for (int col = 0; col < cols.Length; col++)
{
    var column = new char[rows.Length];
    for (int row = 0; row < rows.Length; row++)
    {
        column[row] = rows[row][col];
    }
    cols[col] = new string(column);
}

var energizedTiles = new bool[rows[0].Length, cols[0].Length];
energizedTiles[0, 0] = true;

var beams = new List<Beam>();
beams.Add(new Beam(0, 0, Direction.Right));

while (true)
{
    var newBeams = new List<Beam>();
    foreach (var beam in beams)
    {
        var newBeam = FollowBeam(beam);
        if (newBeam != null)
            newBeams.Add(newBeam);
    }

    for (var i = 0; i < beams.Count; i++)
        if (beams[i].x < 0 || beams[i].x >= cols.Length || beams[i].y < 0 || beams[i].y >= rows.Length)
            beams.Remove(beams[i]);

    foreach (var beam in newBeams)
        beams.Add(beam);

    var newAnswer = 0;
    for (var y = 0; y < rows.Length; y++)
        for (var x = 0; x < cols.Length; x++)
            if (energizedTiles[x, y])
                newAnswer++;

    if (newAnswer == answer)
        stable++;
    else
        answer = newAnswer;

    if (stable > 2)
        break;
}

Console.WriteLine(answer);

Beam FollowBeam(Beam beam)
{
    if (beam.x < 0 || beam.x >= cols.Length || beam.y < 0 || beam.y >= rows.Length)
        return null;

    if (beam.direction == Direction.Right)
    {
        for (var x = beam.x; x < cols.Length; x++)
        {
            energizedTiles[x, beam.y] = true;
            if (!rows[beam.y][x].Equals('.') && !rows[beam.y][x].Equals('-'))
            {
                if (rows[beam.y][x].Equals('\\'))
                {
                    beam.direction = Direction.Down;
                    beam.x = x;
                    beam.y++;
                    break;
                }
                else if (rows[beam.y][x].Equals('/'))
                {
                    beam.direction = Direction.Up;
                    beam.x = x;
                    beam.y--;
                    break;
                }
                else if (rows[beam.y][x].Equals('|'))
                {
                    beam.direction = Direction.Up;
                    beam.x = x;
                    beam.y--;
                    return new Beam(x, beam.y + 1, Direction.Down);
                }
            }
        }
    }
    else if (beam.direction == Direction.Left)
    {
        for (var x = beam.x; x >= 0; x--)
        {
            energizedTiles[x, beam.y] = true;
            if (!rows[beam.y][x].Equals('.') && !rows[beam.y][x].Equals('-'))
            {
                if (rows[beam.y][x].Equals('\\'))
                {
                    beam.direction = Direction.Up;
                    beam.x = x;
                    beam.y--;
                    break;
                }
                else if (rows[beam.y][x].Equals('/'))
                {
                    beam.direction = Direction.Down;
                    beam.x = x;
                    beam.y++;
                    break;
                }
                else if (rows[beam.y][x].Equals('|'))
                {
                    beam.direction = Direction.Up;
                    beam.x = x;
                    beam.y--;
                    return new Beam(x, beam.y + 1, Direction.Down);
                }
            }
        }
    }
    else if (beam.direction == Direction.Up)
    {
        for (var y = beam.y; y >= 0; y--)
        {
            energizedTiles[beam.x, y] = true;
            if (!cols[beam.x][y].Equals('.') && !cols[beam.x][y].Equals('|'))
            {
                if (cols[beam.x][y].Equals('\\'))
                {
                    beam.direction = Direction.Left;
                    beam.y = y;
                    beam.x--;
                    break;
                }
                else if (cols[beam.x][y].Equals('/'))
                {
                    beam.direction = Direction.Right;
                    beam.y = y;
                    beam.x++;
                    break;
                }
                else if (cols[beam.x][y].Equals('-'))
                {
                    beam.direction = Direction.Left;
                    beam.y = y;
                    beam.x--;
                    return new Beam(beam.x + 1, y, Direction.Right);
                }
            }
        }
    }
    else if (beam.direction == Direction.Down)
    {
        for (var y = beam.y; y < rows.Length; y++)
        {
            energizedTiles[beam.x, y] = true;
            if (!cols[beam.x][y].Equals('.') && !cols[beam.x][y].Equals('|'))
            {
                if (cols[beam.x][y].Equals('\\'))
                {
                    beam.direction = Direction.Right;
                    beam.y = y;
                    beam.x++;
                    break;
                }
                else if (cols[beam.x][y].Equals('/'))
                {
                    beam.direction = Direction.Left;
                    beam.y = y;
                    beam.x--;
                    break;
                }
                else if (cols[beam.x][y].Equals('-'))
                {
                    beam.direction = Direction.Left;
                    beam.y = y;
                    beam.x--;
                    return new Beam(beam.x + 1, y, Direction.Right);
                }
            }
        }
    }

    return null;
}

class Beam
{
    public int x;
    public int y;
    public Direction direction;

    public Beam(int x, int y, Direction direction)
    {
        this.x = x;
        this.y = y;
        this.direction = direction;
    }

}

enum Direction
{
    Up,
    Right,
    Down,
    Left
}