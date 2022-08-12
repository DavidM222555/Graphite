using Graphite.GraphUtils;

var graphTest = new Graph();
graphTest.AddNodeFromString("Node1");
graphTest.AddNodeFromString("Node2");
graphTest.AddNodeFromString("Node3");

graphTest.AddDirectedRelation("Node1", "Node2", "Connected");
graphTest.AddDirectedRelation("Node2", "Node3", "Connected");
graphTest.AddDirectedRelation("Node3", "Node1", "Connected");
graphTest.AddDirectedRelation("Node1", "Node1", "Connected");
graphTest.AddDirectedRelation("Node1", "Node1", "Knows");

graphTest.AddPropertyToNode("Node1", "Smart");

var properties = new List<string>
{
    "Prop1",
    "Prop2",
    "Prop3"
};

graphTest.AddPropertyToNode("Node1", "Smart");
// graphTest.AddPropertiesToNode("Node1", properties);

var propsForNode = graphTest.GetPropertiesOfNode("Node1");

foreach (var prop in propsForNode)
{
    Console.WriteLine(prop);
}

// graphTest.DisplayAllNodesAndRelations();