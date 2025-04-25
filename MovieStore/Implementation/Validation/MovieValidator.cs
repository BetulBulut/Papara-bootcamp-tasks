using FluentValidation;
using MovieStore.Schema;

namespace MovieStore.Implementation.Validation;

public class MovieValidator : AbstractValidator<MovieRequest>
{
    public MovieValidator()
    {
        RuleFor(movie => movie.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(movie => movie.ReleaseYear)
            .InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"Release year must be between 1900 and {DateTime.Now.Year}.");

        RuleFor(movie => movie.Genre)
            .IsInEnum().WithMessage("Invalid genre.");

        RuleFor(movie => movie.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(movie => movie.DirectorId)
            .GreaterThan(0).WithMessage("DirectorId must be a valid ID.");

        RuleFor(movie => movie.ActorIds)
            .NotNull().WithMessage("ActorIds cannot be null.")
            .Must(ids => ids.Count > 0).WithMessage("At least one actor must be specified.");
    }
}