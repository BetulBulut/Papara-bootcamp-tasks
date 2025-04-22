using FluentValidation;
using BookStore.Application.GenreOperations.Command;

public class UpdateGenreModelValidator : AbstractValidator<UpdateGenreModel>
{
    public UpdateGenreModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Genre name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name)); // Sadece Name doluysa kontrol et

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Genre description cannot exceed 250 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description)); // Sadece Description doluysa kontrol et
    }
}