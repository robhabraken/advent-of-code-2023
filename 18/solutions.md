# Solutions to Day 18: Lavaduct Lagoon

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Great puzzle again today, quite a challenge and learned a lot. It looked like day 10 a bit: tracing a path around a grid and determining the amount of tiles within. And the colored walls of the trench made me think I might need my algorithm again from that day where I marked all the inside walls of the path. Little did I know..

I've created an `Input` struct this time to more easily store and iterate through the given input, and a `Coordinate` class to keep track of my position. It occured to me that, while the example input started at `0,0`, the actual puzzle input didn't. So there's no clue on the grid size. Hence, the first part of my implementation goes over all instructions first, counting the amount of steps in each direction, to eventually determine the actual grid size. The starting point of the path would  be the absolute value of both the minimum width and minimum heigth as x- and y-coordinates, and the total width and height the absolute value of the minimum width plus the maximum width plus 1 (becasuse I did not yet count the tile I started on). With this, I effectively translated the whole path into one with only positive grid coordinates (because if you would assume the starting position to be `0,0` the actual puzzle input went into negative coordinates and initially I figured that would be difficult to work with). Then I added 1 to the starting position on each axis, and 2 on each grid dimension, effectively creating an empty border around the grid for my yet to come flood fill algorithm.

Then, I've started building up the grid, using a two-dimensional `char` array, I marked all coordinates along the path, which I traversed following the instructions, with a `#` symbol. This gave me an actual map of the grid, that corresponded with the example drawing in the puzzle description.

Then, I've implemented a very simple flood fill algorithm using both recursion and a queue to avoid stack overflow issues. The idea behind a flood fill is very simple: if I'm on a wall (`#`) or on a tile I already visited (`O` in my case), abort. If not, I'm on an empty tile, so I will mark this tile with my fill symbol (`O`). Then, for each tile around me, also call the same method and do the same. I checked the boundaries before calling this method to minimize calls to the `Fill()` function (you can also check for boundaries within that function). This logic automatically floods over your grid as long as you are sure to pick an empty tile to start on. And the extra border around my path ensures I can also go around the edge of the path, if they might touch the boundaries of the grid. But instead of calling yourself (recurion) you create a helper function that creates a queue, puts the first point to check on that queue, and startes looping over the queue as long as there are still items to process. For each actual fill function, you don't call the method, but just put the point to check on the queue, so it automatically gets picked up.

The last part of my implementation was to iterate over the grid and count all empty cells and all of the walls. I think the code for part 1 came out very clean, the grid building code works like a charm, the flood fill algorithm is nice and tidy, and everything performs well. So I was all set for part two, expecting to do something with the colors...

## Part 2

Well, I didn't expect _that_. Of course, I tried running my code from part one with the altered input parsing (which was only a very small change, and I dropped the color attribute because I didn't need that anymore), and got a `Array dimensions exceeded supported range` exception on my two-dimensional array. And for good reason: the grid turned out to be some 25 million by 27 million tiles.. I know knew for sure I didn't need a grid, I had to calculate this. And because I have a path, I can create a list of coordinates, which forms a polygon, of which I can calculate the area.

So I started cleaning up my code, dropping the grid, the flood fill and the grid size calculation among other things. I also realized that I no longer needed to translate my whole path into positive coordinates. It's doesn't matter anymore to have negative coordinates when I am not using an actual array to store the grid in.

Now, I created a `List<Coordinate>()` object to store the points of the polygon and went over the instructions again, now adding the actual amount of steps at a tmie instead of needing to step per tile. So this turned out to be even less code and way more efficient (unsurprisingly, of course).

After that I needed to come up with the area of a polgyon. At first I tried looping over the edges and calculating triangles from point 1 to point 2 et cetera, but that didn't work. The shape was very irregular, and all of my sides where straight. Then, I found the shoelace formula, which was perfect for this application. All in all, it's only two lines of code to come up with the actual area of this complex polygon: loop over the points (minus one) and calculate `((x1*y2) - (x2*y1))/2`, which is a triangle from the center of the polygon (`0,0`) to one side of the current straight perimeter edge, to the other ende of that same edge. If you do that for each straight edge (from step to step), and add that to each other, you get the area of the polygon. Of course, sometimes it travels back, overlapping earlier triangles, but that also produces a negative area, and the sum of all of those in the end is, magically, the exact internal area of the polygon.
```
  x1, y1        x2, y2
      ------------      
      \          /
       \        /
        \      /
         \    /
          \  /
           \/
          0, 0
```
However, my answer did not yet meet the example answer on the example input. So I figured that's because you we also need to calculate the area of the perimeter, which is actual a whole meter wide. As the path only contains straight lines, I added all of the steps when following the instructions, and that turned out to be exactly twice the difference between my answer and that of the puzzle example. Off by one. So I added this `perimeter / 2 + 1` to the area calculated and that gave me the correct answer. I later learned that the off by one is because going round (it is a close path) you always have four right corners extra, each accounting for an area of `0.25`. This formula is actaully refered to as 'Pick's theorem', but I didn't know that yet - this simple, derived version of it, is just what I came up with as the area per triangle.

I'm quite proud of my solution today, it really is very little code for such a relatively complex problem to solve.