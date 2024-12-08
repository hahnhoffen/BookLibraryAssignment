using Microsoft.Extensions.DependencyInjection;
using MediatR;
using BookLibrary.Application.Books.Commands.AddBook;

namespace BookLibrary.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBookCommand).Assembly));

            return services;
        }
    }
}
