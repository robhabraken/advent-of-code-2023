string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\07\\input.txt");

var answer = 0;
var hands = new List<Hand>();

foreach (var line in lines)
    hands.Add(new Hand(line.Split(' ')[0], line.Split(' ')[1]));

hands.Sort();

for (var i = 0; i < hands.Count; i++)
    answer += hands[i].Bid * (i + 1);

Console.WriteLine(answer);

class Hand : IComparable
{
    public string Cards;
    public int Bid;
    public HandType Type;

    public Hand(string cards, string bid)
    {
        Cards = cards;
        Bid = int.Parse(bid.Trim());
        DiscoverType();
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Hand otherHand)
            throw new NullReferenceException();

        if (this.Type == otherHand.Type)
        {
            var thisSortValue = GetSortValue(this.Cards);
            var otherHandSortValue = GetSortValue(otherHand.Cards);
            return string.Compare(otherHandSortValue, thisSortValue);
        }
        else if (otherHand.Type > this.Type)
            return -1;
        else
            return 1;
    }

    private void DiscoverType()
    {
        var occurrenceMap = string.Empty;
        for (var i = 0; i < Card.VALUES.Length; i++)
            occurrenceMap += CountOccurrences(Cards, Card.VALUES[i]).ToString();

        var jokers = int.Parse(occurrenceMap[0].ToString());

        // if there are jokers, find the card with the highest occurence and add the jokers to that card
        // if there are multiple equal occurences, this algorithm takes the first, as it should only use the jokers once,
        // and for the sorting order it doens't matter to which card value the jokers are added (the type will be the same anyway)
        if (jokers > 0)
        {
            var highestOccurrence = -1;
            var highestOccurrenceIndex = -1;
            for (var i = 1; i < Card.VALUES.Length; i++)
            {
                var occurrence = int.Parse(occurrenceMap[i].ToString());
                if (occurrence > highestOccurrence)
                {
                    highestOccurrence = occurrence;
                    highestOccurrenceIndex = i;
                }
            }

            var correctedOccurrenceMap = string.Empty;
            for (var i = 0; i < Card.VALUES.Length; i++)
            {
                if (i == 0)
                    correctedOccurrenceMap += "0";
                else if (i == highestOccurrenceIndex)
                    correctedOccurrenceMap += $"{highestOccurrence + jokers}";
                else
                    correctedOccurrenceMap += occurrenceMap[i];
            }

            occurrenceMap = correctedOccurrenceMap;
        }

        if (occurrenceMap.Contains('5'))
            Type = HandType.FiveOfAKind;
        else if (occurrenceMap.Contains('4'))
            Type = HandType.FourOfAKind;
        else if (occurrenceMap.Contains('3') && occurrenceMap.Contains('2'))
            Type = HandType.FullHouse;
        else if (occurrenceMap.Contains('3'))
            Type = HandType.ThreeOfAKind;
        else if (CountOccurrences(occurrenceMap, '2') == 2)
            Type = HandType.TwoPair;
        else if (occurrenceMap.Contains('2'))
            Type = HandType.OnePair;
        else
            Type = HandType.HighCard;
    }

    private static int CountOccurrences(string haystack, char needle)
    {
        var count = 0;
        foreach (char c in haystack)
            if (c == needle)
                count++;
        return count;
    }

    private static string GetSortValue(string cards)
    {
        var sortValue = string.Empty;
        for (var i = 0; i < cards.Length; i++)
            sortValue += Card.SORT_VALUES[Card.VALUES.IndexOf(cards[i])];
        return sortValue;
    }
}

class Card
{
    public static readonly string VALUES = "J23456789TQKA";
    public static readonly string SORT_VALUES = "MLKJIHGFEDCBA";
}

enum HandType
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}