# Solutions to Day 20: Pulse Propagation

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Today's puzzle was a lot of fun. A lot of reading too though, especially getting to understand the exact logic behind the different module types. Once I got passed that, it didn't seem all that difficult. I started with creating a `Module` class that stored all properties (of all types combined) using the constructor to parse a single input line to build a module object. I've added a `ProcessPulse()` function to process a received pulse and send it to the next module. If you read the instructions carefully, it's clear what each module type does:

- if broadcast, send to all listeners (destination modules)
- if flip-flop, do nothing on receiving a high value, and when receiving a low value, send a high value when in the off state and a low value in the on state 
- if conjuction, first store the received value _for the input module that sent this pulse value_, then check if all values received from all input modules that lead to this module are high pulses, and then send out a low pulse, otherwise always a high: for a single input node this is just an inversion, but for multiple input modules leading to a conjuction it builds up over time, that's important

My process function only processes a pulse and determines the pulse output, and a separate `SendPulse()` function sends that pulse to all destinations (or listeners or subscribers if you like, as it's actually a sort of pub-sub pattern).

What is also explicitely stated, is that all pulses have to be processed in the same order as they're sent! This means, that if you just call `ProcessPulse()` for each signal sent, which also sends out new pulses, the order is incorrect. Because a module that has multiple listeners would start multiple paths across the machines, which functions as a sort of recursion, so each first path will be completely traversed until it gets at the next start of a branch. To solve this, I simply added a `Queue<Pulse>` object to my code and made it so that the `SendPulse()` doesn't actually send a pulse to another module, but enqueues a pulse, while my main loop continuously dequeues pulses from the pulse queue and only then actually sends them to the desired module. This way, I can garantuee that each pulse is processed in the same order as they were sent out.

One last thing to do, is to create a memory list within a module and, register all the inputs that lead to that module, and start storing the last value of each input to a certain module in that receiving module. This required two additional loops after parsing the input, as you first need to know all modules to be able to populate that data - that's why I didn't do that in the constructor of a module object.

Now I could send out the first pulse, like pushing the button and let my module queue handle and process all of the signals until no more pulses were sent. And then repeat that a 1000 times. As sending out a pulse is done from a module, it was most efficient to count the pulses being sent within the modules, and then looping over all modules when ready adding up the pulse counts for both the low and high value to a grand total, and return the product of that as my `answer`.

I really enjoyed building this little gem of code, it was mostly programming skills and not so much a mathematical challenge (yet!), but the complexity of the assignment asked for some clever solutions without getting too complicated. This type of puzzles really suits me well.

## Part 2

I first just added a check to see what the pulses towards the `rx` node were, and let it run for a little while, but this only confirmed what I already expected: this is going to take a very long while and we obviously (it's day 20) can't brute-force the answer! So I figured, in order to be able to calculate the answer, there _has_ to be some sort of cycling going on. And if there is a repetitive pattern (as we've seen earlier on day 8) you can calculate when all those patterns meet with the LCM. So I decided to add two attributes to the `Module` class: `lastOutput` and `cycleSize` and add the following piece of code for each pulse being process (regardless of the type):
```
if (lastOutput != output && cycleSize == 0)
    cycleSize = buttonPressed;
lastOutput = output;
```
Basically, what this does, is store the number of times the button was pressed until the output value changes for the first time after the start. Hence, we know the length of the cycle for this specifc `Module`.

Then I just ran my button press script with a while-loop for as long as all the cycle sizes where set and wrote the output to the console. Obviously, this automatically sorts the modules from shortest to longest cycle time. And then I noticed something: the conjunction modules are all at the end of the chain, and the last four ones leading to the last module all have irregular cycle lenghts, while all others are factors of two. So I figured there are multiple larger loops leading to a number of conjuction modules that simultaneously all have to be receiving a high value, in order to make the last module send a low value to the final machine named `rx`.

In other words, the answer is the least common multiple of those four cycle times. I did this using prime factorization, unifying the prime factors for each cycle time of the input module for the last module and then return the product of those.