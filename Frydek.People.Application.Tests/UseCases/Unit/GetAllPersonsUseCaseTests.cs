using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Frydek.People.Application.Tests.UseCases.Unit;

public class GetAllPersonsUseCaseTests
{
    private IPersonRepository _personRepository = null!;
    private GetAllPersonsUseCase _useCase = null!;

    [SetUp]
    public void SetUp()
    {
        _personRepository = Substitute.For<IPersonRepository>();
        _useCase = new GetAllPersonsUseCase(_personRepository);
    }

    [Test]
    public async Task ExecuteAsync_WhenRepositoryReturnsPeople_ReturnsMappedDtosInSameOrder()
    {
        var alice = new Person("Alice", "Doe", "alice@test.com", 30);
        var bob = new Person("Bob", "Smith", "bob@test.com", 25);
        _personRepository.GetAllAsync().Returns([alice, bob]);

        var result = (await _useCase.ExecuteAsync()).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Id, Is.EqualTo(alice.Id));
            Assert.That(result[0].FirstName, Is.EqualTo(alice.FirstName));
            Assert.That(result[0].LastName, Is.EqualTo(alice.LastName));
            Assert.That(result[0].Email, Is.EqualTo(alice.Email));
            Assert.That(result[0].Age, Is.EqualTo(alice.Age));

            Assert.That(result[1].Id, Is.EqualTo(bob.Id));
            Assert.That(result[1].FirstName, Is.EqualTo(bob.FirstName));
            Assert.That(result[1].LastName, Is.EqualTo(bob.LastName));
            Assert.That(result[1].Email, Is.EqualTo(bob.Email));
            Assert.That(result[1].Age, Is.EqualTo(bob.Age));
        });
    }

    [Test]
    public async Task ExecuteAsync_WhenRepositoryReturnsEmpty_ReturnsEmptyCollection()
    {
        _personRepository.GetAllAsync().Returns([]);

        var result = await _useCase.ExecuteAsync();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_DelegatesToRepositoryOnce()
    {
        _personRepository.GetAllAsync().Returns([]);

        await _useCase.ExecuteAsync();

        await _personRepository.Received(1).GetAllAsync();
    }

    [Test]
    public void ExecuteAsync_WhenRepositoryThrows_PropagatesException()
    {
        _personRepository.GetAllAsync().ThrowsAsync(new InvalidOperationException("db down"));

        Assert.That(
            async () => await _useCase.ExecuteAsync(),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("db down"));
    }
}
