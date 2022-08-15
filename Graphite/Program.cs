using Graphite.GraphCollectionUtils;
using Graphite.GraphUtils;
using Graphite.ProgramUtils;


ProgramRunner.Run();

var graph = new Graph("People");
graph.AddNodeFromString("Dave");
graph.AddNodeFromString("Bob");
graph.AddNodeFromString("Charlie");
graph.AddNodeFromString("Chuck");

graph.AddPropertyToNode("Dave", "Smart");
graph.AddPropertyToNode("Dave", "Blue");
graph.AddPropertyToNode("Charlie", "Smart");
graph.AddPropertyToNode("Charlie", "Blue");

graph.AddDirectedRelation("Dave", "Bob", "Knows");

// var smartPeople = graph.GetNodesWithProperty("Smart");

var propertiesToTest = new List<string>
{
    "Smart",
    "Blue"
};

var nodesWithProperties = graph.GetNodesWithProperties(propertiesToTest);

foreach (var node in nodesWithProperties)
{
    Console.WriteLine(node);
}

// foreach (var person in smartPeople)
// {
//     Console.WriteLine(person);
// }