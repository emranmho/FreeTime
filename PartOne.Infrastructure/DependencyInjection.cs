using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartOne.Application.Common.Interfaces;
using PartOne.Application.Services.Implementation;
using PartOne.Application.Services.Interfaces;
using PartOne.Infrastructure.Data;
using PartOne.Infrastructure.Repository;

namespace PartOne.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MyDatabaseConnectionString");
        
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(connectionString)
        );
        
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IShortenedUrlService, ShortenedUrlService>();
        
        return services;
    }
}