using FluentValidation;
using Frydek.People.App.Infrastructure.ExceptionHandlers;
using Frydek.People.Application.Repositories;
using Frydek.People.Application.UseCases;
using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Application.Validations;
using Frydek.People.Infrastructure.Repositories;

namespace Frydek.People.App.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        return services
            .RegisterRepositories()
            .RegisterUseCases()
            .RegisterValidators()
            .RegisterExceptionHandlers();
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
        return services;
    }

    private static IServiceCollection RegisterUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetPersonUseCase, GetPersonUseCase>();
        services.AddScoped<ICreatePersonUseCase, CreatePersonUseCase>();
        services.AddScoped<IGetAllPersonsUseCase, GetAllPersonsUseCase>();
        services.AddScoped<IUpdatePersonUseCase, UpdatePersonUseCase>();
        services.AddScoped<IDeletePersonUseCase, DeletePersonUseCase>();
        return services;
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();
        return services;
    }

    private static IServiceCollection RegisterExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}
