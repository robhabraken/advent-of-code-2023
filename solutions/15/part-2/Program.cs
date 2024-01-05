using System.Text;

string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\15\\input.txt");

long answer = 0;
var steps = lines[0].Split(',');
var boxes = new Box[256];

foreach (var step in steps)
{
    var lens = new Lens(step);

    if (boxes[lens.BoxIndex] == null)
        boxes[lens.BoxIndex] = new Box();

    if (lens.Remove)
    {
        foreach (var boxedLens in boxes[lens.BoxIndex].Lenses)
        {
            if (boxedLens.Label.Equals(lens.Label))
            {
                boxes[lens.BoxIndex].Lenses.Remove(boxedLens);
                break;
            }
        }
    }
    else
    {
        var replaced = false;
        for (var i = 0; i < boxes[lens.BoxIndex].Lenses.Count; i++)
        {
            if (boxes[lens.BoxIndex].Lenses[i].Label.Equals(lens.Label))
            {
                boxes[lens.BoxIndex].Lenses[i] = lens;
                replaced = true;
                break;
            }
        }

        if (!replaced)
            boxes[lens.BoxIndex].Lenses.Add(lens);
    }
}

for (var boxIndex = 0; boxIndex < boxes.Length; boxIndex++)
    if (boxes[boxIndex] != null)
        for (var lensIndex = 0; lensIndex < boxes[boxIndex].Lenses.Count; lensIndex++)
            answer += (boxIndex + 1) * (lensIndex + 1) * boxes[boxIndex].Lenses[lensIndex].FocalLength;

Console.WriteLine(answer);

class Lens
{
    public string Label;
    public int BoxIndex;
    public int FocalLength;
    public bool Remove;

    public Lens(string step)
    {
        if (step.Contains('-'))
        {
            Remove = true;
            Label = step.Replace("-", "");
        }
        else
        {
            Label = step.Split('=')[0];
            FocalLength = int.Parse(step.Split('=')[1]);
        }
        BoxIndex = GetHashValue();
    }

    private int GetHashValue()
    {
        var currentValue = 0;
        var asciiValues = Encoding.ASCII.GetBytes(Label);
        foreach (var value in asciiValues)
        {
            currentValue += value;
            currentValue *= 17;
            currentValue %= 256;
        }
        return currentValue;
    }
}

class Box
{
    public List<Lens> Lenses;

    public Box()
    {
        Lenses = new List<Lens>();
    }
}