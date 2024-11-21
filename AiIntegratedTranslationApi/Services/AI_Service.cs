using AiIntegratedTranslationApi.Models;
using ChatGPT.Net;
using Newtonsoft.Json;
using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;

namespace AiIntegratedTranslationApi.Services;


public static class AI_Service
{
    private static string GenerateTranslationPrompt(string fromLanguage, string toLanguage, string target)
    {
        string prompt =
            $"""Translate the target "{target}" from {fromLanguage} to {toLanguage}. """ +
            $"Generate an example sentence both in {fromLanguage} and in {toLanguage} for the target. " +
            //$"Generate the definition of the words in both {fromLanguage} and {toLanguage} for the target. " +
            $"Format: reply with a json object " +
            $"target: target " +
            $"targetTranslated: translated target " +
            $"exSentence: exmaple sentence in {fromLanguage} using target " +
            $"exSentenceTranslated: example sentence in {toLanguage} using the translated target word ";
        //$"definition: definition in {fromLanguage} for target " +
        //$"definitionTranslated: definition in the {toLanguage} language for target ";

        string JsonSchema = JsonConvert.SerializeObject(new AI_TranslationResponse_Json(), Formatting.Indented);

        return prompt + " - Schema: " + JsonSchema;
    }

    public async static Task<AI_TranslationResponse_Json> TranslateWithGptAsync(string fromLanguage, string toLanguage, string target, string apiKey)
    {
        ChatGpt AI = new(apiKey);

        string prompt = GenerateTranslationPrompt(fromLanguage, toLanguage, target);

        string result = await AI.Ask(prompt);

        return JsonConvert.DeserializeObject<AI_TranslationResponse_Json>(result);
    }

    public async static Task<AI_TranslationResponse_Json> TranslateWithOllamaAsync(string fromLanguage, string toLanguage, string target, string modelName)
    {
        string prompt = GenerateTranslationPrompt(fromLanguage, toLanguage, target);

        // set up the client
        var uri = new Uri("http://localhost:11434");
        var ollama = new OllamaApiClient(uri);

        // Get list of available models
        var modelList = await ollama.ListLocalModels();

        // Select Model
        // select a model which should be used for further operations
        ollama.SelectedModel = modelName;

        if (modelList.ToList().Any(model => model.Name == modelName) is not true)
        {
            throw new Exception("Model does not exist.");
        }

        // Output stream
        var result = string.Empty;
        ChatRequest request = new()
        {
            Model = modelName,
            Format = "json",
            Messages = [
                new Message() {
                    Role = ChatRole.System,
                    Content = "You are a translation API that outputs json. Schema:" + JsonConvert.SerializeObject(new AI_TranslationResponse_Json(), Formatting.Indented)
                },
                new Message() {
                    Role = ChatRole.User,
                    Content = prompt,
                }
            ],
        };
        await foreach (ChatResponseStream? stream in ollama.Chat(request))
        {
            result += stream?.Message.Content;
        }

        return JsonConvert.DeserializeObject<AI_TranslationResponse_Json>(result);
    }

    public async static Task<List<string>> GetOllamaModelListAsync()
    {
        // set up the client
        Uri uri = new("http://localhost:11434");
        OllamaApiClient ollama = new(uri);

        // Get list of available models
        var modelList = await ollama.ListLocalModels();

        return modelList.Select(model => model.Name).ToList();
    }
}
