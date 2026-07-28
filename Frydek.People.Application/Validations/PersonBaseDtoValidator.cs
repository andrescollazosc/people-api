using FluentValidation;
using Frydek.People.Application.Dtos;

namespace Frydek.People.Application.Validations;

public class PersonBaseDtoValidator<T> : AbstractValidator<T> where T : PersonBaseDto
{
    public PersonBaseDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 150);
    }
}
