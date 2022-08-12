namespace Graphite.GraphUtils;

public class Edge
{
    public Node OutgoingNode { get; }
    public HashSet<UserDefinedRelation> Relations { get; }

    Edge(Node startingNode, Node outgoingNode)
    {
        OutgoingNode = outgoingNode;
        Relations = new HashSet<UserDefinedRelation>();
    }

    public void AddRelation(UserDefinedRelation relationToAdd)
    {
        Relations.Add(relationToAdd);
    }

    public bool CheckRelation(UserDefinedRelation relationToCheck)
    {
        return Relations.Contains(relationToCheck);
    }

}