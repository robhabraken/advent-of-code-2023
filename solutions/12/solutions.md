# Solutions to Day 12: Hot Springs

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This certainly was the hardest puzzle of the first two weeks for me. The first solution I could come up with is to iterate over all possible arrangements and test them for validity. Before reading the input, I created a two-dimensional `List<List<string>>` that contained all possible arrangement for each amount of unknown springs. I populated this using a little trick: iterating over the power of the unknown spring count and converting that to a binary representation automatically produces a range of all possible 0 and 1 combinations, that I replaced with respectively `.` and `#` characters.

Then, for each line read from the input, I counted the number of unknown springs, and subsequently iterated over the corresponding possible combinations in the arrangements list to count all valid arrangements. The `IsValid()` function tests if a given arrangement satisfies the given number and size of groups by deduplicating the `.`s, removing the groups, trimming and splitting on the now singular `.` and then counting the string length of each damaged spring group.

This is quite a compact solution, but very inefficient and thus slow. For part 1, this works, though not ideal. For part 2, this would takes ages, so I really need to come up with a real solution.

## Part 2

I started completely from scratch a few times, but didn't get it to work. I tried first determining every group of damaged springs that could only be in just one position, iterating over what's left. I tried removing the edges to make the input smaller (when there's a group of 6 and the second spring is damaged, we know the third, fourth and fifth also need to be damaged). But all those exercises, though functional, didn't really make enough impact as the input string was five times as long as it initially was. What I came up with then is iterate over the springs and test for each sections if it was valid or how many options there were, but I couldn't figure out how to do that without losing edge cases that crossed sections, or where to split a section.

After a week or so, and several attemps, I decided to browse through the r/adventofcode subreddit and watch a few YouTube videos to learn from solutions of others. The recursive one using memoization felt most close to what I was attempting. So I implemented that to learn from it - it is not my own solution in terms of logic, just my go at implementing a solution that seemed both elegant in sufficient. I have added a lot of comments in my code to explain how it works. If you want to know how, check it out, it's easier to read than explaining it here.
