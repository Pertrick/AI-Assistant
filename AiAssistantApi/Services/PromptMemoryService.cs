using AiAssistantApi.Services.Interfaces;

namespace AiAssistantApi.Services;

public class PromptMemoryService : IPromptMemoryService
{
    private readonly InMemoryService _inMemoryService;
    private readonly FileMemoryService _fileMemoryService;

    public PromptMemoryService(InMemoryService inMemoryService, FileMemoryService fileMemoryService)
    {
        _inMemoryService = inMemoryService;
        _fileMemoryService = fileMemoryService;
    }

    public void AddPrompt(string userId, string prompt)
    {
        if(IsAnonymous(userId))
        {
            _inMemoryService.AddPrompt(userId, prompt);
        }
        else
        {
            _fileMemoryService.AddPrompt(userId, prompt);
        }
    }

    public List<string> GetPromptHistory(string userId)
    {
        if(IsAnonymous(userId))
        {
            return _inMemoryService.GetPromptHistory(userId);
        }
        else
        {
            return _fileMemoryService.GetPromptHistory(userId);
        }
    }


     private bool IsAnonymous(string userId)
    {
        return userId.StartsWith("ANON_", StringComparison.OrdinalIgnoreCase);
    }

    
}