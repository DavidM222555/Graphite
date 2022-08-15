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

        switch (commandWords[0].ToUpper())
        {
            case "CREATE":
                if (commandWords.Length < 2)
                {
                    throw new InvalidDataException("Invalid command length. Should have length 2.");
                }

                var graphNamesToAdd = commandWords[1].Split(",");

                foreach (var graphNameToAdd in graphNamesToAdd)
                {
                    CreateGraphInCollection(graphNameToAdd, graphCollection);
                    Console.WriteLine("Graph " + graphNameToAdd + " successfully added.");
                }
                
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
            
            case "GIVE":
                var listOfNodes = commandWords[1].Split(",").ToList();
                var listOfProps = commandWords[3].Split(",").ToList();
                var graphNameToGivePropsTo = commandWords[5];

                var graphToGivePropsTo = graphCollection.NameToGraph[graphNameToGivePropsTo];
                
                graphToGivePropsTo.AddPropertiesToNodes(listOfNodes, listOfProps);
                
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
                var properties = commandWords[4].Split(",").ToList();
                var graphNameToGetFrom = commandWords[6];
                
                GetNodesWithPropertiesAndRelations(graphCollection, graphNameToGetFrom, properties);
                
                break;
        }
    }

    private static void CreateGraphInCollection(string graphName, GraphCollection graphCollection)
    {
        // If the graph collection already contains the given name we just return
        if (graphCollection.NameToGraph.ContainsKey(graphName))
        {
            Console.WriteLine(graphName + " is already contained in the graph collection");
            
            return;
        }    
        
        graphCollection.AddGraphToCollectionByName(graphName);
    }

    private static void AddNodeToGraph(GraphCollection graphCollection, string graphName, string nodeNameToAdd)
    {
        if (!graphCollection.NameToGraph.ContainsKey(graphName))
        {
            Console.WriteLine("Unable to add node to graph because " + graphName + " hasn't been created.");
            
            return;
        }

        var graphToAddTo = graphCollection.NameToGraph[graphName];
        
        graphToAddTo.AddNodeFromString(nodeNameToAdd);
    }

    private static void AddRelationToGraph(GraphCollection graphCollection, string graphName, string startNodeName,
        string endNodeName, string relationName)
    {
        if (!graphCollection.NameToGraph.ContainsKey(graphName))
        {
            Console.WriteLine("Cannot add relation to graph because the graph " + graphName + " hasn't been created");
            return;
        }

        var graphToAddTo = graphCollection.NameToGraph[graphName];

        if (!graphToAddTo.StringToNode.ContainsKey(startNodeName))
        {
            Console.WriteLine("Unable to add relation to graph because " + startNodeName + " doesn't exist in the graph.");
            return;
        }
        
        if (!graphToAddTo.StringToNode.ContainsKey(endNodeName))
        {
            Console.WriteLine("Unable to add relation to graph because " + endNodeName + " doesn't exist in the graph.");
            return;
        }
        
        graphToAddTo.AddDirectedRelation(startNodeName, endNodeName, relationName);
    }

    private static void GetNodesWithPropertiesAndRelations(GraphCollection graphCollection, string graphName, List<string> properties)
    {
        if (!graphCollection.NameToGraph.ContainsKey(graphName))
        {
            Console.WriteLine("Unable to query property because " + graphName + " has not been created yet.");
        }
        
        var graphToQuery = graphCollection.NameToGraph[graphName];

        var nodesWithProps = graphToQuery.GetNodesWithProperties(properties);

        foreach (var node in nodesWithProps)
        {
            Console.WriteLine("Node: " + node);
        }
    }
}