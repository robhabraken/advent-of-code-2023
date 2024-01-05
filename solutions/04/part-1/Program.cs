string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\04\\input.txt");

var answer = 0;
foreach (var line in lines)
{
    var card = line.Split(':')[1];
    var winningNumbers = card.Split('|')[0].Trim().Split(' ');
    var numbersYouHave = card.Split('|')[1].Trim().Split(' ');

    var score = 0;
    foreach (var number in numbersYouHave)
        if (!string.IsNullOrEmpty(number.Trim()))
            if (winningNumbers.Contains(number.Trim()))
                if (score == 0)
                    score++;
                else
                    score *= 2;

    answer += score;
}

Console.WriteLine(answer);