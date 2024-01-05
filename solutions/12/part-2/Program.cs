string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\12\\input.txt");

/*
 * Disclaimer: though this code is all mine, the idea behind it isn't. I wasn't able to solve this
 * puzzle completely on my own (for part 1 I did, which unfortunately is a brute-force approach that didn't work for part 2).
 * The implementation below is based on logic of others, used by me to learn from implementing such patterns.
 */

var cache = new Dictionary<string, long>();

long answer = 0;
foreach (string line in lines)
{
    // read input data
    var springs = line.Split(' ')[0];
    var groupList = line.Split(' ')[1];

    // unfold the records
    springs = $"{springs}?{springs}?{springs}?{springs}?{springs}";
    groupList = $"{groupList},{groupList},{groupList},{groupList},{groupList}";

    // parse groups into integer list
    var groups = new List<int>();
    foreach (var group in groupList.Split(','))
        groups.Add(int.Parse(group));

    // count the different arrangements
    answer += Count(springs, groups);
}

Console.WriteLine(answer);

long Count(string springs, List<int> groups)
{
    // scenario 1: springs is empty
    if (string.IsNullOrEmpty(springs))
        // if there are also no groups left, this is a valid arrangement, so count 1
        if (!groups.Any())
            return 1;
        else
            // if there still are groups left, but no more springs, this arrangement is invalid
            return 0;

    // scenario 2: no groups left
    if (!groups.Any())
        // if there are also no broken springs left this arrangement is still valid, so count 1
        // (meaning all remaining characters are either . or ?, all being operational springs)
        if (!springs.Contains('#'))
            return 1;
        else
            // if there are still broken springs left, but no groups, this arrangement is invalid
            return 0;

    // use memoization from here onwards to greatly increase execution time
    var cacheKey = CreateCacheKey(springs, groups);
    if (cache.ContainsKey(cacheKey))
        return cache[cacheKey];

    long count = 0;

    // if the first character of springs is either a . or a ?, treat this character as an operational spring
    // and count the amount of valid arrangement of the rest of the springs, adding that to the count for this arrangement
    // (if the question mark indeed would be an operational spring, the rest of the springs list should still be valid without the first spring)
    if (".?".Contains(springs[0]))
        count += Count(springs[1..], groups);

    // if the first character of springs is either a # or a ?, treat this character as a damaged spring
    // meaning this would mark the start of a group, but in order for that to be valid, we need to check if:
    // - the value of the first group is not higher than the current springs length (if so, the group wouldn't fit in the current springs list)
    // - there should be no operational springs within the length of the first group
    // - the next character _after_ the length of the first group cannot be a damaged spring (it can be a . or a ? though)
    //   or this group of damaged springs is the last one in the list and thus the length of springs equals the size of the first group
    if ("#?".Contains(springs[0]) &&
        groups[0] <= springs.Length &&
        !springs[..groups[0]].Contains('.') &&
        (springs.Length == groups[0] || !springs[groups[0]].Equals('#')))
    {
        // if all conditions are met, this arrangement could be valid, so remove both the first group from the list and the corresponding number of springs
        // and count the number of arrangements of what's left and add that to the count for this arrangement
        var groupsCopy = new List<int>(groups);
        groupsCopy.RemoveAt(0);
        count += Count(Substring(springs, groups[0] + 1), groupsCopy);
    }

    // add the result to the cache and return the calculated value
    cache.Add(cacheKey, count);
    return count;
}

string CreateCacheKey(string springs, List<int> groups)
{
    foreach (var group in groups)
        springs += $"-{group}";
    return springs;
}

// this is an out-of-bounds-safe substring equivalent to the default C# one
string Substring(string input, int from)
{
    var result = string.Empty;
    if (input.Length > from)
        result = input[from..];
    return result;
}