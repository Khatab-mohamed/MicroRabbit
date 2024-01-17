namespace MicroRabbit.Banking.Application.Interfaces;
public interface IAccountService
{
    IEnumerable<Account> GetAccounts();
}
