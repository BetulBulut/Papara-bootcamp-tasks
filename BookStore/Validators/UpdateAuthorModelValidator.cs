using BookStore.Application.AuthorOperations.Command;
using FluentValidation;

public class UpdateAuthorModelValidator : AbstractValidator<UpdateAuthorModel>
{
    public UpdateAuthorModelValidator()
    {
        RuleFor(x => x.FirstName).MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        RuleFor(x => x.LastName).MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }
}