using FluentValidation;
using WarehouseManager.Core.DTOs;

namespace WarehouseManager.Services.Validators;

public class WarehouseValidator : AbstractValidator<CreateWarehouseDto>
{
    public WarehouseValidator()
    {
        RuleFor(f => f.Name).NotEmpty().MinimumLength(1).MaximumLength(20)
            .WithMessage("Name must be filled.");
    }
}