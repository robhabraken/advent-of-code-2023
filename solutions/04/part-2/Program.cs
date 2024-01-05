string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\04\\input.txt");

var answer = 0;
var cards = new int[lines.Length];
for (var line = 0; line < lines.Length; line++)
{
    var card = lines[line].Split(':')[1];
    var winningNumbers = card.Split('|')[0].Trim().Split(' ');
    var numbersYouHave = card.Split('|')[1].Trim().Split(' ');

    var matches = 0;
    foreach (var number in numbersYouHave)
        if (!string.IsNullOrEmpty(number.Trim()))
            if (winningNumbers.Contains(number.Trim()))
                matches++;

    cards[line]++;
    for (var i = 1; i <= matches; i++)
        cards[line + i] += (cards[line]);
}

foreach (var card in cards)
    answer += card;

Console.WriteLine(answer);