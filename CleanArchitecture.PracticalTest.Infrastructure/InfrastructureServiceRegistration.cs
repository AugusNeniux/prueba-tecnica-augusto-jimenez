using CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;
using CleanArchitecture.PracticalTest.Application.Contracts.Data;
using CleanArchitecture.PracticalTest.Infrastructure.Data;
using CleanArchitecture.PracticalTest.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArchitecture.PracticalTest.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContextDb>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ContextDb")!)
                .UseSnakeCaseNamingConvention() // Convierte nombres de tablas y columnas a snake_case
                .EnableSensitiveDataLogging() // Para ver los valores de las consultas en la consola
        );

        // Repositorios, UnitOfWork y QueryStores
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();



        return services;
    }
}