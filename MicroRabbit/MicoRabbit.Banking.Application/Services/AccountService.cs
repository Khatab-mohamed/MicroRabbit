
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands.Transfer;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services;


public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEventBus _bus;

    public AccountService(IAccountRepository accountRepository,
                          IEventBus bus)
    {
        _accountRepository = accountRepository;
        _bus = bus;
    }

    public IEnumerable<Account> GetAccounts()
    {
        var accounts = _accountRepository.GetAccounts();
        return accounts;
    }

    public void Transfer(AccountTransfer accountTransfer)
    {
        var createTransferCommand = new CreateTransferCommand(
            accountTransfer.FromAccount,
            accountTransfer.ToAccount,
            accountTransfer.TransferAmount);
        _bus.SendCommand(createTransferCommand); // Message published to RabbitMQ
    }
}
