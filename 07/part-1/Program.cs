string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

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
    public static readonly string VALUES = "23456789TJQKA";
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