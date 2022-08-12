using System.Collections.Generic;
using System.Linq;
using Graphite.GraphUtils;
using NUnit.Framework;

namespace GraphiteUnitTests;

public class Tests
{
    [Test]
    public void AddNodeToGraphStringDict()
    {
        var testGraph = new Graph();
        testGraph.AddNodeFromString("TestNode");
        
        Assert.AreEqual(testGraph.StringToNode.ContainsKey("TestNode"), true);
    }

    [Test]
    public void AddNodeToList()
    {
        var testGraph = new Graph();
        testGraph.AddNodeFromString("TestNode");

        var nodeToFind = testGraph.StringToNode["TestNode"];
        
        Assert.Contains(nodeToFind, testGraph.NodesInGraph);
    }

    [Test]
    public void PropertyTest()
    {
        var testGraph = new Graph();
        testGraph.AddNodeFromString("TestNode");
        testGraph.AddPropertyToNode("TestNode", "TestProp");
        
        Assert.Contains("TestProp", testGraph.GetPropertiesOfNode("TestNode"));
    }

    [Test]
    public void PropertyUniqueTest()
    {
        var testGraph = new Graph();
        testGraph.AddNodeFromString("TestNode");
        testGraph.AddPropertyToNode("TestNode", "TestProp");
        testGraph.AddPropertyToNode("TestNode", "TestProp");
        
        // Make sure that the number of properties for test node is equal to 1 and not 2
        Assert.AreEqual(1, testGraph.GetPropertiesOfNode("TestNode").Count);
    }

    [Test]
    public void AddPropertiesToNodeTest()
    {
        var testGraph = new Graph();
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
        var testGraph = new Graph();
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
        var testGraph = new Graph();
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
}