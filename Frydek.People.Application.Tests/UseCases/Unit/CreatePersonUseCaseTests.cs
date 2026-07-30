using FluentValidation;
using Frydek.People.Application.Dtos;
using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Frydek.People.Application.Tests.UseCases.Unit;

public class CreatePersonUseCaseTests
{
    private IPersonRepository _personRepository = null!;
    private IUnitOfWork _unitOfWork = null!;

    [SetUp]
    public void SetUp()
    {
        _personRepository = Substitute.For<IPersonRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
    }

    private CreatePersonUseCase UseCaseWith(IValidator<CreatePersonDto> validator) =>
        new(validator, _personRepository, _unitOfWork);

    private static CreatePersonDto ValidDto() => new()
    {
        FirstName = "Alice",
        LastName = "Doe",
        Email = "alice@test.com",
        Age = 30
    };

    private sealed class PassValidator : AbstractValidator<CreatePersonDto> { }

    private sealed class FailValidator : AbstractValidator<CreatePersonDto>
    {
        public FailValidator() => RuleFor(x => x.FirstName).Must(_ => false).WithMessage("required");
    }

    [Test]
    public async Task ExecuteAsync_WhenValid_CreatesPersonCommitsAndReturnsMappedDto()
    {
        var dto = ValidDto();
        var useCase = UseCaseWith(new PassValidator());

        var result = await useCase.ExecuteAsync(dto);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.FirstName, Is.EqualTo(dto.FirstName));
            Assert.That(result.LastName, Is.EqualTo(dto.LastName));
            Assert.That(result.Email, Is.EqualTo(dto.Email));
            Assert.That(result.Age, Is.EqualTo(dto.Age));
        });
        await _personRepository.Received(1).CreateAsync(Arg.Any<Person>());
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Test]
    public void ExecuteAsync_WhenValidationFails_ThrowsAndDoesNotTouchRepositoryOrUnitOfWork()
    {
        var dto = ValidDto();
        var useCase = UseCaseWith(new FailValidator());

        Assert.That(
            async () => await useCase.ExecuteAsync(dto),
            Throws.TypeOf<ValidationException>());

        _ = _personRepository.DidNotReceive().CreateAsync(Arg.Any<Person>());
        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Test]
    public async Task ExecuteAsync_DelegatesCreateBeforeCommitWithMappedEntity()
    {
        var dto = ValidDto();
        var useCase = UseCaseWith(new PassValidator());

        await useCase.ExecuteAsync(dto);

        Received.InOrder(async () =>
        {
            await _personRepository.CreateAsync(Arg.Is<Person>(p =>
                p != null &&
                p.FirstName == dto.FirstName &&
                p.LastName == dto.LastName &&
                p.Email == dto.Email &&
                p.Age == dto.Age));
            await _unitOfWork.CommitAsync();
        });
    }

    [Test]
    public void ExecuteAsync_WhenRepositoryThrows_PropagatesAndDoesNotCommit()
    {
        var dto = ValidDto();
        var useCase = UseCaseWith(new PassValidator());
        _personRepository.CreateAsync(Arg.Any<Person>())
            .ThrowsAsync(new InvalidOperationException("db down"));

        Assert.That(
            async () => await useCase.ExecuteAsync(dto),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("db down"));

        _ = _unitOfWork.DidNotReceive().CommitAsync();
    }
}
