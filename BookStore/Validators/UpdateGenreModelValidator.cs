using FluentValidation;
using BookStore.Application.GenreOperations.Command;

public class UpdateGenreModelValidator : AbstractValidator<UpdateGenreModel>
{
    public UpdateGenreModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name must not be empty.")
            .MinimumLength(2).WithMessage("Genre name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Genre name cannot exceed 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Genre description must not be empty.")
            .MinimumLength(3).WithMessage("Genre description must be at least 3 characters long.")
            .MaximumLength(250).WithMessage("Genre description cannot exceed 250 characters.");
    }
}