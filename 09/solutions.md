# Solutions to Day 9: Mirage Maintenance

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Today's puzzle seems to be rather easy, as it basically exactly explained what to do to complete the sequences. I briefly thought about recursion, but that would probably make it more complex than needed. A simply loop would do. I created a List\<List\<int>> collection to store each sequence in: a list of lists of integers. The first list would contain the given sequence. The added lists are for the derivatives. So after parsing and storing the initial sequence, I created an 'endless' loop using `while(true)` to create new lists that went from the sequence index to minus one of the initial sequence length, and added a number for each difference between the current and next integer. After creating each derivative, I checked if all values were zeros to break out of my while loop if so.

Then, I needed one more loop to walk over this list of lists backwards. For the first list encountered (which is the last one in the list) I add a zero, and for all other lists I add the sum of the last value of the current list and the last value of the next list to the next list. Mind that I obviously skip the last item in the list (checking for `i >= 0` instead of `i > 0`) because I always edit the next list based on the current list's values.

Lastly, I simply add the last value of the first List in the collection to my `answer`.

## Part 2

I kind of expected the second puzzle to have an irregular pattern or a major increase in input size, so I was surprised to learn it only swapped sides, not adding a new item to the end of the list, but to the front. Though the algorithm is actually exactly the same. The only thing you need to change is subtract the values instead of adding them.

Programmatically, it would be way more work to add an item to the beginning of a list while maintaining the order - you would need to copy it over to a new object list after adding a new first value as you cannot add objects to a collection before the first index and also the index count would change making it more complex. But I actually didn't bother with that, it doesn't have to be 'visually' correct. So I kept all of my code: although I now calculated the new value using the first two values, and put the new value to the end of the next list nonetheless. This was way easier to achieve and didn't require me to change anything to my code of part 1.

So:
```
sequences[i].Add(sequences[i][^1] + sequences[i + 1][^1]);
```
became:
```
sequences[i].Add(sequences[i][0] - sequences[i + 1][^1]);
```
In other words: don't use the last (`^1`) item of the current sequence but the first item of the current sequence (`0`) and substract the *last* value of the next sequence from that (instead of adding it).

So effectively, I only changed three characters within my original program to get the answer for the second puzzle of the day.