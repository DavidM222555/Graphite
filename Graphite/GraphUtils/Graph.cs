namespace Graphite.GraphUtils;

public class Graph
{
    public List<Node> NodesInGraph { get; }
    public Dictionary<string, Node> StringToNode { get; }

    public Graph()
    {
        NodesInGraph = new List<Node>();
        StringToNode = new Dictionary<string, Node>();
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
        
        startingNode.AddRelationToEdge(endingNode, new UserDefinedRelation(relationName));
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
            
            foreach (var entry in node.EdgeTable)
            {
                foreach (var relation in entry.Value)
                {
                    Console.Write(relation.RelationName + " to " + entry.Key.NodeName + " \n");
                }
            }
        }
    }

    public List<string> GetPropertiesOfNode(string nodeName)
    {
        return StringToNode[nodeName].UserDefinedProperties.ToList();
    }
    
}