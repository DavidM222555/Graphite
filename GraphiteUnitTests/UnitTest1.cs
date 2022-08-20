using System.Collections.Generic;
using System.Linq;
using Graphite.GraphCollectionUtils;
using Graphite.GraphUtils;
using NUnit.Framework;

namespace GraphiteUnitTests;

public class Tests
{
    [Test]
    public void AddNodeToGraphStringDict()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode");
        
        Assert.AreEqual(testGraph.StringToNode.ContainsKey("TestNode"), true);
    }

    [Test]
    public void AddNodeToList()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode");

        var nodeToFind = testGraph.StringToNode["TestNode"];
        
        Assert.Contains(nodeToFind, testGraph.NodesInGraph);
    }

    [Test]
    public void PropertyTest()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode");
        testGraph.AddPropertyToNode("TestNode", "TestProp");
        
        Assert.Contains("TestProp", testGraph.GetPropertiesOfNode("TestNode"));
    }

    [Test]
    public void PropertyUniqueTest()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode");
        testGraph.AddPropertyToNode("TestNode", "TestProp");
        testGraph.AddPropertyToNode("TestNode", "TestProp");
        
        // Make sure that the number of properties for test node is equal to 1 and not 2
        Assert.AreEqual(1, testGraph.GetPropertiesOfNode("TestNode").Count);
    }

    [Test]
    public void AddPropertiesToNodeTest()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode");

        var propsToAdd = new List<string>
        {
            "Prop1",
            "Prop2",
            "Prop3"
        };
        
        testGraph.AddPropertiesToNode("TestNode", propsToAdd);

        var countOfProp1 = testGraph.GetPropertiesOfNode("TestNode").Count(s => s == "Prop1");
        var countOfProp2 = testGraph.GetPropertiesOfNode("TestNode").Count(s => s == "Prop2");
        var countOfProp3 = testGraph.GetPropertiesOfNode("TestNode").Count(s => s == "Prop3");
        
        Assert.AreEqual(1, countOfProp1);
        Assert.AreEqual(1, countOfProp2);
        Assert.AreEqual(1, countOfProp3);
    }

    [Test]
    public void AddPropertyToMultipleNodes()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode1");
        testGraph.AddNodeFromString("TestNode2");

        var listOfNodes = new List<string>
        {
            "TestNode1",
            "TestNode2"
        };
        
        testGraph.AddPropertyToNodes(listOfNodes, "TestProp");
        
        var countOfPropInNode1 = testGraph.GetPropertiesOfNode("TestNode1").Count(s => s == "TestProp");
        var countOfPropInNode2 = testGraph.GetPropertiesOfNode("TestNode2").Count(s => s == "TestProp");

        Assert.AreEqual(1, countOfPropInNode1);
        Assert.AreEqual(1, countOfPropInNode2);
    }

    [Test]
    public void AddPropertiesToNodes()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode1");
        testGraph.AddNodeFromString("TestNode2");
        
        var listOfNodes = new List<string>
        {
            "TestNode1",
            "TestNode2"
        };

        var listOfProps = new List<string>
        {
            "TestProp1",
            "TestProp2"
        };
        
        testGraph.AddPropertiesToNodes(listOfNodes, listOfProps);
        
        var countOfProp1InNode1 = testGraph.GetPropertiesOfNode("TestNode1").Count(s => s == "TestProp1");
        var countOfProp2InNode1 = testGraph.GetPropertiesOfNode("TestNode1").Count(s => s == "TestProp2");
        var countOfProp1InNode2 = testGraph.GetPropertiesOfNode("TestNode2").Count(s => s == "TestProp1");
        var countOfProp2InNode2 = testGraph.GetPropertiesOfNode("TestNode2").Count(s => s == "TestProp2");

        Assert.AreEqual(1, countOfProp1InNode1);
        Assert.AreEqual(1, countOfProp2InNode1);
        Assert.AreEqual(1, countOfProp1InNode2);
        Assert.AreEqual(1, countOfProp2InNode2);
    }

    [Test]
    public void GetNodesWithProperty()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode1");
        testGraph.AddNodeFromString("TestNode2");
        testGraph.AddNodeFromString("TestNode3");
        
        testGraph.AddPropertyToNode("TestNode1", "TestProp");
        testGraph.AddPropertyToNode("TestNode2", "TestProp");

        var nodesWithProp = testGraph.GetNodesWithProperty("TestProp");

        var countOfTestPropInNode1 = nodesWithProp.Count(s => s == "TestNode1");
        var countOfTestPropInNode2 = nodesWithProp.Count(s => s == "TestNode2");

        Assert.AreEqual(1, countOfTestPropInNode1);
        Assert.AreEqual(1, countOfTestPropInNode2);
    }

    [Test]
    public void GetNeighborNodesByRelation()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode1");
        testGraph.AddNodeFromString("TestNode2");
        testGraph.AddNodeFromString("TestNode3");
        
        testGraph.AddDirectedRelation("TestNode1", "TestNode2", "Connected");
        testGraph.AddDirectedRelation("TestNode1", "TestNode1", "Connected");

        // Test to make sure we don't get nodes related along a different relation
        testGraph.AddDirectedRelation("TestNode1", "TestNode3", "Knows");

        var nodesRelatedByConnection = testGraph.GetRelatedNodes("TestNode1", "Connected");
        
        var isNode1Related = nodesRelatedByConnection.Count(s => s == "TestNode1");
        var isNode2Related = nodesRelatedByConnection.Count(s => s == "TestNode2");
        
        Assert.AreEqual(1, isNode1Related);
        Assert.AreEqual(1, isNode2Related);
        Assert.AreEqual(2, nodesRelatedByConnection.Count);
    }

    [Test]
    public void BidirectionalRelation()
    {
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode1");
        testGraph.AddNodeFromString("TestNode2");
        testGraph.AddNodeFromString("TestNode3");
        
        testGraph.AddBidirectionalEdge("TestNode1", "TestNode2", "Connected");

        var nodesRelatedToTestNode1 = testGraph.GetRelatedNodes("TestNode1", "Connected");
        var nodesRelatedToTestNode2 = testGraph.GetRelatedNodes("TestNode2", "Connected");
        
        var isNode1Related = nodesRelatedToTestNode1.Count(s => s == "TestNode2");
        var isNode2Related = nodesRelatedToTestNode2.Count(s => s == "TestNode1");
        
        Assert.AreEqual(1, isNode1Related);
        Assert.AreEqual(1, isNode2Related);
    }

    [Test]
    public void GetNodesWithProperties()
    {
        var graph = new Graph("TestGraph");
        graph.AddNodeFromString("TestNode1");
        graph.AddNodeFromString("TestNode2");

        graph.AddPropertyToNode("TestNode1", "TestProp1");
        graph.AddPropertyToNode("TestNode1", "TestProp2");
        
        graph.AddPropertyToNode("TestNode2", "TestProp1");
        
        var propertiesToTest = new List<string>
        {
            "TestProp1",
            "TestProp2"
        };

        var nodesWithProperties = graph.GetNodesWithProperties(propertiesToTest);
        
        Assert.AreEqual(1, nodesWithProperties.Count);
        Assert.AreEqual("TestNode1", nodesWithProperties[0]);
    }

    [Test]
    public void RemoveNodeWithPropertyTest()
    {
        var graph = new Graph("TestGraph");
           
        graph.AddNodeFromString("TestNode1");
        graph.AddNodeFromString("TestNode2");
        graph.AddNodeFromString("TestNode3");

        // Give test node 1 and test node 3 the same property
        graph.AddPropertyToNode("TestNode1", "TestProp1");
        graph.AddPropertyToNode("TestNode2", "TestProp2");
        graph.AddPropertyToNode("TestNode3", "TestProp1");

        graph.RemoveNodesWithProperty("TestProp1");
        
        // Check that the graph only has one node and that that node is equal to test node 2
        Assert.AreEqual(1, graph.NodesInGraph.Count);
        Assert.AreEqual(1, graph.StringToNode.Count);
        Assert.AreEqual("TestNode2", graph.NodesInGraph[0].NodeName);
    }
    
    [Test]
    public void RemoveNodesWithProperties()
    {
        var graph = new Graph("TestGraph");
        
        graph.AddNodeFromString("TestNode1");
        graph.AddNodeFromString("TestNode2");
        graph.AddNodeFromString("TestNode3");
        graph.AddNodeFromString("TestNode4");

        // Give test node 1 and test node 3 the same property
        graph.AddPropertyToNode("TestNode1", "TestProp1");
        graph.AddPropertyToNode("TestNode2", "TestProp2");
        graph.AddPropertyToNode("TestNode3", "TestProp1");
        graph.AddPropertyToNode("TestNode4", "TestProp3");

        var propsToRemove = new List<string>
        {
            "TestProp1",
            "TestProp2",
        };
        
        graph.RemoveNodesWithProperties(propsToRemove);
        
        Assert.AreEqual(1, graph.NodesInGraph.Count);
        Assert.AreEqual(1, graph.StringToNode.Count);
        Assert.AreEqual("TestNode4", graph.NodesInGraph[0].NodeName);
    }

    [Test]
    public void RemoveNodeWithName()
    {
        var graph = new Graph("TestGraph");
        
        graph.AddNodeFromString("TestNode1");
        graph.AddNodeFromString("TestNode2");
        
        graph.RemoveNodeWithName("TestNode1");
        
        Assert.AreEqual(1, graph.NodesInGraph.Count);
        Assert.AreEqual("TestNode2", graph.NodesInGraph[0].NodeName);
        Assert.AreEqual(1, graph.StringToNode.Count);
    }

    [Test]
    public void RemoveNodesWithNames()
    {
        var graph = new Graph("TestGraph");
        
        graph.AddNodeFromString("TestNode1");
        graph.AddNodeFromString("TestNode2");
        graph.AddNodeFromString("TestNode3");

        var listOfNodeNames = new List<string>()
        {
            "TestNode1",
            "TestNode2"
        };

        graph.RemoveNodesWithNames(listOfNodeNames);
        
        Assert.AreEqual(1, graph.NodesInGraph.Count);
        Assert.AreEqual(1, graph.StringToNode.Count);
        Assert.AreEqual("TestNode3", graph.NodesInGraph[0].NodeName);
    }
    
    [Test]
    public void SingleGraphFileWriteAndRead()
    {
        // Tests if we can successfully write a single graph to a file and retrieve it correctly
        var testGraph = new Graph("TestGraph");
        testGraph.AddNodeFromString("TestNode1");
        testGraph.AddNodeFromString("TestNode2");
        testGraph.AddDirectedRelation("TestNode1", "TestNode2", "Connected");

        const string path = "TestFile.txt";
        
        GraphCollection.WriteGraphToFile(testGraph, path);

        var testGraphCollectionFromRead = GraphCollection.FromFile(path);
        var testGraphFromRead = testGraphCollectionFromRead.NameToGraph["TestGraph"];
        
        Assert.True(testGraphFromRead.GraphName == testGraph.GraphName);
        
        // Basic first test is to make sure that all the nodes are contained in this graph
        foreach (var node in testGraph.NodesInGraph)
        {
            Assert.True(testGraphFromRead.NodesInGraph.Exists(s => s.NodeName == node.NodeName));
        }
    }

}