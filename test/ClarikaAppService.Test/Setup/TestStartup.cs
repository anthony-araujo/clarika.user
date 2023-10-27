using ClarikaAppService.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ClarikaAppService.Infrastructure.Data;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace ClarikaAppService.Test.Setup;

public class TestStartup : Startup
{
    public override void Configure(IConfiguration configuration, IServiceCollection services)
    {
        base.Configure(configuration, services);
    }

    public override void ConfigureServices(IServiceCollection services, IHostEnvironment environment)
    {
        base.ConfigureServices(services, environment);
    }

    public override void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment environment)
    {
        base.ConfigureMiddleware(app, environment);
    }

    public override void ConfigureEndpoints(IApplicationBuilder app, IHostEnvironment environment)
    {
        base.ConfigureEndpoints(app, environment);
    }

    //protected override void AddDatabase(IConfiguration configuration, IServiceCollection services)
    //{
    //    var connection = new SqliteConnection(new SqliteConnectionStringBuilder
    //    {
    //        DataSource = ":memory:"
    //    }.ToString());

    //    services.AddDbContext<ApplicationDatabaseContext>(context => context.UseSqlite(connection));
    //    services.AddScoped<DbContext>(provider => provider.GetService<ApplicationDatabaseContext>());
    //}

    protected override void AddDatabase(IConfiguration configuration, IServiceCollection services)
    {
        //services.AddDbContext<ApplicationDatabaseContext>(options =>
        //{
        //    options.UseInMemoryDatabase(databaseName: "YourInMemoryDatabaseName");
        //});

        //services.AddScoped<DbContext>(provider => provider.GetService<ApplicationDatabaseContext>());

        string connectionString = configuration.GetConnectionString("AppDbContext");
        services.AddScoped<IDbConnection>((provider) => new SqlConnection(connectionString));

    }
}
