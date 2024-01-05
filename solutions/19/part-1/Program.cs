string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\19\\input.txt");

var startWorkflow = new Workflow();
var workflows = new Dictionary<string, Workflow>();
var parts = new List<Part>();

var answer = 0;
var passedBlankLine = false;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        passedBlankLine = true;
        continue;
    }

    if (!passedBlankLine)
    {
        var workflow = new Workflow(line);
        workflows.Add(workflow.name, workflow);
        if (workflow.name.Equals("in"))
            startWorkflow = workflow;
    }
    else
        parts.Add(new Part(line));
}

foreach (var part in parts)
{
    var result = startWorkflow.Process(part);

    while (!result.Equals("R") && !result.Equals("A"))
        result = workflows[result].Process(part);

    if (result.Equals("A"))
        answer += part.rating;
}

Console.WriteLine(answer);

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

    public string Process(Part part)
    {
        string result = string.Empty;
        foreach (var rule in rules)
        {
            result = rule.ApplyRule(part);
            if (result != null)
                break;
        }
        return result;
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

    public string ApplyRule(Part part)
    {
        if (string.IsNullOrEmpty(operatorSymbol))
        {
            return target;
        }
        else if (operatorSymbol.Equals("<"))
        {
            if (part.properties[propertyName] < referenceValue)
                return target;
        }
        else
        {
            if (part.properties[propertyName] > referenceValue)
                return target;
        }

        return null;
    }
} 

class Part
{
    public Dictionary<string, int> properties;
    public int rating;

    public Part(string input)
    {
        properties = new Dictionary<string, int>();
        foreach (var property in input[1..^1].Split(','))
            properties.Add(property.Split('=')[0], int.Parse(property.Split('=')[1]));

        rating = 0;
        foreach (var property in properties.Values)
            rating += property;
    }
}