namespace Frydek.People.Application.Dtos;

public record CreatePersonDto(
    string FirstName,
    string LastName,
    string Email,
    int Age
);
