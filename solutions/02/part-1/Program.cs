﻿string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\02\\input.txt");

var answer = 0;
foreach (var line in lines)
{
    var game = line.Replace("Game ", "", StringComparison.InvariantCultureIgnoreCase);
    var colonIndex = game.IndexOf(':');
    var gameNumber = int.Parse(game[..colonIndex]);
    var gameInfo = game[(colonIndex + 1)..].Trim();

    var impossible = false;
    foreach (var set in gameInfo.Split(';'))
    {
        foreach (var colors in set.Split(","))
        {
            var cubeCount = int.Parse(colors.Replace("red", "", StringComparison.InvariantCultureIgnoreCase)
                                              .Replace("green", "", StringComparison.InvariantCultureIgnoreCase)
                                              .Replace("blue", "", StringComparison.InvariantCultureIgnoreCase)
                                              .Trim());

            if ((colors.Trim().EndsWith("red") && cubeCount > 12) ||
                (colors.Trim().EndsWith("green") && cubeCount > 13) ||
                (colors.Trim().EndsWith("blue") && cubeCount > 14))
            {
                impossible = true;
            }
        }
    }

    if (!impossible)
        answer += gameNumber;
}

Console.WriteLine(answer);