using FluentValidation;
using ModelUse.Schema;


namespace ModelUse.Implementation.Validation;

public class BookRequestValidator : AbstractValidator<BookRequest>
{
    public BookRequestValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Title must be between 2 and 50 characters.");
        RuleFor(x => x.Author)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Author must be between 2 and 50 characters.");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}