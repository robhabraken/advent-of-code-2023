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
    var oldPatternNote = analyzePattern(0);
    var patternNote = analyzePattern(1, oldPatternNote) * 100;

    if (patternNote < 0)
    {
        rotate();
        if (oldPatternNote < 0)
            oldPatternNote = analyzePattern(0);
        else
            oldPatternNote = -1;
        patternNote = analyzePattern(1, oldPatternNote);
    }
    answer += patternNote;

    pattern = new List<string>();
}

int analyzePattern(int allowedDifferences, int skipReflectionLine = -1)
{
    for (var reflectionLine = 0; reflectionLine < pattern.Count - 1; reflectionLine++)
        if (reflectionLine != skipReflectionLine - 1 && validate(reflectionLine, allowedDifferences))
            return reflectionLine + 1;

    return -1;
}

bool validate(int index, int allowedDifferences)
{
    var differences = 0;
    var distanceFromEdge = Math.Max(pattern.Count - index - 2, index);
    for (var delta = 0; delta <= distanceFromEdge; delta++)
        if (index - delta >= 0 && index + delta + 1 < pattern.Count)
            for (var i = 0; i < pattern[index - delta].Length; i++)
                if (!pattern[index - delta][i].Equals(pattern[index + delta + 1][i]))
                    if (++differences > allowedDifferences)
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