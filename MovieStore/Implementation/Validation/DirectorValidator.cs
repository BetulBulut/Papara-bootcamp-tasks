using FluentValidation;
using MovieStore.Schema;

namespace MovieStore.Implementation.Validation;

public class DirectorValidator : AbstractValidator<DirectorRequest>
{
    public DirectorValidator()
    {
        RuleFor(Director => Director.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(Director => Director.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }
}