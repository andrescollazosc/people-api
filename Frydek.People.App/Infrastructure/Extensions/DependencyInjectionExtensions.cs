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
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterDependencies(IConfiguration configuration)
        {
            return services
                .RegisterRepositories()
                .RegisterUseCases()
                .RegisterValidators()
                .RegisterExceptionHandlers()
                .RegisterDatabases(configuration);
        }

        private IServiceCollection RegisterRepositories()
        {
            services.AddScoped<IPersonRepository, PersonRepository>();

            return services;
        }

        private IServiceCollection RegisterUseCases()
        {
            services.AddScoped<IGetPersonUseCase, GetPersonUseCase>();
            services.AddScoped<ICreatePersonUseCase, CreatePersonUseCase>();
            services.AddScoped<IGetAllPersonsUseCase, GetAllPersonsUseCase>();
            services.AddScoped<IUpdatePersonUseCase, UpdatePersonUseCase>();
            services.AddScoped<IDeletePersonUseCase, DeletePersonUseCase>();
        
            return services;
        }

        private IServiceCollection RegisterValidators()
        {
            services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();

            return services;
        }

        private IServiceCollection RegisterExceptionHandlers()
        {
            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        private IServiceCollection RegisterDatabases(IConfiguration configuration)
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
}
