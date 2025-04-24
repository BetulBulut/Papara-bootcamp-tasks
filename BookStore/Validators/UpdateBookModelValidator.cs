using BookStore.Application.BookOperations.Command.UpdateBook;
using FluentValidation;

public class UpdateBookModelValidator : AbstractValidator<UpdateBookModel>
{
    public UpdateBookModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title must not be empty.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.GenreId).GreaterThan(0).WithMessage("GenreId must be greater than 0.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("AuthorId must be greater than 0.");
    }
}