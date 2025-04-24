using BookStore.Application.BookOperations.Command.CreateBook;
using FluentValidation;

public class CreateBookModelValidator : AbstractValidator<CreateBookModel>
{
    public CreateBookModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.")
                             .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.GenreId).GreaterThan(0).WithMessage("GenreId must be greater than 0.");
        RuleFor(x => x.PublishedDate).NotEmpty().WithMessage("Published Date is required.")
                         .LessThan(DateTime.Now.Date).WithMessage("Published Date cannot be today or in the future.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("AuthorId must be greater than 0.");
        RuleFor(x => x.ISBN).NotEmpty().WithMessage("ISBN is required.")
                            .MaximumLength(50).WithMessage("ISBN cannot exceed 50 characters.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}