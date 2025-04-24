using BookStore.Application.AuthorOperations.Command;
using FluentValidation;

public class UpdateAuthorModelValidator : AbstractValidator<UpdateAuthorModel>
{
    public UpdateAuthorModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name must not be empty.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name must not be empty.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }
}