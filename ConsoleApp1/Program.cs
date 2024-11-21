using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Reflection;

namespace ConsoleApp1;

internal class Program
{
    async static Task Main(string[] args)
    {
        // set up the client
        var uri = new Uri("http://localhost:11434");
        var ollama = new OllamaApiClient(uri);

        // Get list of available models
        var modelList = await ollama.ListLocalModels();

        // List models
        foreach (Model model in modelList)
        {
            Console.WriteLine(model.Name);
        }

        // Prompt for model selection
        Console.WriteLine();
        Console.WriteLine("Please select one of the above models (type exactly) \n");
        string selectedModel = Console.ReadLine() ?? string.Empty;

        // Select Model
        // select a model which should be used for further operations
        ollama.SelectedModel = selectedModel;

        // Prompt model
        Console.WriteLine();
        Console.WriteLine("Please provide the prompt to be asked of the model \n");
        string prompt = Console.ReadLine() ?? string.Empty;
        // Output stream
        Console.WriteLine();
        await foreach (GenerateResponseStream? stream in ollama.Generate(prompt))
        {
            Console.Write(stream?.Response);
        }
    }
}
