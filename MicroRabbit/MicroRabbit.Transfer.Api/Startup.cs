using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MicroRabbit.Transfer.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Db Context
        var connection = Configuration.GetConnectionString("TransferDbConnection");

        services.AddDbContext<TransferDbContext>(options =>
           options.UseSqlServer(connection, options =>
           {
               options.EnableRetryOnFailure();
               options.CommandTimeout(600);
           }));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer Microservice", Version = "v1" });
        });


        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(assembly);
        RegisterServices(services, Configuration);


    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        else
            app.UseHsts();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseCors("CorsPolicy");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        ConfigureEventBus(app);
    }

    private void ConfigureEventBus(IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        DependencyContainer.RegisterServices(services, configuration);
    }
}
