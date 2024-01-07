# Solutions to Day 17: Clumsy Crucible

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This puzzle took me the longest of all 2023 AoC puzzles. I wrote a Dijkstra implementation to find the quickest route right away, and started experimention with the restrictions of not going in the same direction for four or more consecutive steps. My first attempt to keep track of direction and step count while building up the cost graph worked fairly well and I _almost_ got the optimal route. And that's what distracted me because I thought I was nearly there, but there was a fallacy in my solution: I counted the number of steps during building the cost graph, but that wasn't necessarily the route to take, thus sometimes when the final path was being constructed, it already had to change direction after two steps, leading to the wrong path. And also my cost graph was built up on the optimal route, locking a certain node into one direction, while it could very well be that the optimal path contains suboptimal subpaths due to the restriction of a maximum of 3 steps. I think the solution might be to construct a more elaborate cost graph that takes into account both direction and maybe even stepcount, but I was fiddling too long with this solution that I ultimately decided to start over from scratch and do a BFS implementation. This worked quite well, and although not very fast, it was fast enough to complete my last puzzle of 2023! Maybe I'll come back to this to do a better attempt later, we'll see.

So first, I build up a grid of `CityBlock` objects to keep track of the state for each direction I'm visiting this block from: `visited` and `cost`. Then I create a queue and start searching the grid using my `ProcessJunction()` function. This function is called process junction, because I only act on a junction, not on a straight. When I am at position X in the grid, I can go 1 to 3 steps to my left, and 1 to 3 steps to my right. Not forward. Because the stepping left and right already due the 1 to 3 steps sequentially, and each of those only have to look at _their_ left and right. So my processing looks like this:

- Get the current city block we're on.
- If I am on the end node and my current cost is lower than what I've found before, make this my answer for now.
- If I have been here _from this direction_ before and my cost was higher, abort this path (this goes for any location on the grid, if I visited this node before at higher cost, the end cost is always going to be higher going forward so I don't need to further traverse this path).
- Then we can set the visited boolean and the current cost for my current direction on this block.
- Determine the new direction for both going left and right, together with the delta in x- and y-coordinate (if was going to the right, going left would be up and right would be down - and the x-coordinate doesn't change, but going to the right would increase the y-coordinate, hence deltaY is set to 1; going right is always added to the position, going left substracted, and this works the same for a negative deltaY, reversing the movement on the grid).
- Declare a new variable for keeping track of my cost going both left and right: as we're going to step 1 to 3 steps in a certain direction, I need to keep adding the cost of each step to my local variable in order to be able to send the cumulative cost to the third step.
- For each step, check if my new position is going to be within the bounds of the grid (better to check here than after dequeuing, saving a lot of unnecessary queue items), add the cost of that block to the new cost for this step and enqueue the step to that new position together with its new direction and cost.

And that's all there is. Once my queue is completely empty, I have traversed all possible paths and found the cheapest path given the restriction of a maximum of 3 steps going forward.

My solution was still quite slow though (15 minutes), so after this I changed the queue type from a `Queue<Tuple<int, int, Direction, int>>()` to a `Queue<double>()`. I used the queue to pass the following properties of my state: `int x, int y, Direction direction, int cost` and although using a `Tuple` for that is way faster than using a class (because of the queue pattern that would constantly create new classes that need to be collected by the garbage collector almost immediately, making for a very inefficient and memory gobbling program), it still is quite a bit more expensive than a simple type. So I stuffed all of those properties into a single double called `state`. I changed the direction enumeration to an integer value being 0, 1, 2 or 3 and multiplied that with 100.000. My coordinate system for a grid of 141 by 141 can be translated into a single integer index by multiplying the y-coordinate by the width of the grid and adding the x-coordinate to that (ranging up to almost 20.000). And my cost is divided by 10.000, effectively storing that value behind the decimal point. Now I can safely add all of those numbers together and enqueue them, and after dequeuing I simply 'extract' those from my double again:

```
// extract all variables from our state 'object'
var cost = (int)Math.Round((state - (int)state) * 10000);
var position = (int)state;

var direction = position / 100000;
position -= (direction * 100000);

var y = position / width;
var x = position % width;
```

This trick made my solutions more than 3 times faster! Even with the penalty of those extra calculations. Now it runs in 4.7 minutes for part 1.

## Part 2

This was easy! I just changed this line:
```
for (var steps = 1; steps <= 3; steps++)
```
into:
```
for (var steps = 1; steps <= 10; steps++)
```
As I am not allowed to do more than 10 steps in a row. But why not make the lower boundary 4? Because I am using this loop to cumulatively count the cost of my steps into that direction. And while 1 to 3 aren't valid _as a step_, I should still add up the cost of going over those blocks. so I added an extra line with that loop before enqueuing the next step:
```
if (steps >= 4)
```
What this does, is that it still goes over all steps from 1 to 10 to calculate the corresponding cost, but it only starts stepping into another direction after 3 steps. It only enqueues going left and right from step 4 to 10.

And that's all, one number changed and two (equal) lines added. And it runs way way faster too, because this increased restriction means that there are way less possibilities in routes to take over the grid. With my new `double state` this ran in 7.2 seconds.
