# Solutions to Day 15: Lens Library

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This one was more of an assignment than a puzzle. Basically, you just had to do exactly what it said: loop over the steps, get the ASCII value of it, multiply by 17, get the remainder of dividing by 256, and add that to your answer. Not even 20 lines of code in total and only a few minutes of work.

## Part 2

The second part was a bit more work, but equally simple. I've created a `Lens` and a `Box` class to make it easy to keep track of the different attributes. A lens contains a label, a box index, a focal length and a boolean value to indicate if it is a lens to remove (otherwise, add), and a box only contains a list of lens objects. I've moved my code of part one (only the few lines of actual logic) into `GetHashValue()` method to calculate the box index and used that within my constructor of the lens class. Then, I've created an empty array of 256 box objects, started iterating over the steps in the input file, and did the following per step: if the corresponding box didn't exist, create it. Create a new `Lens` object (automatically parsing the input in its constructor). Then check if the lens needs to be removed or not, and do so accordingly with a small loop over the lenses in the box, or otherwise checking if we need to replace or add the new lens.

And then one more loop over the boxes, looping over the lenses, and adding the outcome of the given formula to my `answer`.