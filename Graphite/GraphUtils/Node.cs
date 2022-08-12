namespace Graphite.GraphUtils;

public class Node
{
    public string NodeName { get; set; }
    
    public HashSet<string> UserDefinedProperties { get; }
    public Dictionary<Node, List<UserDefinedRelation>> EdgeTable { get; }
    
    public Node(string nodeName)
    {
        NodeName = nodeName;
        UserDefinedProperties = new HashSet<string>();
        EdgeTable = new Dictionary<Node, List<UserDefinedRelation>>();
    }

    public void AddUserDefinedProperty(string newProperty)
    {
        UserDefinedProperties.Add(newProperty);
    }

    public void CreateEdge(Node outgoingNode)
    {
        if (!EdgeTable.ContainsKey(outgoingNode))
        {
            EdgeTable.Add(outgoingNode, new List<UserDefinedRelation>());
        }
    }

    public List<UserDefinedRelation> GetRelationsBetweenNodes(Node outgoingNode)
    {
        return EdgeTable.ContainsKey(outgoingNode) ? EdgeTable[outgoingNode] : new List<UserDefinedRelation>();
    }

    public void AddRelationToEdge(Node outgoingNode, UserDefinedRelation relationToAdd)
    {
        // If we don't have a list of user defined relations for an outgoing node
        // yet then we create a new list for it
        if (!EdgeTable.ContainsKey(outgoingNode))
        {
            EdgeTable.Add(outgoingNode, new List<UserDefinedRelation>());
            var relations = GetRelationsBetweenNodes(outgoingNode);
            relations.Add(relationToAdd);

            return;
        }
        
        EdgeTable[outgoingNode].Add(relationToAdd);
    }
    
}