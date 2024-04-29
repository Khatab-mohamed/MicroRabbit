using Microsoft.EntityFrameworkCore.Design;

namespace MicroRabbit.Banking.Data.Context;
public class BankingDbContextFactory : IDesignTimeDbContextFactory<BankingDbContext>
{
    public BankingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BankingDbContext>();
        optionsBuilder.UseSqlServer("BankingDbConnection");

        return new BankingDbContext(optionsBuilder.Options);
    }
}
