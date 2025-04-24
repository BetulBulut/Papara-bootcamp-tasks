using BookStore.Application.AuthorOperations.Command.CreateAuthor;
using FluentValidation;

public class CreateAuthorModelValidator : AbstractValidator<CreateAuthorModel>
{
    public CreateAuthorModelValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                     .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                     .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                    .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Birth date is required.")
                                 .LessThan(DateTime.Now.Date).WithMessage("Birth date cannot be in the future.");
    }
}