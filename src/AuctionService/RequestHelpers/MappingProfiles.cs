using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;



namespace AuctionService.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>()
                .IncludeMembers(x => x.Item); //When mapping from Auction to AuctionDto, also include members from the related Item entity
            CreateMap<Item, AuctionDto>(); //helper for line above: explains how to map Item

            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s)); //When creating a new Auction from a CreateAuctionDto, map the Item property from the same source
            CreateMap<CreateAuctionDto, Item>(); //helper for line above: explains how to map Item
        }
    }
}
