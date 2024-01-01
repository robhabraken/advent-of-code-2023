string[] lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\advent-of-code-2023-io\\25\\input.txt");

long answer = 0;

var components = new Dictionary<string, Component>();
var connections = new List<Connection>();
var groupOne = new List<Component>();

// read component list
foreach (var line in lines)
    components.Add(line[..3], new Component(line[..3]));

// create connections between components
foreach (var line in lines)
{
    var component = components[line[..3]];
    foreach (var connection in line[5..].Split(' '))
    {
        // if a component within the connection list doesn't exist yet, add it
        if (!components.ContainsKey(connection))
            components.Add(connection, new Component(connection));

        // create a bidirectional connection between both components
        component.connections.Add(components[connection]);
        components[connection].connections.Add(component);
    }
}

// calculate the shortest route to a component itself and create a connection object for each connection
// the rating assigned to a connection tells us how many steps it took to go via this route
foreach (var component in components.Values)
{
    foreach (var connection in component.connections)
    {
        var connectionRating = int.MaxValue;
        foreach (var secondaryConnection in connection.connections)
        {
            if (secondaryConnection != component)
            {
                var rating = FindShortestRouteToSelf(secondaryConnection, component, connection);
                if (rating < connectionRating)
                    connectionRating = rating;
            }
        }
        connections.Add(new Connection(component, connection, connectionRating));
    }
}

// sort the connections and take the 6 highest rated connections (we need to disconnect 3, but they're bidirectional)
// the routes that take longest to travel back over to the originating component are most likely to connect the two groups together
foreach (var connection in connections.OrderByDescending(x => x.rating).Take(6))
{
    // remove these connections
    foreach (var component in components.Values)
        if (component == connection.from)
            for (var i = 0; i < component.connections.Count; i++)
                if (component.connections[i] == connection.to)
                {
                    component.connections.Remove(component.connections[i]);
                    break;
                }
}

// take a random component and start collecting all connected components, that'll define the first group count, then multiply with what's left
FindConnections(components.Values.First());
answer = groupOne.Count * (components.Count - groupOne.Count);

Console.WriteLine(answer);

int FindShortestRouteToSelf(Component startingPoint, Component target, Component avoid)
{
    var depth = 3;
    var searchQueue = new Queue<Component>();
    var swapQueue = new Queue<Component>();

    foreach (var connection in startingPoint.connections)
    {
        if (connection == target)
            return depth;
        else if (connection != startingPoint && connection != avoid)
            searchQueue.Enqueue(connection);
    }

    while (true)
    {
        depth++;
        while (searchQueue.Any())
        {
            var component = searchQueue.Dequeue();
            foreach (var connection in component.connections)
            {
                if (connection == target)
                    return depth;
                else if (connection != component && connection != startingPoint && connection != avoid)
                    swapQueue.Enqueue(connection);
            }
        }

        searchQueue = new Queue<Component>(swapQueue);
        swapQueue.Clear();
    }
}

void FindConnections(Component component)
{
    groupOne.Add(component);

    foreach (var connection in component.connections)
        if (!groupOne.Contains(connection))
            FindConnections(connection);
}

class Component
{
    public string name;
    public List<Component> connections;

    public Component(string name)
    {
        this.name = name;
        connections = new List<Component>();
    }
}

class Connection
{
    public Component from;
    public Component to;
    public int rating;

    public Connection(Component from, Component to, int rating)
    {
        this.from = from;
        this.to = to;
        this.rating = rating;
    }
}