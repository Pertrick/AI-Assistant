using AiAssistantApi.Models.Dtos;
using FluentValidation;

namespace AiAssistantApi.Validators
{
    public class GeminiValidator : AbstractValidator<GeminiRequest>
    {
        public GeminiValidator()
        {
            RuleFor(x => x.Prompt).NotEmpty().WithMessage("Prompt is required");    
            RuleFor(x => x.Prompt).MaximumLength(1000).WithMessage("Prompt must be less than 1000 characters");
        }
    }
}