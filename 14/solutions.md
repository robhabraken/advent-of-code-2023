# Solutions to Day 14: Parabolic Reflector Dish

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This was surprisingly straight forward! I read the input and stored it in a two-dimensional `char` array. Then I started looping over the rows. And for each column, if I wasn't already on a rock, I started looking down until I hit a rock. If that was a rolling rock, I replaced my current symbol with it, and if it was a steady rock I stopped. The third piece of code is another nested loop across the platform, where I go over all positions and if I find a rolling rock, I subtract the current y-coordinate from the vertical array length and added that to my answer.

## Part 2

Running 1.000.000.000 cycles doesn't make much sense and would probably take hours if not more. But I imagined that if you repeat the same process of cycling the platform in each direction with the same variables (same fixed rocks and same round rocks moving across the platform), that the location of all of the round rocks must be going in circles. eventually. Once it settles in, there must be a repetitive pattern of positions on the platform. So let's try to find that!

First, I copied my rolling algorithm into a function `RollNorth()` and I've created the variations for all of the other directions. It may be possible to write a generic algorithm, but since so many small things change (flipping x and y, going from left to right or right to left, increasing or decreasing indexes) that would produce a way more complex piece of code so I'm fine with the somewhat duplicate algorithms for now.

Then I moved my algorithm to count the weights into a method so I can call if multiple times. The contents of the `CountWeights()` function didn't change.

I need an easy way to store the current state of the platform that is also easy to compare. I named this state a 'snapshot' (as it is a moment in time representing a specific state of the platform) and create a single string that contains all rows concatenated to each other.

I also needed some more variables to store the different anchors I needed: a list of snapshots I've found, a list of weights corresponding to a specific number of cycles, the first reoccurrence of a snapshot and the index of that reoccurrence. And also the pattern size: the amount of cycles a repeating pattern consists of.

Now I started looping for as long as I've found the pattern size, because once I know that, I have everything I need to know. If I find a new snapshot, I put it in my list (the index isn't relevant here). If I find a pattern snapshot that occurs again (and thus, the first one I find that is already in my list), I know I already completed one whole loop of the recurring pattern and am starting on my second one. So I store that pattern and keep looping, until I hit that snapshot again! Why? Because the number of cycles it takes to get to a recurring pattern is unknown and impossible to determine. But once I have found a specific snapshot twice, I know I ran two whole loops of a recurring pattern, and I know the size of that. And by that, I also know when the recurring pattern started. This all only takes a few 100 cycles to gather all the information I need.

Then I simply determine that starting point. I subtract that from the target number of cycles and get the remainder of that dividing it by the loop size (using modulo). It doesn't matter how many loops of cycles it takes to get to 1.000.000.000, but I need to know how many cycles are left in the last uncompleted loop of cycles. Then I look up that same number in my weight list: the starting point of the recurring pattern plus the remainder would be the same result in weight as many, many cycles later when I hit 1.000.000.000.