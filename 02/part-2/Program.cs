string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\02\\input.txt");

var answer = 0;
foreach (var line in lines)
{
    var game = line.Replace("Game ", "", StringComparison.InvariantCultureIgnoreCase);
    var colonIndex = game.IndexOf(':');
    var gameNumber = int.Parse(game[..colonIndex]);
    var gameInfo = game[(colonIndex + 1)..].Trim();

    int red = 0, green = 0, blue = 0;
    foreach (var set in gameInfo.Split(';'))
    {
        foreach (var colors in set.Split(","))
        {
            var cubeCount = int.Parse(colors.Replace("red", "", StringComparison.InvariantCultureIgnoreCase)
                                              .Replace("green", "", StringComparison.InvariantCultureIgnoreCase)
                                              .Replace("blue", "", StringComparison.InvariantCultureIgnoreCase)
                                              .Trim());

            if (colors.Trim().EndsWith("red") && cubeCount > red)
            {
                red = cubeCount;
            } else if (colors.Trim().EndsWith("green") && cubeCount > green)
            {
                green = cubeCount;
            } else if (colors.Trim().EndsWith("blue") && cubeCount > blue)
            {
                blue = cubeCount;
            }
        }
    }

    answer += (red * green * blue);
}

Console.WriteLine(answer);