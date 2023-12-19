string[] lines = File.ReadAllLines("..\\..\\..\\..\\input.txt");

var startWorkflow = new Workflow();
var workflows = new Dictionary<string, Workflow>();

long answer = 0;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
        break;

    var workflow = new Workflow(line);
    workflows.Add(workflow.name, workflow);
    if (workflow.name.Equals("in"))
        startWorkflow = workflow;
}

var startPath = new List<Boundary>();
FindAcceptedPaths(startWorkflow, startPath);

void FindAcceptedPaths(Workflow workflow, List<Boundary> path)
{
    foreach (var rule in workflow.rules)
    {
        if (string.IsNullOrEmpty(rule.operatorSymbol))
        {
            var newBoundary = new Boundary(string.Empty, Boundary.MIN, Boundary.MAX, rule.target); // unconditional
            path.Add(newBoundary);
            if (newBoundary.rejected || newBoundary.accepted)
                Analyze(path); // end of path
            else
                FindAcceptedPaths(workflows[rule.target], path); // move over to next workflow
            return;
        }
        else
        {
            var newBoundary = new Boundary(rule);
            if (newBoundary.rejected || newBoundary.accepted)
            {
                var nextList = new List<Boundary>(path);
                nextList.Add(new Boundary(rule));
                Analyze(nextList); // end of path
            }
            else
            {
                var nextList = new List<Boundary>(path);
                nextList.Add(newBoundary);
                FindAcceptedPaths(workflows[rule.target], nextList); // move over to next workflow
            }
            var reverseBoundary = new Boundary(rule, true);
            path.Add(reverseBoundary); // continue this workflow rules with opposite boundary
        }
    }
}

void Analyze(List<Boundary> path)
{
    if (path[^1].rejected)
        return;

    var minValues = new Dictionary<string, long>();
    minValues.Add("x", Boundary.MIN);
    minValues.Add("m", Boundary.MIN);
    minValues.Add("a", Boundary.MIN);
    minValues.Add("s", Boundary.MIN);

    var maxValues = new Dictionary<string, long>();
    maxValues.Add("x", Boundary.MAX);
    maxValues.Add("m", Boundary.MAX);
    maxValues.Add("a", Boundary.MAX);
    maxValues.Add("s", Boundary.MAX);

    foreach (var boundary in path)
    {
        if (!string.IsNullOrEmpty(boundary.propertyName)) // not unconditional
        {
            if (boundary.from > minValues[boundary.propertyName])
                minValues[boundary.propertyName] = boundary.from;

            if (boundary.to < maxValues[boundary.propertyName])
                maxValues[boundary.propertyName] = boundary.to;
        }
    }

    var x = maxValues["x"] - minValues["x"] + 1;
    var m = maxValues["m"] - minValues["m"] + 1;
    var a = maxValues["a"] - minValues["a"] + 1;
    var s = maxValues["s"] - minValues["s"] + 1;

    answer += x * m * a * s;
}

Console.WriteLine(answer);

class Boundary
{
    public static readonly int MIN = 1;
    public static readonly int MAX = 4000;

    public string propertyName;
    public int from;
    public int to;

    public bool rejected;
    public bool accepted;

    public Boundary(Rule rule, bool inverse = false)
    {
        propertyName = rule.propertyName;

        if (!inverse)
        {
            if (rule.operatorSymbol.Equals("<"))
            {
                from = MIN;
                to = rule.referenceValue - 1;
            }
            else
            {
                from = rule.referenceValue + 1;
                to = MAX;
            }
        }
        else
        {
            if (rule.operatorSymbol.Equals("<"))
            {
                from = rule.referenceValue;
                to = MAX;
            }
            else
            {
                from = MIN;
                to = rule.referenceValue;
            }
        }

        rejected = rule.target.Equals("R");
        accepted = rule.target.Equals("A");
    }

    public Boundary(string propertyName, int from, int to, string target)
    {
        this.propertyName = propertyName;
        this.from = from;
        this.to = to;

        rejected = target.Equals("R");
        accepted = target.Equals("A");
    }
}

class Workflow
{
    public string name;
    public List<Rule> rules;

    public Workflow() { }

    public Workflow(string input)
    {
        var curlyIndex = input.IndexOf('{');
        name = input.Substring(0, curlyIndex);

        rules = new List<Rule>();
        var rulesInput = input.Substring(++curlyIndex).Replace("}", string.Empty);
        foreach (var ruleInput in rulesInput.Split(','))
            rules.Add(new Rule(ruleInput));
    }
}

class Rule
{
    public string propertyName;
    public string operatorSymbol;
    public int referenceValue;
    public string target;

    public Rule(string input)
    {
        if (input.Contains(':'))
        {
            propertyName = input[0].ToString();
            operatorSymbol = input[1].ToString();
            referenceValue = int.Parse(input.Split(':')[0][2..]);
            target = input.Split(':')[1];
        }
        else
            target = input;
    }
}