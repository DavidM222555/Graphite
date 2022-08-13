namespace Graphite.GraphUtils;

public class Graph
{
    public List<Node> NodesInGraph { get; }
    public Dictionary<string, Node> StringToNode { get; }
    public string GraphName { get; }
    
    public Graph(string graphName)
    {
        NodesInGraph = new List<Node>();
        StringToNode = new Dictionary<string, Node>();
        GraphName = graphName;
    }

    /// <summary>
    /// Gets the node corresponding to a given name. Names should be guaranteed to be unique
    /// because of this
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns>Returns node with a given name or null if it doesn't exist</returns>
    public Node? GetNodeFromName(string nodeName)
    {
        return StringToNode.ContainsKey(nodeName) ? StringToNode[nodeName] : null;
    }

    /// <summary>
    /// Create a node in the graph based off a given string name -- begins by testing
    /// if the node is already in the graph or not, if it is then we don't modify the structure at all
    /// </summary>
    /// <param name="nodeName"></param>
    public void AddNodeFromString(string nodeName)
    {
        // If we already have the node string in the graph we just return.
        if (StringToNode.ContainsKey(nodeName))
        {
            return;
        }
        
        var nodeToAdd = new Node(nodeName);
        
        NodesInGraph.Add(nodeToAdd);
        StringToNode[nodeName] = nodeToAdd;
    }

    /// <summary>
    /// Add a relation from startingNode to endingNode.
    /// </summary>
    /// <param name="startingNodeName">Name corresponding to starting node</param>
    /// <param name="endingNodeName">Name corresponding to ending node</param>
    /// <param name="relationName">The name of the relation between these nodes</param>
    public void AddDirectedRelation(string startingNodeName, string endingNodeName, string relationName)
    {
        // If the node doesn't already exist in the graph we will create it
        if (!StringToNode.ContainsKey(startingNodeName))
        {
            AddNodeFromString(startingNodeName);
        }

        if (!StringToNode.ContainsKey(endingNodeName))
        {
            AddNodeFromString(endingNodeName);
        }

        var startingNode = StringToNode[startingNodeName];
        var endingNode = StringToNode[endingNodeName];
        
        startingNode.AddRelationToEdge(endingNode, relationName);
    }

    /// <summary>
    /// Add a bidirectional relation between two nodes. For instance, if you want to simplify the process
    /// of adding a 'knows' relation that both nodes have with one another you can use this method instead of
    /// the unidirectional AddDirectedEdge method.
    /// </summary>
    /// <param name="startingNodeName"></param>
    /// <param name="endingNodeName"></param>
    /// <param name="relationName"></param>
    public void AddBidirectionalEdge(string startingNodeName, string endingNodeName, string relationName)
    {
        AddDirectedRelation(startingNodeName, endingNodeName, relationName);
        AddDirectedRelation(endingNodeName, startingNodeName, relationName);
    }

    /// <summary>
    /// Gets all nodes related to the calling node by relationToTest
    /// </summary>
    /// <param name="startingNode"></param>
    /// <param name="relationToTest">Relation property to test</param>
    /// <returns>A list of nodes related by relationToTest to startingNode</returns>
    public List<string> GetRelatedNodes(string startingNode, string relationToTest)
    {
        if (StringToNode[startingNode].RelationToNodes.ContainsKey(relationToTest))
        {
            return StringToNode[startingNode].RelationToNodes[relationToTest].Select(node => node.NodeName).ToList();
        }

        return new List<string>();
    }

    /// <summary>
    /// Adds a property to a given node. If node is not already in the graph then we add the node
    /// to the graph and then add the property to it.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="propertyName"></param>
    public void AddPropertyToNode(string nodeName, string propertyName)
    {
        if (!StringToNode.ContainsKey(nodeName))
        {
            AddNodeFromString(nodeName);
        }
        
        StringToNode[nodeName].AddUserDefinedProperty(propertyName);
    }

    /// <summary>
    /// Adds a given property to multiple nodes.
    /// </summary>
    /// <param name="nodeNames">List of node names we want to add a property to</param>
    /// <param name="propertyName">Name of property we want to apply to these nodes</param>
    public void AddPropertyToNodes(List<string> nodeNames, string propertyName)
    {
        foreach (var nodeName in nodeNames)
        {
            AddPropertyToNode(nodeName, propertyName);
        }
    }

    /// <summary>
    /// Adds multiple properties to a given node.
    /// </summary>
    /// <param name="nodeName">Name of node to add properties to</param>
    /// <param name="properties">List of properties to give a given node</param>
    public void AddPropertiesToNode(string nodeName, List<string> properties)
    {
        foreach (var property in properties)
        {
            AddPropertyToNode(nodeName, property);
        }
    }
    
    /// <summary>
    /// Adds given properties to every node in nodeNames
    /// </summary>
    /// <param name="nodeNames"></param>
    /// <param name="properties"></param>
    public void AddPropertiesToNodes(List<string> nodeNames, List<string> properties)
    {
        foreach (var property in properties)
        {
            foreach (var node in nodeNames)
            {
                AddPropertyToNode(node, property);
            }
        }
    }

    /// <summary>
    /// Helper function for checking the current relations between nodes in the graph.
    /// </summary>
    public void DisplayAllNodesAndRelations()
    {
        foreach (var node in NodesInGraph)
        {
            Console.Write(node.NodeName + " has the relations: \n");
            
            foreach (var (key, value) in node.EdgeTable)
            {
                foreach (var relation in value)
                {
                    Console.Write(relation + " to " + key.NodeName + " \n");
                }
            }
        }
    }

    public List<string> GetPropertiesOfNode(string nodeName)
    {
        return StringToNode[nodeName].UserDefinedProperties.ToList();
    }

    /// <summary>
    /// Retrieves all the nodes in the graph with a given property name
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns>A list of nodes with a given property</returns>
    public List<string> GetNodesWithProperty(string propertyName)
    {
        return (from node in NodesInGraph where node.UserDefinedProperties.Contains(propertyName) select node.NodeName).ToList();
    }
    
}