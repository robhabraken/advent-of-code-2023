# Solutions to Day 6: Wait For It

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Another easy puzzle today, especially compared to yesterday's challenge. The parsing of the input takes up more lines of code in my solution than the algorithm itself. I know there's a mathematical approach to this, but I didn't bother because honestly, looping through all possible times the button can be pressed and checking if the distance is greater than the one give is actually less code than the mathematical computation. One line for-loop, one line if-check, I line to count a valid amount. I wouldn't describe this as a true brute-force solution (some do) as I think brute-forcing is trying all possible answers withou using any logic to find the right answer, while this is just looping through the curve and counting the inputs for where the distance is above a certain threshold. And for the given input, which was surprisingly small, it wasn't exactly slow either: 15 ms. 

## Part 2

Where most second parts of AoC puzzles are harder, I think this was even easier than part one. I could drop the for loop that went over the (four) races in part one, because I now only needed to check one answer. And I didn't even need to change my parsing all that much, only removing the whitespace from the input instead of splitting it into an array. Now, apart from the parsing and writing the answer to the console output, the whole of my logic for my simple approach was only three lines of code:
```
for (var buttonPressed = 0; buttonPressed < time; buttonPressed++)
    if (buttonPressed * (time - buttonPressed) > distance)
        answer++;
```
I don't even think this is less elegant than the mathematical approach. And it was a lot faster to come up with as well.

On most days, such a non-mathematical approach for part 2 blows up your computer and execution times by coming up with crazy high numbers or a zillion iterations. Not this time though, the input numbers where quite low and my program came up with an answer in 98 ms.