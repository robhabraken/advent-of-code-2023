# Solutions to Day 8: Haunted Wasteland

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

The logic for the first part is not very complicated: you look up the starting point, read which direction you need to go next, retrieve the corresponding destination for the current location, then look that one up and repeat, as long as you did not yet reach `ZZZ`. I've created a class `NetworkNode` to do the parsing of the input and store the two destinations, and then created a loop over the input to create all of the network nodes and add them to my list. I've used a `Dictionary<string, NetworkNode>` object so I can easily retrieve the node by its label. Then, I've created a local variable named `iamHere` to store my current position, and starting looping over the network nodes using a while loop, my condition being that the current node wasn't `ZZZ`. Oh, and I also used an `instructionIndex` counter to keep track of the current position in the instruction list (lefts and rights), resetting that back to 0 once I reached the end of that list. Now all you need to do is count the steps and you're done.

## Part 2

Well, now things got interesting... initially I thought, that's easy, just reuse my code from part, create an array of starting points, and move them all one step for each loop over the network nodes. Which works, and is almost as fast as running only one path through the nodes. The caveat here though is that it takes ages to arrive at a mutual location for those 6 individual paths (in my case, the input file contained 6 starting positions). Though I gave it a shot nonetheless, not yet knowing how long it would take. I did this before my working day started, fired up the script and went to a number of consecutive meetings. When I came back after lunch, the script was still running. Back to the drawing board!

This one then took me quite a while to figure out. I couldn't think of a way to just 'know' when their paths would collide and I kind of assumed that each ghost would bump into a node ending one a `Z` every now and then, and actually multiple times within one loop of following the instructions, so I didn't immediately 'see' a pattern. But then it occured to me, _if_ their would be some reoccurrence in the paths, then and only then you would be able to calculate their collision point. So it almost has to be the case for this puzzle to work. I decided to test my theory and just run one of the paths for a little while and output the current number of steps each time that ghost hit an end node. Surprisingly, the pattern was even simpler than I expected: the ghost arrived at the end node in exactly the same amount of steps _every_ time. So I checked it for the others as well, and each of the paths consistently outputted the same number for that path. Then, I knew what to do!

I created an array of what I called 'breakpoints': the point a ghost hit a node ending on a `Z` on its path. Then I started running over the network nodes using my code from path one, stopping the very moment I hit my first breakpoint. And repeat that for all the starting points in the input file. Now I have a list of all different numbers (one for each ghost / path). So what would be the first time all of the ghosts would meet? That would be the _'least common multiple'_ of those numbers. I know you can calculate that LCM using a mathematical method finding a prime that you can divide all values by, but I figured it was way less code to just iterate over the first breakpoint, checking if the remainder of the division would be equal to 0 for all paths. That actually is almost the same code as you would need to find the prime for the more mathematical approach, but if you do that with the actual numbers, you have your answer straight away.

My code ended up being quite efficient and small in size, and rather quick as well: under 4 seconds. And the outcome was a number well over 20 trillion!!