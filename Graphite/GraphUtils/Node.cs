namespace Graphite.GraphUtils;

public class Node
{
    public string NodeName { get; set; }
    public HashSet<string> UserDefinedProperties { get; }
    public Dictionary<Node, List<string>> EdgeTable { get; }
    public Dictionary<string, List<Node>> RelationToNodes { get; }

    public Node(string nodeName)
    {
        NodeName = nodeName;
        UserDefinedProperties = new HashSet<string>();
        EdgeTable = new Dictionary<Node, List<string>>();
        RelationToNodes = new Dictionary<string, List<Node>>();
    }

    public void AddUserDefinedProperty(string newProperty)
    {
        UserDefinedProperties.Add(newProperty);
    }

    public List<string> GetRelationsBetweenNodes(Node outgoingNode)
    {
        return EdgeTable.ContainsKey(outgoingNode) ? EdgeTable[outgoingNode] : new List<string>();
    }

    public void AddRelationToEdge(Node outgoingNode, string relationToAdd)
    {
        Console.WriteLine("Adding relation " + relationToAdd);
        
        // Begin by testing whether or not the relations to nodes dictionary has a list for
        // the relation under consideration
        if (!RelationToNodes.ContainsKey(relationToAdd))
        {
            RelationToNodes.Add(relationToAdd, new List<Node>{outgoingNode});
        }
        else
        {
            RelationToNodes[relationToAdd].Add(outgoingNode);
        }
        
        // If we don't have a list of user defined relations for an outgoing node
        // yet then we create a new list for it
        if (!EdgeTable.ContainsKey(outgoingNode))
        {
            EdgeTable.Add(outgoingNode, new List<string>());
            var relations = GetRelationsBetweenNodes(outgoingNode);
            relations.Add(relationToAdd);

            return;
        }

        EdgeTable[outgoingNode].Add(relationToAdd);
    }
    
    
}