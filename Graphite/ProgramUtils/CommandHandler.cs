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
            {
                if (commandWords.Length < 2)
                {
                    Console.WriteLine(
                        "Invalid command length. Should have length 2 and be of the form: CREATE graph1,graph2,...");
                    return;
                }

                var graphNamesToAdd = commandWords[1].Split(",");

                foreach (var graphNameToAdd in graphNamesToAdd)
                {
                    CreateGraphInCollection(graphNameToAdd, graphCollection);
                }

                break;
            }
            case "ADD":
            {
                if (command.Length < 4)
                {
                    Console.WriteLine(
                        "Invalid command length. Should have length 4 and be of the following form: ADD NODES node1,node2,... TO graph");
                    return;
                }

                var nodeNameToAdd = commandWords[1];
                var graphNameToAddTo = commandWords[3];

                AddNodeToGraph(graphCollection, graphNameToAddTo, nodeNameToAdd);

                break;
            }
            case "GIVE":
            {
                var listOfNodes = commandWords[1].Split(",").ToList();
                var listOfProps = commandWords[3].Split(",").ToList();
                var graphNameToGivePropsTo = commandWords[5];

                var graphToGivePropsTo = graphCollection.NameToGraph[graphNameToGivePropsTo];

                graphToGivePropsTo.AddPropertiesToNodes(listOfNodes, listOfProps);

                break;
            }
            case "RELATE":
            {
                if (command.Length < 8)
                {
                    Console.WriteLine("Invalid command. Should have length 8 and be of the following form: " +
                                      "RELATE node1 TO node2 WITH relationship IN graph");

                    return;
                }

                var startNode = commandWords[1];
                var endNode = commandWords[3];
                var relationName = commandWords[5];
                var graphName = commandWords[7];

                AddRelationToGraph(graphCollection, graphName, startNode, endNode, relationName);
                
                break;
            }
            case "REMOVE":
            {
                if (commandWords.Length < 6)
                {
                    Console.WriteLine("Invalid command. Should start with REMOVE NODES or REMOVE PROPS");
                }

                if (!graphCollection.NameToGraph.ContainsKey(commandWords[6]))
                {
                    Console.WriteLine(commandWords[6] + " is not a valid graph.");
                }

                var graphToRemoveNodesFrom = graphCollection.NameToGraph[commandWords[6]];

                switch (commandWords[3])
                {
                    case "NAMES":
                    {
                        var listOfNodes = commandWords[4].Split(",").ToList();
                        graphToRemoveNodesFrom.RemoveNodesWithNames(listOfNodes);

                        break;
                    }

                    case "PROPS":
                    {
                        var listOfProps = commandWords[4].Split(",").ToList();
                        graphToRemoveNodesFrom.RemoveNodesWithProperties(listOfProps);
                        
                        break;
                    }
                }

                break;
            }
            case "GET":
            {
                var properties = commandWords[4].Split(",").ToList();
                var graphNameToGetFrom = commandWords[6];

                GetNodesWithPropertiesAndRelations(graphCollection, graphNameToGetFrom, properties);

                break;
            }
            case "LIST":
            {
                if (commandWords.Length < 3)
                {
                    Console.WriteLine("Invalid command. Should have the form: LIST NODES IN <graphname>");
                }
                
                var graphNameToGetFrom = graphCollection.NameToGraph[commandWords[3]];

                var listOfNodes = graphNameToGetFrom.NodesInGraph;

                foreach (var node in listOfNodes)
                {
                    Console.WriteLine(node.NodeName);
                }

                break;
            }
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