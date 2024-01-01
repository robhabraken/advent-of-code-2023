# Solutions to Day 25: Snowverload

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Where to start? Looking at the input file there doesn't appear to be any signs of grouping so I used an online visualization tool to come up with a pattern. It obviously shows exactly what the puzzle description states: there's two major groups of components connected to each other, with only three single connections crossing from one to the other group. But what defines a component being part of a 'group'? The number of connections doesn't tell much, and traversing paths within a group to find all connecting components doesn't work because those three connections interconnect everything. After staring at the dots and lines for a while I came up with an idea: components that are closely grouped, have a lot of connections between them. So you can travel over those connections back to where you started quite fast. But if you travel over a connection that sends you to the other group, you need to take a lot more steps to get back to the original group if you want to create a loop.

First, I created a `Component` and a `Connection` class to store both object types and did the parsing, which wasn't a lot of work today. I made sure to add each connection in both directions, and added new components only found in the connection list on the right side of a colon. The next part took a bit of fiddling to get it right, but what it does is evaluating each connection as viewed from each component: looping over all available components, and than evaulating each connecting of each component, counting how many steps it takes to go back to the current component over each connection. So for the example input, we start with `jqt` and find a route that goes back to `jqt` over `rhn`, over `xhk` and over `nvd`. But also `ntq` as that is also a direction created further down in the example input. This means we have four connections. So what if, if you travel from `jqt` to `rhn`, without being allowed to travel back over that same route, how many steps would it take to find your way back to `jqt`. Because those nodes are part of a triangle, and both are connected via `xhk`, the answer is 3. So we 'rate' the `jqt - rhn` connecting with the number `3`. If there are multiple routes, we'll take the fastest route found, so the lowest possible rating for that connection is the one that counts.

We do this by first looping over all components, then over all its connections, and then over the connections of those connections to start (earlier isn't possible). We skip the connection that leads us back to where we came from, and call `FindShortestRouteToSelf()` that gives back a step count. As mentioned, the lowest rating counts and is stored as a new `Connection` object with a `from` and `to` attribute together with a rating.

For implementing the `FindShortestRouteToSelf()` function I didn't use recursion. This might seem like a viable option but it isn't. For two reasons: I need to check the steps (connections) per count while recursion would travel each node to its, and secondly there is no natural stopping point within such a network graph since everything is connected. In other words, recursion would go on forever and quickly throw a stack overflow exception, and it would be very efficient because it would try to find a way over each possible connection, exploring that to infinity until it would find itself again. Instead, what I need, is check all third level connections, then all fourth level, all fifth, all sixth, etc. So a double while loop and a `Queue` is way more efficient here. I first check all connections of my starting node - if I already found my target (the original component) I stop and return the current search depth. If not, I add this connection (the node itself actually) to the queue. Then, I explore all components in the queue, and enqueue all _their_ connections. But I don't trace their paths until I've checked all of the connections that are in the queue first. As I need to check all connections of the same depth first. Now the next challenge is that if I just add items to the queue until I found myself, it would work, but I wouldn't be able to tell the search depth, as all current components would add their connections to the end of the list and the code would seamlessly move over to the next level of depth without me knowing. So my solution to this is to use two queues: dequeue the first one and enqueue to the new one. If the first one is empty, increase the level of depth, and swap them. This process can be iterated over until we found our target. This approach isn't the fastest, but it works flawlessly and it still only takes up about 30 seconds to map out the entire network graph.

The built up list of connections together with their rating is then ordered descending on rating and we take the first 6 elements. Because we want to find the 3 connections that have the highest rating, or in other words, over which it takes the longest to get back to where I started. And because all connections are bidirectional, my algorithm finds 6 of them. Corresponding to the above example it will find both `jqt - nvd` and `nvd - jqt`. Then I iterate over the list of components and their connections once more to remove those 6 connections.

The result will be that there are now to separate groups of components. But we're not there yet, we still need to determine which components make up a group. We only need to know the size of one group, because the other logically is made up out of the rest of the components. Therefore, I just pick the first component from the list and start traversing over all its connections. Now, because we removed ('disconnected') those 3 connections, it will actually only find all grouped compomnents. For this, I did use recursion and it only takes a few lines of code to do so. I stored those in a separate `List<Component>` and my answer is the count of that `groupOne` found times the difference between the total amount of components and that same number.

## Part 2

_Not yet started_