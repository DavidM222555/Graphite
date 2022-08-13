using Graphite.GraphCollectionUtils;

namespace Graphite.ProgramUtils;

public static class ProgramRunner
{
    public static void Run()
    {
        var graphCollection = new GraphCollection();
        
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.WriteLine("Welcome to Graphite!");
        Console.WriteLine();

        // Program loop
        while (true)
        {
            var command = Console.ReadLine();
            
            CommandHandler.HandleCommand(command, graphCollection);
        }
    }
}