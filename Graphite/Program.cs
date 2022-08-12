using Graphite.GraphUtils;

var graphTest = new Graph();
graphTest.AddNodeFromString("Node1");
graphTest.AddNodeFromString("Node2");
graphTest.AddNodeFromString("Node3");

graphTest.AddDirectedRelation("Node1", "Node2", "Connected");
graphTest.AddDirectedRelation("Node1", "Node1", "Connected");

graphTest.AddPropertyToNode("Node1", "Smart");

var relatedNodes = graphTest.GetRelatedNodes("Node1","Connected");

foreach (var relatedNode in relatedNodes) 
{
    Console.WriteLine(relatedNode);
}

// graphTest.DisplayAllNodesAndRelations();