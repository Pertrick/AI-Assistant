using FluentValidation;
using AiAssistantApi.Models.Dtos.Auth;

namespace AiAssistantApi.Validator;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}