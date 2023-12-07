string[] lines = File.ReadAllLines("..\\..\\..\\input.example");

var answer = 0;
var hands = new List<Hand>();

foreach (var line in lines)
{
    hands.Add(new Hand(line.Split(' ')[0], line.Split(' ')[1]));
}

hands.Sort();

Console.WriteLine(answer);

class Hand : IComparable
{
    public static readonly string CARDS = "23456789TJQKA";

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
            for (var i = 0; i < this.Cards.Length; i++)
            {
                Console.WriteLine(this.Cards[i] + " to compare with " + otherHand.Cards[i]);
            }
            return 0;
        }
        else if (this.Type < otherHand.Type)
            return 1;
        else
            return -1;
    }

    private void DiscoverType()
    {
        var occurenceMap = string.Empty;
        for (var i = 0; i < CARDS.Length; i++)
            occurenceMap += CountOccurences(Cards, CARDS[i]).ToString();

        if (occurenceMap.Contains('5'))
            Type = HandType.FiveOfAKind;
        else if (occurenceMap.Contains('4'))
            Type = HandType.FourOfAKind;
        else if (occurenceMap.Contains('3') && occurenceMap.Contains('2'))
            Type = HandType.FullHouse;
        else if (occurenceMap.Contains('3'))
            Type = HandType.ThreeOfAKind;
        else if (CountOccurences(occurenceMap, '2') == 2)
            Type = HandType.TwoPair;
        else if (occurenceMap.Contains('2'))
            Type = HandType.OnePair;
        else
            Type = HandType.HighCard;
    }

    private static int CountOccurences(string haystack, char needle)
    {
        var count = 0;
        foreach (char c in haystack)
            if (c == needle)
                count++;
        return count;
    }
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