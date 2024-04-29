global using MicroRabbit.Transfer.Data.Context;
global using MicroRabbit.Transfer.Domain.Interfaces;

namespace MicroRabbit.Transfer.Data.Repository;


public class TransferRepository : ITransferRepository
{
    private readonly TransferDbContext _ctx;

    public TransferRepository(TransferDbContext ctx)
    {
        _ctx = ctx;
    }
    public IEnumerable<TransferLog> GetTransferLogs()
    {
        return _ctx.TransferLogs;
    }
}
