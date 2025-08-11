using Microsoft.AspNetCore.Mvc;
using AiAssistantApi.Services.Interfaces;
using AiAssistantApi.Models.Dtos;
using System.Security.Claims;

namespace AiAssistantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly IPromptMemoryService _promptMemoryService;
        public GeminiController(IGeminiService geminiService, IPromptMemoryService promptMemoryService)
        {
            _geminiService = geminiService;
            _promptMemoryService = promptMemoryService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> GetReply([FromBody] GeminiRequest request){
            if (request == null || string.IsNullOrEmpty(request.Prompt)){
                return BadRequest("Prompt is required");
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if(string.IsNullOrEmpty(userId) && Request.Headers.TryGetValue("X-Anonymous-User-Id", out var anonymousUserId)){
                userId = anonymousUserId.ToString();
            }

            if(string.IsNullOrEmpty(userId)){
                return Unauthorized("User not found");
            }

            _promptMemoryService.AddPrompt(userId, request.Prompt);

            var promptHistory = _promptMemoryService.GetPromptHistory(userId);

            var context = string.Join("\n", promptHistory.TakeLast(7));

            var response = await _geminiService.GetGeminiResponseAsync(context);

            return Ok(new GeminiResponse{Reply = response});
        }
    }
}