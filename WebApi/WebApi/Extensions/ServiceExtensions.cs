using Microsoft.EntityFrameworkCore;
using RestfulApiProject.Data;
using RestfulApiProject.Repositories;
using RestfulApiProject.Services;
using WebApi.Services;

namespace WebApi.Extensions;
public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        services.AddScoped<IAuthService, AuthService>();
        services.AddLogging();
    }
}
