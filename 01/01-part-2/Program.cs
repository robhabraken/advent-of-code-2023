string[] lines = File.ReadAllLines("..\\..\\..\\input.txt");

string[] spelledDigits = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

var answer = 0;
foreach (var line in lines)
{
    var firstDigitIndex = int.MaxValue;
    var lastDigitIndex = -1;

    char? firstDigit = null, lastDigit = null;
    for (var i = 0; i < line.Length; i++)
    {
        if (Char.IsDigit(line, i))
        {
            if (!firstDigit.HasValue)
            {
                firstDigitIndex = i;
                firstDigit = line[i];
            }
            lastDigitIndex = i;
            lastDigit = line[i];
        }
    }

    for (var i = 0; i < spelledDigits.Length; i++)
    {
        var first = line.IndexOf(spelledDigits[i]);
        var last = line.LastIndexOf(spelledDigits[i]);

        if (first > -1 && first < firstDigitIndex)
        {
            firstDigitIndex = first;
            firstDigit = (i + 1).ToString()[0];
        }

        if (last > lastDigitIndex)
        {
            lastDigitIndex = last;
            lastDigit = (i + 1).ToString()[0];
        }
    }
    
    var calibrationValue = Int32.Parse($"{firstDigit}{lastDigit}");
    answer += calibrationValue;
}

Console.WriteLine(answer);