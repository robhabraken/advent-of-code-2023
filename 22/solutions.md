# Solutions to Day 22: Sand Slabs

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Today's part 1 was an easy one again and a lot of fun to build. May be a bit complex to imagine, but basically it's just building a three-dimensional object array to store all bricks in, sorting them by z-index for ease of further calculations, and then moving up from the bottom to the top to iteratively check if there's bricks under them and if not shift them down. The trick here really is the sorting, so you only have to make one pass. For the next loop over the bricks, you check if there's bricks above it and add them to a temporary list. Then you go over that temporary list and check which bricks are under those. If any of the bricks under the bricks on top that are not the brick you're looking to remove, it's safe to remove because it is supported by other bricks. My explanation here may be a bit hard to follow, but I put comments in my code so go check it out, it's easy to see how it works.

What makes is quite easy to parse is that fortunately, they have given us sorted dimensions: the left position always has equal or lower x-, y-, and z-coordinates. So you could easily loop through those. Additionally, when checking, you only have to loop over two dimensions! Only the x- and y-axis. The z-axis does't require looping, because you only need to know which bricks are below or above a brick on the x- and y-axis. If you want to know if there's any brick below, check the cube under the *first* z-coordinate minus one. And if you want to check the cube above a brick, check the *second* z-coordinate plus two. This works for both vertically and horizontally oriented blocks. Also having compressed the grid first like the puzzle description states makes the puzzle a lot easier to see if there's anything below or above a brick.

So to solve this puzzle, I've created a `Coord` object with a x-, y- and z-coordinate. And a `Brick` object with an array of two coordinates (positions). The constructor builds from the input string and does the parsing. And it implements `IComporable` to make blocks sortable along the z-axis. Then, I've read the input constructing all the bricks, and simultaneously determining the grid size. Then, I sorted the bricks. And created the grid, placing a reference to each brick *in each of the grid cells it was in*! This is an important tactic, apart from the sorting, to keep the code clean and the approach easy. I don't have to check the actual dimensions further down when checking blocks, I just need to check all cells directly below or above me and automatically will find the right brick object. This might make me find duplicate objects, but that's easy to ignore (or check for).

Then I did the loops as described above. The first one is the shifting down:
* Check all x- and y-coordinates below my first z-coordinate, if I do not find any other bricks, this brick is in the air and free to shift down at least one position
* Move over all my brick object reference in the actual grid in all of the cells that this brick spans
* Update the coordinate objects in the brick to reflect the new position
* Repeat for this brick until I can no longer shift one down
* Repeat for all bricks going up - automatically creating free air under bricks further up, as I shift down from the bottom through my sorted list!

Then the check if it's safe to remove:
* For each brick check if there's bricks above my second z-coordinate, and store references to those bricks in a list called `bricksOnTop` (if I didn't already added it, because a brick could appear in more cells above me)
* Iterate to that new list and then do the opposite: check for bricks below my first z-coordinate, if there are no bricks below, or only brick references to the brick I'm looking to remove, it's not safe to remove, otherwise, it is

And that's all, just add each brick that seems to be free to be removed to my `answer`.

I'm convinced there are other ways to calculate the outcome and you might not *need* to actually build the three-dimensional array and do all the shifts and checks like I did, but this was easiest for me to envision and thus build. And it turned out to be quite efficient and very fast.

## Part 2

