namespace AiAssistantApi.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GetGeminiResponseAsync(string prompt);
    }
}