namespace MicroRabbit.Banking.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BankingDbContext _ctx;

    public AccountRepository(BankingDbContext ctx)
    {
        _ctx = ctx;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _ctx.Accounts;
    }
}
