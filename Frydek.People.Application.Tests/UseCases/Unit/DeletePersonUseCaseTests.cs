using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Entities;
using Frydek.People.Core.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Frydek.People.Application.Tests.UseCases.Unit;

public class DeletePersonUseCaseTests
{
    private IPersonRepository _personRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private DeletePersonUseCase _useCase = null!;

    [SetUp]
    public void SetUp()
    {
        _personRepository = Substitute.For<IPersonRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _useCase = new DeletePersonUseCase(_personRepository, _unitOfWork);
    }

    [Test]
    public async Task ExecuteAsync_WhenPersonExists_DeletesAndCommits()
    {
        var person = new Person("Alice", "Doe", "alice@test.com", 30);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        await _useCase.ExecuteAsync(person.Id);

        await _personRepository.Received(1).DeleteAsync(person);
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Test]
    public void ExecuteAsync_WhenPersonNotFound_ThrowsAndDoesNotTouchDeleteOrCommit()
    {
        var id = Guid.NewGuid();
        _personRepository.GetByIdAsync(id).Returns((Person?)null);

        Assert.That(
            async () => await _useCase.ExecuteAsync(id),
            Throws.TypeOf<NotFoundException>().With.Message.Contains(id.ToString()));

        _ = _personRepository.DidNotReceive().DeleteAsync(Arg.Any<Person>());
        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Test]
    public async Task ExecuteAsync_DelegatesGetThenDeleteThenCommit()
    {
        var person = new Person("Alice", "Doe", "alice@test.com", 30);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        await _useCase.ExecuteAsync(person.Id);

        Received.InOrder(async () =>
        {
            await _personRepository.GetByIdAsync(person.Id);
            await _personRepository.DeleteAsync(person);
            await _unitOfWork.CommitAsync();
        });
    }

    [Test]
    public void ExecuteAsync_WhenDeleteThrows_PropagatesAndDoesNotCommit()
    {
        var person = new Person("Alice", "Doe", "alice@test.com", 30);
        _personRepository.GetByIdAsync(person.Id).Returns(person);
        _personRepository.DeleteAsync(person).ThrowsAsync(new InvalidOperationException("db down"));

        Assert.That(
            async () => await _useCase.ExecuteAsync(person.Id),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("db down"));

        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }
}
