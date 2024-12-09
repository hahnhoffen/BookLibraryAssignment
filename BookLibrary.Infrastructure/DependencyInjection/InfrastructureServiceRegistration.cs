using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;
using Microsoft.Extensions.DependencyInjection;
using BookLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace BookLibrary.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool useFakeDatabase = false)
        {
            if (useFakeDatabase)
            {
                services.AddSingleton<FakeDatabase>();
                services.AddSingleton<IBookRepository, FakeBookRepository>();
                services.AddSingleton<IAuthorRepository, FakeAuthorRepository>();
                services.AddSingleton<IUserRepository, FakeUserRepository>();
            }
            else
            {
                services.AddDbContext<RealDataBase>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                services.AddScoped<IBookRepository, RealBookRepository>();
                services.AddScoped<IAuthorRepository, RealAuthorRepository>();
                services.AddScoped<IPasswordService, PasswordService>();
                services.AddScoped<IUserRepository, RealUserRepository>();
            }
            return services;
        }

    }
}

