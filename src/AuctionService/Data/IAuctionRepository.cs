using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService.Data
{
    public interface IAuctionRepository
    {
        //replacing AuctionDBContext wth Interface so that its easier to do MOCK tests
        //this also removes coupling - usecase of Repository Layer

        Task<List<AuctionDto>> GetAuctionAsync(string date);
        Task<AuctionDto> GetAuctionByIdAsync(Guid id);
        Task<Auction> GetAuctionEntityById(Guid id);
        void AddAuction(Auction auction);
        void RemoveAuction(Auction auction);
        Task<bool> SaveChangesAsync(); 
    }
}
