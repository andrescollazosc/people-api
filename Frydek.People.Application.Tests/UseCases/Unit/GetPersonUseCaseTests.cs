using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Entities;
using Frydek.People.Core.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Frydek.People.Application.Tests.UseCases.Unit;

public class GetPersonUseCaseTests
{
    private IPersonRepository _personRepository = null!;
    private GetPersonUseCase _useCase = null!;

    [SetUp]
    public void SetUp()
    {
        _personRepository = Substitute.For<IPersonRepository>();
        _useCase = new GetPersonUseCase(_personRepository);
    }

    [Test]
    public async Task ExecuteAsync_WhenPersonExists_ReturnsMappedDto()
    {
        var person = new Person("Alice", "Doe", "alice@test.com", 30);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        var result = await _useCase.ExecuteAsync(person.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(person.Id));
            Assert.That(result.FirstName, Is.EqualTo(person.FirstName));
            Assert.That(result.LastName, Is.EqualTo(person.LastName));
            Assert.That(result.Email, Is.EqualTo(person.Email));
            Assert.That(result.Age, Is.EqualTo(person.Age));
        });
    }

    [Test]
    public void ExecuteAsync_WhenPersonNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _personRepository.GetByIdAsync(id).Returns((Person?)null);

        Assert.That(
            async () => await _useCase.ExecuteAsync(id),
            Throws.TypeOf<NotFoundException>().With.Message.Contains(id.ToString()));
    }

    [Test]
    public async Task ExecuteAsync_DelegatesToRepositoryWithGivenId()
    {
        var id = Guid.NewGuid();
        _personRepository.GetByIdAsync(id).Returns(new Person("A", "B", "a@b.com", 20));

        await _useCase.ExecuteAsync(id);

        await _personRepository.Received(1).GetByIdAsync(id);
    }

    [Test]
    public void ExecuteAsync_WhenRepositoryThrows_PropagatesException()
    {
        var id = Guid.NewGuid();
        _personRepository.GetByIdAsync(id).ThrowsAsync(new InvalidOperationException("db down"));

        Assert.That(
            async () => await _useCase.ExecuteAsync(id),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("db down"));
    }
}
