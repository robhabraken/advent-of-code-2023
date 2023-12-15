string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var answer = 0;
foreach (var line in lines)
{
    char? firstDigit = null, lastDigit = null;
    foreach (var character in line)
    {
        if (char.IsDigit(character))
        {
            if (!firstDigit.HasValue)
                firstDigit = character;
            lastDigit = character;
        }
    }
    var calibrationValue = int.Parse($"{firstDigit}{lastDigit}");
    answer += calibrationValue;
}

Console.WriteLine(answer);