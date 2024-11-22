using BookLibrary.Infrastructure.Repositories;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.DataBase;
using Microsoft.Extensions.DependencyInjection;

namespace BookLibrary.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register FakeDatabase as a singleton
            services.AddSingleton<FakeDatabase>();

            // Register repositories for dependency injection
            services.AddSingleton<IBookRepository, FakeBookRepository>();
            services.AddSingleton<IAuthorRepository, FakeAuthorRepository>();

            return services;
        }
    }
}
