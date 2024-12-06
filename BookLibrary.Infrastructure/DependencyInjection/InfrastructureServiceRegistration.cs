using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;
using Microsoft.Extensions.DependencyInjection;
using BookLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, bool useFakeDatabase = false)
        {
            if (useFakeDatabase)
            {
                // Register FakeDatabase and repositories for testing
                services.AddSingleton<FakeDatabase>();
                services.AddSingleton<IBookRepository, FakeBookRepository>();
                services.AddSingleton<IAuthorRepository, FakeAuthorRepository>();
            }
            else
            {
                // Register RealDatabase and repositories for production/development
                services.AddDbContext<RealDataBase>(options =>
                    options.UseSqlServer("DefaultConnection"));
                services.AddScoped<IBookRepository, RealBookRepository>();
                services.AddScoped<IAuthorRepository, RealAuthorRepository>();
            }

            return services;
        }

    }
}

