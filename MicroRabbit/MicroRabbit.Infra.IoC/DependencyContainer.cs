
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MicroRabbit.Infra.IoC;


public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services,IConfiguration configuration)
    {
        // Domain Bus
        services.AddTransient<IEventBus, RabbitMQBus>();
        // Domain Banking Commands
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, CreateTransferCommandHandler>();
        // Application Services
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ITransferService, TransferService>();
        // Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<ITransferRepository, TransferRepository>();

        services.AddDbContext<BankingDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("BankingDbConnection")));
        services.AddDbContext<TransferDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TransferDbConnection")));     
        // Domain Events
        services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();
    }
}
