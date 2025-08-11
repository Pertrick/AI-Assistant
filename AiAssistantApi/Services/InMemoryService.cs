using AiAssistantApi.Services.Interfaces;
using System.Collections.Concurrent;

namespace AiAssistantApi.Services
{
    public class InMemoryService : IPromptMemoryService
    {
        private readonly ConcurrentDictionary<string, List<string>> _userPrompts = new ();

        public void AddPrompt(string userId, string prompt)
        {
            if (!_userPrompts.TryGetValue(userId, out var history))
            {
                history = _userPrompts.GetOrAdd(userId, _ => new List<string>());
            }
            history.Add(prompt);
        }

        public List<string> GetPromptHistory(string userId)
        {
            if (_userPrompts.TryGetValue(userId, out var history))
            {
                return history;
            }
            return new List<string>();
        }
    }
}