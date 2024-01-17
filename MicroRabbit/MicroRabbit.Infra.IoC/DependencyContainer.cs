using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using System.Reflection;

namespace MicroRabbit.Infra.IoC;


public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        // Domain Bus
        services.AddTransient<IEventBus, RabbitMQBus>();

        // Application Services
        services.AddTransient<IAccountService, AccountService>();


        // Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddScoped<BankingDbContext>();


    }
}
 