using MicroRabbit.Banking.Domain.Events;

namespace MicroRabbit.Banking.Domain.CommandHandlers.Transfer;



public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
{
    private readonly IEventBus _bus;

    public CreateTransferCommandHandler(IEventBus bus)
    {
        _bus = bus;
    }

    public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        // publish event to RabbitMQ
        
        _bus.Publish(new TransferCreatedEvent(request.From,
                                              request.To,
                                              request.Amount));

        return Task.FromResult(true);

    }
}
