using FluentValidation;
using WarehouseManager.Core.DTOs;

namespace WarehouseManager.Services.Validators;

public class UserValidator : AbstractValidator<CreateUserDto>
{
    public UserValidator()
    {
        RuleFor(f => f.UserName).NotEmpty().MinimumLength(1).MaximumLength(20)
            .WithMessage("UserName Must be filled.");
        RuleFor(f => f.Email).NotEmpty().MinimumLength(1).MaximumLength(20)
            .WithMessage("Email Must be filled.");
        RuleFor(f => f.Password).NotEmpty().MinimumLength(1).MaximumLength(20)
            .WithMessage("Password Must be filled.");
    }
}