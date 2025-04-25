using FluentValidation;
using MovieStore.Schema;

namespace MovieStore.Implementation.Validation;

public class ActorValidator : AbstractValidator<ActorRequest>
{
    public ActorValidator()
    {
        RuleFor(actor => actor.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(actor => actor.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }
}