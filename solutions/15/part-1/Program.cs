using System.Text;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\15\\input.txt");

long answer = 0;
var steps = lines[0].Split(',');

foreach (var  step in steps)
{
    var currentValue = 0;
    var asciiValues = Encoding.ASCII.GetBytes(step);
    foreach (var value in asciiValues)
    {
        currentValue += value;
        currentValue *= 17;
        currentValue %= 256;
    }
    answer += currentValue;
}

Console.WriteLine(answer);