string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\03\\input.txt");

var answer = 0;
for (var line = 0; line < lines.Length; line++)
{
    var startIndex = -1;
    for (var character = 0; character < lines[line].Length; character++)
    {
        if (char.IsDigit(lines[line][character]))
        {
            if (startIndex == -1)
                startIndex = character;

            if (character == lines[line].Length - 1 || !char.IsDigit(lines[line][character + 1]))
            {
                var validNumber = false;
                for (var row = line - 1; row <= line + 1; row++)
                    for (var column = startIndex - 1; column <= character + 1; column++)
                        if (row >= 0 && row < lines.Length &&
                            column >= 0 && column < lines[row].Length)
                            if (lines[row][column] != '.' && !char.IsDigit(lines[row][column]))
                                validNumber = true;

                if (validNumber)
                    answer += int.Parse(lines[line].Substring(startIndex, character - startIndex + 1));

                startIndex = -1;
            }
        }
    }
}

Console.WriteLine(answer);