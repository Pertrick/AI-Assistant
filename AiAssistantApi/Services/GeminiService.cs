using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using AiAssistantApi.Services.Interfaces;

namespace AiAssistantApi.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration.GetValue<string>("GeminiApiKey");
        }

        public async Task<string> GetGeminiResponseAsync(string prompt)
        {
            var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + _apiKey;
            var body = new {
                contents = new[] {
                    new {
                        parts = new[] {
                            new {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode){
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(responseBody);
            var reply = jsonDocument.RootElement.GetProperty("candidates").EnumerateArray().FirstOrDefault()
                                                .GetProperty("content")
                                                .GetProperty("parts").EnumerateArray().FirstOrDefault()
                                                .GetProperty("text")
                                                .GetString();
            return reply ?? "No response from Gemini";
        }
    }
}