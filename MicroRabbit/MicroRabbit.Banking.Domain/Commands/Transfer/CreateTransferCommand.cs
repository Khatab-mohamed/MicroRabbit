namespace MicroRabbit.Banking.Domain.Commands.Transfer;

public class CreateTransferCommand : TransferCommand
{
    public CreateTransferCommand(int from, int to, decimal amount)
    {
        From = from;
        To = to;
        Amount = amount;
    }
}