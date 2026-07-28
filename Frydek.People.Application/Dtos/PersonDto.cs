namespace Frydek.People.Application.Dtos;

public record PersonDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    int Age
);
