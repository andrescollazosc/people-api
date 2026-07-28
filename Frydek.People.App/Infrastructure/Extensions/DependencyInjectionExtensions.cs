using FluentValidation;
using Frydek.People.App.Infrastructure.ExceptionHandlers;
using Frydek.People.Application.UseCases;
using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Application.Validations;
using Frydek.People.Core.Abstractions;
using Frydek.People.Infrastructure;
using Frydek.People.Infrastructure.Data;
using Frydek.People.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Frydek.People.App.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .RegisterRepositories()
            .RegisterUseCases()
            .RegisterValidators()
            .RegisterExceptionHandlers()
            .RegisterDatabases(configuration);
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

    private static IServiceCollection RegisterDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NpgsqlDataSource>(_ =>
        {
            var connectionString = configuration.GetConnectionString("DB_POSTGRES_PEOPLE")
                ?? throw new InvalidOperationException(
                    "Connection string 'DB_POSTGRES_PEOPLE' is not configured.");

            return new NpgsqlDataSourceBuilder(connectionString)
                .EnableDynamicJson()
                .Build();
        });

        services.AddDbContext<PeopleDbContext>((sp, options) =>
            options.UseNpgsql(sp.GetRequiredService<NpgsqlDataSource>()));

        services.AddScoped<IUnitOfWork, EfUnitOfWork<PeopleDbContext>>();

        return services;
    }
}
