# Solutions to Day 19: Aplenty

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

Today was a fun one, not very hard, but more frustrating that it needed be, because of a bug that I didn't see for a while in my part 2 solution. My approach worked, my algorithm was correct, but I placed two lines on the wrong side of a curly bracket... oh well, took a little longer but I managed anyway. After a short break I came back to my solution, traced the paths manually in the example input, discovered I was missing some paths and then spotted the mistake. I sure am starting to get the hang of the way of thinking of solving this type of puzzles, and I'm proud again on the clean, proper and fast solution for today!

## Part 1

This was more of a parsing assignment than anything else. I created three helper classes `Workflow`, `Rule` and `Part` and let them do the parsing in their constuctor so I could easily build up the required objects from the input file. I added an `ApplyRule()` function to the `Rule` class to test a part against that rule, returning the next action or `null` when the rule didn't match. Then, following up, I addedd a `Process()` function to the `Workflow` class that could process a specific part, testing it against the workflow. The result is either the name of another workflow, or an end node (`R` or `A`).

Using a `Dictionary<string, Workflow>` object to store the workflows in made it easy for me to retrieve a workflow by name. Also, I stored a reference to the starting workflow named `in` separately to easy reference that as well. Now, the only thing left to do was loop through the parts, and then for each part start at the `startWorkflow`, get the resulted next step, and loop over the other workflows using a while-loop until I hit an end node. Then, if that end node was an `A`, add the rating of this part to my answer. Having the above helper classes and functions made the core of my solution very efficient and simple to read:
```
foreach (var part in parts)
{
    var result = startWorkflow.Process(part);

    while (!result.Equals("R") && !result.Equals("A"))
        result = workflows[result].Process(part);

    if (result.Equals("A"))
        answer += part.rating;
}
```

## Part 2

For the second part I first cleaned up some unnecessary code: no more use for the `Part` class of for the `ApplyRule()` and `Process()` functions. Instead I added a `Boundary`. I could've named this 'Range' as well. My thinking was: let's trace each path from the start to any `A` endpoint using recursion, and keep track of the boundaries of each property (or range if you will). If the first rule says "m" has to be lower than 1801, and another rule in that same path to an accepted state would require "m" to be higher than 838, we now that only "m"-values between 839 and 1800 (both inclusive, instead of the input!) would make it through the end.

So my `Boundary` class contained static readonly values for the minimum and maximum range, a property name for the property which this range applies to (either "x", "m", "a" or "s"), and booleans to keep track of either an rejected or accepted state. Also, my constructor required a `Rule` object and set the values accordingly: an input value of 838 and a `>` operator would set the applicable range from 839 to 4000. I also created a constructor to manually populate those fields, actually only used for unconditional rules (like `qkq` which means no matter what your property values are, just go to workflow `qkq`).

Also, I've added the option to inverse the rule when creating a boundary. Why? Because my path traversing algorithm also needed to follow the other route: _"what if the condition isn't met"_. So when a rule in a workflow says: 'when the value of the "s"-property is greater than 2770 go to the `qs` workflow', it also implicitely says that when the value of "s" is 2770 or lower, go to the next rule of this workflow, which is "m" being lower than 1801 in the example. So my recursion needs to split paths upon each rule that contains a condition, and by allowing the boundary object to create inverse rules, this became a little easier further down the line.

Now comes the recursion: start following the `startWorkflow` with an empty `List<Boundary>` (the 'path'). Iterate over all rules of that workflow. If I hit a rule without a condition, create an unconditional boundary and add that to the path and for rejected or accepted end nodes stop the recursion, otherwise follow on to the next workflow (calling myself for the next workflow passing on the path list). If I hit a rule with a condition, I need to split ways: I create a new path object (deep copy) and either stop the recursion if it's an end node, or follow on to the next workflow (calling myself). Also, on the current path, I add a boundary with an inversed condition and move on within the same method, effectively going to the next rule of this current workflow. This `FindAcceptedPaths()` function is the core of the solution for part 2. It's actually harder to explain in text than to just follow what happens in the code (I added some comments for clarity). This also took me a while to get right - as I already mentioned, I got the nested if-statement wrong initially, skipping some paths and messing up the outcome. I moved the last curly bracket up two lines and then it worked perfectly fine!

The last step is the `Analyze()` method which is called for each end node of the recursive function. This basically just creates MIN and MAX boundary values for each properties and narrows down the allowed range while going over all of the rules for this specific path. But only if the path is not rejected of course. The number of possibilities of property values for this path is the product of the allowed range for each property. Then, we add that to the total number of possibilities found, which is my `answer`. I love how this line of code is a vital part of the AoC solution for today:
```
answer += x * m * a * s;
```