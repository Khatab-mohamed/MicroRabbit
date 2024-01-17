
namespace MicroRabbit.Banking.Application.Services;


public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IEnumerable<Account> GetAccounts()
    {
        var accounts = _accountRepository.GetAccounts();
        return accounts;
    }
}
