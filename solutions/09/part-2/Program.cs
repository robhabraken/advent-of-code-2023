string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\09\\input.txt");

var answer = 0;
foreach (var line in lines)
{
    var sequences = new List<List<int>>();
    sequences.Add(new List<int>());
    foreach (var value in line.Split(' '))
        sequences[0].Add(int.Parse(value));

    var sequenceCount = 0;
    while (true)
    {
        var differences = new List<int>();
        for (int i = 0; i < sequences[sequenceCount].Count - 1; i++)
            differences.Add(sequences[sequenceCount][i + 1] - sequences[sequenceCount][i]);

        sequences.Add(differences);

        var allZero = true;
        foreach (var difference in differences)
            if (difference != 0)
                allZero = false;

        if (allZero)
            break;
        else
            sequenceCount++;
    }

    for (var i = sequences.Count - 1; i >= 0; i--)
    {
        if (i == sequences.Count - 1)
            sequences[i].Add(0);
        else
            sequences[i].Add(sequences[i][0] - sequences[i + 1][^1]);
    }

    answer += sequences[0][^1];
}

Console.WriteLine(answer);