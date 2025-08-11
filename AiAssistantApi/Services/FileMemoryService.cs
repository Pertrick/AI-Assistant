using Newtonsoft.Json;
using AiAssistantApi.Services.Interfaces;

namespace AiAssistantApi.Services;

public class FileMemoryService : IPromptMemoryService
{
    private string GetFilePath(string userId)
    {
        return Path.Combine("PromptHistory", $"{userId}.json");
    }

    public void AddPrompt(string userId, string prompt)
    {
        var promptHistory = GetPromptHistory(userId);
        promptHistory.Add(prompt);

        var filePath = GetFilePath(userId);
        SaveToFile(filePath, JsonConvert.SerializeObject(promptHistory, Formatting.Indented));
    }

    public List<string> GetPromptHistory(string userId)
    {
        var path = GetFilePath(userId);

        if (!File.Exists(path))
            return new List<string>();

        var content = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<string>>(content) ?? new List<string>();
    }

    public void SaveToFile(string filePath, string content)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, content);
    }
}
