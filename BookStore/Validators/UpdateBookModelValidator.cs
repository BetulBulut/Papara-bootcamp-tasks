using BookStore.Application.BookOperations.Command.UpdateBook;
using FluentValidation;

public class UpdateBookModelValidator : AbstractValidator<UpdateBookModel>
{
    public UpdateBookModelValidator()
    {
        RuleFor(x => x.Title).MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.GenreId).GreaterThanOrEqualTo(0).WithMessage("GenreId must be 0 or greater.");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be 0 or greater.");
    }
}