namespace MicroRabbit.Transfer.Application.Interfaces;

public interface ITransferService
{
    IEnumerable<TransferLog> GetTransferLogs();
}
