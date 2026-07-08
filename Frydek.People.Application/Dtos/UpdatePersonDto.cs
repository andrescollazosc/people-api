namespace Frydek.People.Application.Dtos;

public record UpdatePersonDto(
    string FirstName,
    string LastName,
    string Email,
    int Age
);
