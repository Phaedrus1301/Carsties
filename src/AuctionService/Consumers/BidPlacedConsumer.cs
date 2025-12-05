using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _dbContext;

        public BidPlacedConsumer(AuctionDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("--> BidPlaced : Consuming bid placing in Auction-SVC");
            
            var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

            if (context.Message.BidStatus.Contains("Accepted") &&
                context.Message.Amount > auction.CurrentHighBid 
                || auction.CurrentHighBid == null)
            {
                auction.CurrentHighBid = context.Message.Amount;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
