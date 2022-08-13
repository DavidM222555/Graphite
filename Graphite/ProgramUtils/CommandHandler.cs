using Graphite.GraphCollectionUtils;

namespace Graphite.ProgramUtils;

public static class CommandHandler
{
    public static void HandleCommand(string? command, GraphCollection graphCollection)
    {
        if (string.IsNullOrEmpty(command))
        {
            return;
        }
        
        var commandWords = command.Split(" ");

        switch (commandWords[0])
        {
            // Command of the form: CREATE <graphName>
            case "CREATE":
                if (commandWords.Length < 2)
                {
                    throw new InvalidDataException("Invalid command length. Should have length 2.");
                }

                var graphNameToAdd = commandWords[1];
                CreateGraphInCollection(graphNameToAdd, graphCollection);
                
                Console.WriteLine("Graph " + graphNameToAdd + " successfully added.");
                
                break;
            
            case "ADD":
                if (command.Length < 4)
                {
                    throw new InvalidDataException("Invalid command length. Should have length 4.");
                }

                var nodeNameToAdd = commandWords[1];
                var graphNameToAddTo = commandWords[3];
                
                AddNodeToGraph(graphCollection, graphNameToAddTo, nodeNameToAdd);
                
                break;
            case "RELATE":
                if (command.Length < 8)
                {
                    throw new InvalidDataException("Invalid command length. Should have length 4.");
                }

                var startNode = commandWords[1];
                var endNode = commandWords[3];
                var relationName = commandWords[5];
                var graphName = commandWords[7];
                
                AddRelationToGraph(graphCollection, graphName, startNode, endNode, relationName);
                break;
                
            case "GET":
                var properties = commandWords[3].Split(",");
                

                break;

        }
    }

    private static void CreateGraphInCollection(string graphName, GraphCollection graphCollection)
    {
        // If the graph collection already contains the given name we just return
        if (graphCollection.NameToGraph.ContainsKey(graphName))
        {
            return;
        }    
        
        graphCollection.AddGraphToCollectionByName(graphName);
    }

    private static void AddNodeToGraph(GraphCollection graphCollection, string graphName, string nodeNameToAdd)
    {
        if (!graphCollection.NameToGraph.ContainsKey(graphName))
        {
            
            return;
        }

        var graphToAddTo = graphCollection.NameToGraph[graphName];
        
        graphToAddTo.AddNodeFromString(nodeNameToAdd);
    }

    private static void AddRelationToGraph(GraphCollection graphCollection, string graphName, string startNodeName,
        string endNodeName, string relationName)
    {
        var graphToAddTo = graphCollection.NameToGraph[graphName];
        
        if (!graphToAddTo.StringToNode.ContainsKey(startNodeName) || !graphToAddTo.StringToNode.ContainsKey(endNodeName))
        {
            throw new InvalidDataException("Nodes not in graph. ");
        }
        
        graphToAddTo.AddDirectedRelation(startNodeName, endNodeName, relationName);
    }

    private static void GetNodesWithProperty(GraphCollection graphCollection, string graphName, List<string> properties)
    {
        var graphToQuery = graphCollection.NameToGraph[graphName];
        
    }
}