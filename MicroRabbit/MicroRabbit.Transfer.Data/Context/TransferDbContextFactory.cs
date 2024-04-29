namespace MicroRabbit.Transfer.Data.Context;

public class TransferDbContextFactory : IDesignTimeDbContextFactory<TransferDbContext>
{
    public TransferDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TransferDbContext>();
        optionsBuilder.UseSqlServer("TransferDbConnection");

        return new TransferDbContext(optionsBuilder.Options);
    }
}
