namespace Frydek.People.Application.Dtos;

public record PersonBaseDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public int Age { get; init; }
}
