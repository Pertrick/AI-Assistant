namespace AiAssistantApi.Services.Interfaces
{
    public interface IPromptMemoryService
    {
        void AddPrompt(string userId, string prompt);
        List<string> GetPromptHistory(string userId);
    }
}