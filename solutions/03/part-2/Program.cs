string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\03\\input.txt");

var answer = 0;
var numberMatrix = new int[lines.Length, lines[0].Length];
for (var line = 0; line < lines.Length; line++)
{
    var startIndex = -1;
    for (var character = 0; character < lines[line].Length; character++)
    {
        if (char.IsDigit(lines[line][character]))
        {
            if (startIndex == -1)
            {
                startIndex = character;
            }
            if (character == lines[line].Length - 1 || !char.IsDigit(lines[line][character + 1]))
            {
                for (var index = startIndex; index <= character; index++)
                    numberMatrix[line, index] = int.Parse(lines[line].Substring(startIndex, character - startIndex + 1));

                startIndex = -1;
            }
        }
    }
}

for (var line = 0; line < lines.Length; line++)
{
    for (var character = 0; character < lines[line].Length; character++)
    {
        if (lines[line][character].Equals('*'))
        {
            var numbersFound = new List<int>();

            if (GetValue(numberMatrix, line -1, character) > 0)
            {
                numbersFound.Add(GetValue(numberMatrix, line - 1, character));
            }
            else
            {
                if (GetValue(numberMatrix, line - 1, character - 1) > 0)
                    numbersFound.Add(GetValue(numberMatrix, line - 1, character - 1));
                if (GetValue(numberMatrix, line - 1, character + 1) > 0)
                    numbersFound.Add(GetValue(numberMatrix, line - 1, character + 1));
            }

            if (GetValue(numberMatrix, line, character - 1) > 0)
                numbersFound.Add(GetValue(numberMatrix, line, character - 1));
            if (GetValue(numberMatrix, line, character + 1) > 0)
                numbersFound.Add(GetValue(numberMatrix, line, character + 1));

            if (GetValue(numberMatrix, line + 1, character) > 0)
            {
                numbersFound.Add(GetValue(numberMatrix, line + 1, character));
            }
            else
            {
                if (GetValue(numberMatrix, line + 1, character - 1) > 0)
                    numbersFound.Add(GetValue(numberMatrix, line + 1, character - 1));
                if (GetValue(numberMatrix, line + 1, character + 1) > 0)
                    numbersFound.Add(GetValue(numberMatrix, line + 1, character + 1));
            }

            if (numbersFound.Count == 2)
                answer += (numbersFound[0] * numbersFound[1]);
        }
    }
}
int GetValue(int[,] matrix, int x, int y)
{
    int value = 0;
    if (x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1))
        value = matrix[x, y];
    return value;
}

Console.WriteLine(answer);