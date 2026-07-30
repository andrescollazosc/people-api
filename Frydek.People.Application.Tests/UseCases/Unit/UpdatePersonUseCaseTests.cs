using FluentValidation;
using Frydek.People.Application.Dtos;
using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Entities;
using Frydek.People.Core.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Frydek.People.Application.Tests.UseCases.Unit;

public class UpdatePersonUseCaseTests
{
    private IPersonRepository _personRepository = null!;
    private IUnitOfWork _unitOfWork = null!;

    [SetUp]
    public void SetUp()
    {
        _personRepository = Substitute.For<IPersonRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
    }

    private UpdatePersonUseCase UseCaseWith(IValidator<UpdatePersonDto> validator) =>
        new(validator, _personRepository, _unitOfWork);

    private static UpdatePersonDto ValidDto() => new()
    {
        FirstName = "Alice",
        LastName = "Doe",
        Email = "alice@test.com",
        Age = 30
    };

    private sealed class PassValidator : AbstractValidator<UpdatePersonDto> { }

    private sealed class FailValidator : AbstractValidator<UpdatePersonDto>
    {
        public FailValidator() => RuleFor(x => x.FirstName).Must(_ => false).WithMessage("required");
    }

    [Test]
    public async Task ExecuteAsync_WhenValidAndPersonExists_UpdatesCommitsAndReturnsMappedDto()
    {
        var existing = new Person("Old", "Name", "old@test.com", 20);
        _personRepository.GetByIdAsync(existing.Id).Returns(existing);
        var dto = ValidDto();
        var useCase = UseCaseWith(new PassValidator());

        var result = await useCase.ExecuteAsync(existing.Id, dto);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(existing.Id));
            Assert.That(result.FirstName, Is.EqualTo(dto.FirstName));
            Assert.That(result.LastName, Is.EqualTo(dto.LastName));
            Assert.That(result.Email, Is.EqualTo(dto.Email));
            Assert.That(result.Age, Is.EqualTo(dto.Age));
            Assert.That(existing.FirstName, Is.EqualTo(dto.FirstName));
            Assert.That(existing.LastName, Is.EqualTo(dto.LastName));
            Assert.That(existing.Email, Is.EqualTo(dto.Email));
            Assert.That(existing.Age, Is.EqualTo(dto.Age));
        });
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Test]
    public void ExecuteAsync_WhenValidationFails_ThrowsAndDoesNotTouchRepositoryOrUnitOfWork()
    {
        var useCase = UseCaseWith(new FailValidator());

        Assert.That(
            async () => await useCase.ExecuteAsync(Guid.NewGuid(), ValidDto()),
            Throws.TypeOf<ValidationException>());

        _ = _personRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>());
        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Test]
    public void ExecuteAsync_WhenPersonNotFound_ThrowsAndDoesNotCommit()
    {
        var id = Guid.NewGuid();
        _personRepository.GetByIdAsync(id).Returns((Person?)null);
        var useCase = UseCaseWith(new PassValidator());

        Assert.That(
            async () => await useCase.ExecuteAsync(id, ValidDto()),
            Throws.TypeOf<NotFoundException>().With.Message.Contains(id.ToString()));

        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Test]
    public async Task ExecuteAsync_DelegatesGetBeforeCommit()
    {
        var existing = new Person("Old", "Name", "old@test.com", 20);
        _personRepository.GetByIdAsync(existing.Id).Returns(existing);
        var useCase = UseCaseWith(new PassValidator());

        await useCase.ExecuteAsync(existing.Id, ValidDto());

        Received.InOrder(async () =>
        {
            await _personRepository.GetByIdAsync(existing.Id);
            await _unitOfWork.CommitAsync();
        });
    }

    [Test]
    public void ExecuteAsync_WhenRepositoryThrows_PropagatesAndDoesNotCommit()
    {
        var id = Guid.NewGuid();
        _personRepository.GetByIdAsync(id).ThrowsAsync(new InvalidOperationException("db down"));
        var useCase = UseCaseWith(new PassValidator());

        Assert.That(
            async () => await useCase.ExecuteAsync(id, ValidDto()),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("db down"));

        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }
}
