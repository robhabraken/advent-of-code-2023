using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\12\\input.txt");

var regex = new Regex(Regex.Escape("?"));

var arrangements = new List<List<string>>();
for (var damageCount = 0; damageCount <= 18; damageCount++)
{
    arrangements.Add(new List<string>());
    for (var i = 0; i < (int)Math.Pow(2, damageCount) && damageCount > 0; i++)
        arrangements[damageCount].Add(Convert.ToString(i, 2).PadLeft(damageCount, '0').Replace('0', '.').Replace('1', '#'));
}

long answer = 0;
foreach (string line in lines)
{
    var damageCount = CountOccurrences(line, '?');
        
    foreach(var arrangement in arrangements[damageCount])
    {
        var record = line;
        for (var i = 0; i < arrangement.Length; i++)
            record = regex.Replace(record, arrangement[i].ToString(), 1);

        if (IsValid(record))
            answer++;
    }
}

Console.WriteLine(answer);

int CountOccurrences(string haystack, char needle)
{
    var count = 0;
    foreach (char c in haystack)
        if (c == needle)
            count++;
    return count;
}

bool IsValid(string record)
{
    var springList = Regex.Replace(record.Split(' ')[0].Trim(), "(\\.){1,}", ",").Trim(',').Split(',');
    var springs = string.Empty;
    foreach (var spring in springList)
        springs += $"{spring.Length},";

    return record.Split(' ')[1].Trim().Equals(springs.Trim(','));
}