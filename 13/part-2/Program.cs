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
    var oldPatternNote = analyzePattern();
    var patternNote = findSmudge(oldPatternNote) * 100;

    if (patternNote < 0)
    {
        rotate();
        if (oldPatternNote < 0)
            oldPatternNote = analyzePattern();
        else
            oldPatternNote = -1;
        patternNote = findSmudge(oldPatternNote);
    }
    answer += patternNote;

    pattern = new List<string>();
}

int findSmudge(int oldPatternNote)
{
    for (var y = 0; y < pattern.Count; y++)
    {
        for (var x = 0; x < pattern[y].Length; x++)
        {
            var newValue = '.';
            var originalPattern = pattern[y];
            if (pattern[y][x].Equals('.'))
                newValue = '#';

            var chars = pattern[y].ToCharArray();
            chars[x] = newValue;
            pattern[y] = new string(chars);

            var result = analyzePattern(oldPatternNote);
            if (result >= 0)
                return result;

            pattern[y] = originalPattern;
        }
    }
    return -1;
}

int analyzePattern(int oldPatternNote = -1)
{
    var findAnchors = new List<int>();
    for (var i = 0; i < pattern.Count; i++)
        if (i < pattern.Count - 1)
            if (pattern[i].Equals(pattern[i + 1]))
                findAnchors.Add(i);

    if (findAnchors.Count > 0)
        foreach (var anchor in findAnchors)
            if (anchor != oldPatternNote - 1 && validateAnchor(anchor))
                return anchor + 1;

    return -1;
}

bool validateAnchor(int index)
{
    var distanceFromEdge = Math.Max(pattern.Count - index - 2, index);
    for (var delta = 1; delta <= distanceFromEdge; delta++)
        if (index - delta >= 0 && index + delta + 1 < pattern.Count &&
            !pattern[index - delta].Equals(pattern[index + delta + 1]))
        {
            return false;
        }

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