namespace MicroRabbit.Infra.Bus;


public sealed class RabbitMQBus : IEventBus
{
    #region Fields

    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;
    #endregion

    #region Ctor
    public RabbitMQBus(IMediator mediator,
                       Dictionary<string, List<Type>> handlers,
                       List<Type> eventTypes)
    {
        _mediator = mediator;
        _handlers = handlers;
        _eventTypes = eventTypes;
    }
    #endregion

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory() { HostName = "localhost" }; // create a connection factory
        using var connection = factory.CreateConnection(); // connection to RabbitMQ
        using var channel = connection.CreateModel(); // channel to RabbitMQ
        var eventName = @event.GetType().Name; // get the name of the event
        channel.QueueDeclare(eventName, false, false, false, null); // declare a queue for the event
        var message = JsonConvert.SerializeObject(@event); // serialize the event
        var body = Encoding.UTF8.GetBytes(message); // convert the event to bytes
        channel.BasicPublish("", eventName, null, body); // publish the event

    }

    public void Subscribe<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name; // get the name of the event
        var handlerType = typeof(TH); // get the type of the event handler

        var containsKey = _handlers.ContainsKey(eventName); // check if the event is already registered
        if (!containsKey)
        {
            _handlers.Add(eventName, new List<Type>()); // add the event to the list of events
        }
        var handler = _handlers[eventName];// get the event handler for the event
        var contains = handler.Any(s => s.GetType() == handlerType); // check if the event handler is already registered
        if (!contains)
        {
            handler.Add(handlerType); // add the event handler to the list of event handlers
        }
        else
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for '{eventName}'",
                                        nameof(handlerType)); // throw an exception if the event handler is already registered
        }
        _handlers[eventName] = handler; // add the event handler to the list of event handlers

        StartBasicConsume<T>(); // start the basic consume

    }
    private void StartBasicConsume<T>() where T : Event
    {
        var factory = new ConnectionFactory() { HostName = "localhost" }; // create a connection factory
        var connection = factory.CreateConnection(); // connection to RabbitMQ
        var channel = connection.CreateModel(); // channel to RabbitMQ
        var eventName = typeof(T).Name; // get the name of the event
        channel.QueueDeclare(eventName, false, false, false, null); // declare a queue for the event
        var consumer = new AsyncEventingBasicConsumer(channel); // create a consumer
        consumer.Received += Consumer_Received; // subscribe to the consumer received event
        channel.BasicConsume(eventName, true, consumer); // start the basic consume

    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey; // get the name of the event
        var message = Encoding.UTF8.GetString(@event.Body.ToArray()); // convert the event to bytes
        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName)) // check if the event is already registered
        {
            var subscriptions = _handlers[eventName]; // get the event handler for the event
            foreach (var subscription in subscriptions)
            {
                var handler = Activator.CreateInstance(subscription); // create an instance of the event handler
                if (handler == null) continue; // check if the event handler is null
                var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName); // get the type of the event
                var @event = JsonConvert.DeserializeObject(message, eventType); // deserialize the event
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType); // get the type of the event handler
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event }); // invoke the event handler
            }
        }
    }
}
