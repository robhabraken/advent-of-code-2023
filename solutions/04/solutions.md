# Solutions to Day 4: Scratchcards

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Another easy one. You get two lists and need to check if the numbers from one list (`numbersYouHave`) are present in the other list (`winningNumbers`). That's basically how a lotery works, but code-wise it's merely a call `Contains()` on a list, at least in C#. And because you can easily transform a string into an array using the `Split()` function, the actual solution is no more than a single loop and some nested if-statements. For each number you have, if the string isn't empty (the delimiters are varying between one and two spaces for a nice visual alignment, but that produces empty strings using the `Split()` function, hence the extra check here), and if the winning numbers list contains your number, then do the scoring. That's another if-statement on itself, as for the first score, you just add 1, and for the following numbers won you double the existing score value.

## Part 2

Now things are getting a bit more interesting. Especially because the winning numbers not only determine how many of the next cards are added to your set of cards, but also influence the number of extra cards won by numbers from those extra copies. So it's adding up constantly. I needed to read the puzzle description a few times and think about an elegant solution. Surprisingly though, I hardly needed any more code than for the first part. The initial nested loop is still the same, only now we just count the number of matches per card. To keep track of the number of copies for each card, I've created a cards array (which is just an integer array, as I only need to keep track of the amount of cards per index, where the index of the array corresponds to the index of the line in the input file).

The last thing to do is iterate over that array for the number of correct numbers on the current card (its `matches`) and increase the number of copies for the consecutive cards. Initially, I just increased them by one. But then I figured out that if I already had let's say three copies of the current card, and I won numbers in this card, I also needed to add three copies to the next card, because my current card has three copies, it also wins three copies of the next card. In the end, the following code is the only actual addition to the first version and the code that does all the multiplications for the copies you've won:

```
cards[line]++;
for (var i = 1; i <= matches; i++)
    cards[line + i] += (cards[line]);
```