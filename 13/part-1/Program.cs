string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

long answer = 0;
var pattern = new List<string>();
for (var i = 0; i < lines.Length; i++)
{
    if (!string.IsNullOrEmpty(lines[i].Trim()))
        pattern.Add(lines[i]);

    if (string.IsNullOrEmpty(lines[i].Trim()) || i == lines.Length - 1)
        processPattern();
}

Console.WriteLine(answer);

void processPattern()
{
    var patternNote = analyzePattern() * 100;
    if (patternNote < 0)
    {
        rotate();
        patternNote = analyzePattern();
    }
    answer += patternNote;

    pattern = new List<string>();
}

int analyzePattern()
{
    var findAnchors = new List<int>();
    for (var i = 0; i < pattern.Count; i++)
        if (i < pattern.Count - 1)
            if (pattern[i].Equals(pattern[i + 1]))
                findAnchors.Add(i);

    if (findAnchors.Count > 0)
        foreach (var anchor in findAnchors)
            if (validateAnchor(anchor))
                return anchor + 1;

    return -1;
}

bool validateAnchor(int index)
{
    var distanceFromEdge = Math.Max(pattern.Count - index - 2, index);
    for (var delta = 1; delta <= distanceFromEdge; delta++)
        if (index - delta >= 0 && index + delta + 1 < pattern.Count &&
            !pattern[index - delta].Equals(pattern[index + delta + 1]))
            return false;

    return true;
}

void rotate()
{
    var rotatedPattern = new List<string>();
    for (var x = 0; x < pattern[0].Length; x++)
    {
        var row = string.Empty;
        for (var y = pattern.Count - 1; y >= 0; y--)
            row += pattern[y][x];

        rotatedPattern.Add(row);
    }
    pattern = rotatedPattern;
}