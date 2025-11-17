using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class AuctionDeletedFaultConsumer
    {
        public async Task Consume(ConsumeContext<Fault<AuctionDeleted>> context)
        {
            Console.WriteLine("--> AuctionDeleted : Consuming faulty creating");
            var exception = context.Message.Exceptions.First();

            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Id = "UFKEDUP";
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("Not an argument exception");
            }
        }
    }
}
