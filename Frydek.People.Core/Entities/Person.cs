namespace Frydek.People.Core.Entities;

public class Person
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public int Age { get; private set; }

    private Person() { }

    public Person(string firstName, string lastName, string email, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Age = age;
    }

    public void Update(string firstName, string lastName, string email, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Age = age;
    }
}
