namespace MicroRabbit.Banking.Api;


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
        services.AddDbContext<BankingDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("BankingDbConnection")));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        RegisterServices(services);

        var assembly = Assembly.GetExecutingAssembly();

       // Add MediatR
        services.AddMediatR(assembly);

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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
    }

    private static void RegisterServices(IServiceCollection services)
    {
        DependencyContainer.RegisterServices(services);
    }
}
