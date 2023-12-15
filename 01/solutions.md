# Solutions to Day 1: Trebuchet?!

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Of for an easy start: finding the first and the last character that is a digit within a given input string. I used a nullable `char?` variable to store both, so while looping over the characters I could see if the `firstDigit` was already set, and if not, that was the first digit. I would actually set `lastDigit` each time I found a digit (using `char.IsDigit()`), because that would automatically end up containing the value of the last digit found at the end of the loop. The I only had to combine them into a string, parse it to an int, and add the outcome to my `answer`.

## Part 2

Part two was a little bit more challenging. I noticed the spelled out digits actually did overlap (like `twone` and `oneight`) so cannot just do a string replace to replace every occurrence with its numerical value. My solution was to do the same in code as I would do when looking at a given input string: look for the first digit, and look for the first spelled out digit, and see which comes first. Same for the last digit. So I kept my code from part 1 to get the first and last actual digit, but now also added storing the indexes of those results within the input string. Then I've created a `string[]` containing all spelled out values of the digits and iterated over that array. For each I did an `indexOf()` and `lastIndexOf()`. Then I only had to check which index was lower for the first digit, and which index was higher for the last digit: the actual digit or the spelled out one. If it was the spelled out value, I replaced the original stored digit. The rest of the code is the same.