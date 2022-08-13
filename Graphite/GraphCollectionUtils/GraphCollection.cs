using Graphite.GraphUtils;

namespace Graphite.GraphCollectionUtils;

public class GraphCollection
{
    public Dictionary<string, Graph> NameToGraph { get; }

    public GraphCollection()
    {
        NameToGraph = new Dictionary<string, Graph>();
    }

    /// <summary>
    /// Creates a graph based off the following graph encoding scheme:
    /// #GraphDef
    /// GraphName
    /// #GraphDefEnd
    /// #NodeDef
    /// Node1,Node2,Node3,...
    /// #NodeDefEnd
    /// #RelationDef
    /// Node1,Relation,Node2
    /// Node2,Relation,Node3
    /// #EndRelationDef
    /// ... Any more graph defs you want
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static GraphCollection FromFile(string filePath)
    {
        var graphCollection = new GraphCollection();
        var lines = File.ReadAllLines(@filePath);
        
        Graph? currentGraph = null;
        var graphName = "";

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "#GraphDef")
            {
                i++;
                graphName = lines[i];
                currentGraph = new Graph(graphName);

                while (lines[i] != "#GraphDefEnd")
                {
                    i++;
                }
            }

            if (lines[i] == "#NodeDef")
            {
                i++;
                
                while (lines[i] != "#NodeDefEnd")
                {
                    if (currentGraph is null)
                    {
                        throw new InvalidDataException("Graph hasn't been defined. Check to make sure you have #GraphDef");
                    }
                    
                    currentGraph.AddNodeFromString(lines[i]);
                    i++;
                }
            }

            if (lines[i] == "#RelationDef")
            {
                i++;
                
                while (lines[i] != "#RelationDefEnd")
                {
                    if (currentGraph is null)
                    {
                        throw new InvalidDataException(
                            "Graph hasn't been defined. Check to make sure you have #GraphDef");
                    }

                    var splitRelation = lines[i].Split(',');
                    
                    if (splitRelation.Length < 3)
                    {
                        throw new InvalidDataException("Relation definition doesn't contain three items");
                    }

                    var startingNode = splitRelation[0];
                    var relationName = splitRelation[1];
                    var endingNode = splitRelation[2];
                    
                    // Test to make sure the starting node and ending node are in the graph
                    if (!currentGraph.StringToNode.ContainsKey(startingNode) || !currentGraph.StringToNode.ContainsKey(endingNode))
                    {
                        throw new InvalidDataException("Starting node and ending node are not in graph. " +
                                                       "Make sure you included them in #NodeDef");
                    }
                    
                    currentGraph.AddDirectedRelation(startingNode, endingNode, relationName);
                    
                    i++;
                }

                if (currentGraph != null) graphCollection.NameToGraph[graphName] = currentGraph;
                currentGraph = null;
            }
        }
            
        return graphCollection;
    }

    /// <summary>
    /// Writes a graph to a file with the proper encoding discussed in the FromFile construction.
    /// </summary>
    /// <param name="graphToWrite"></param>
    /// <param name="filePath"></param>
    public static void WriteGraphToFile(Graph graphToWrite, string filePath)
    {
        using var outputFile = new StreamWriter(filePath, false);
        
        // Write graph node name section
        var nameToWrite = graphToWrite.GraphName;
            
        outputFile.WriteLine("#GraphDef");
        outputFile.WriteLine(nameToWrite);
        outputFile.WriteLine("#GraphDefEnd");

        // Write node section
        outputFile.WriteLine("#NodeDef");

        foreach (var node in graphToWrite.NodesInGraph)
        {
            outputFile.WriteLine(node.NodeName);
        }

        outputFile.WriteLine("#NodeDefEnd");
        
        // Write relation section
        outputFile.WriteLine("#RelationDef");
        
        foreach (var node in graphToWrite.NodesInGraph)
        {
            foreach (var (outgoingNode, relationList) in node.EdgeTable)
            {
                foreach (var relation in relationList)
                {
                    var stringToWrite = node.NodeName + "," + relation + "," + outgoingNode.NodeName;

                    outputFile.WriteLine(stringToWrite);
                }
            }
        }
        
        outputFile.WriteLine("#RelationDefEnd");
    }

    public void AddGraphToCollectionByName(string graphName)
    {
        var newGraph = new Graph("graphName");

        NameToGraph[graphName] = newGraph;
    }
    
}