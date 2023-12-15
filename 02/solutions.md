# Solutions to Day 2: Cube Conundrum

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Lots of text parsing for today. More than actual logic. The task is a simple one: since the given fixed amount of red, green and blue cubes (12, 13 and 14), we need to iterate over the given input and check if the cube count from the input is higher than any of those values for the corresponding color. If so, that game would've been impossible, so don't add it to the `answer`.

## Part 2

For the second part, we need to find the highest number of cubes per color per game listed in the input. I created an integer variable for each color, and started looping over the set in each game. Then it's a matter of checking if the amount of cubes is higher than the one already stored, and if so, replace the stored values with the amount found in this set. Lastly, multiple the outcome of all colors beforing adding it to the final answer. Again, there's not much to explain about the solution I chose, most of it is parsing the input.