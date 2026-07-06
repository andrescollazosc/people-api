using Frydek.People.Application.Repositories;
using Frydek.People.Application.UseCases;
using Frydek.People.Application.UseCases.Impl;
using Frydek.People.Infrastructure.Repositories;

namespace Frydek.People.App.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    private static IConfiguration Configuration { get; set; } = null!;
    private static IServiceCollection Services { get; set; } = null!;

    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
        
        RegisterRepositories();
        RegisterUseCases();
    }

    private static void RegisterRepositories()
    {
        Services.AddScoped<IPersonRepository, PersonRepository>();
    }
    
    private static void RegisterUseCases()
    {
        Services.AddScoped<IGetPersonUseCase, GetPersonUseCase>();
        Services.AddScoped<ICreatePersonUseCase, CreatePersonUseCase>();
        Services.AddScoped<IGetAllPersonsUseCase, GetAllPersonsUseCase>();
    }

}