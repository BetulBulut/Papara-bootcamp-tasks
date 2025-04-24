using FluentValidation;
using MovieStore.Schema;

namespace MovieStore.Implementation.Validation;

public class CustomerValidator : AbstractValidator<CustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.LastName).MinimumLength(2).MaximumLength(50);  
        RuleFor(x => x.Username).MinimumLength(2).MaximumLength(50)
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Username can only contain letters and numbers.");
        
    }
}