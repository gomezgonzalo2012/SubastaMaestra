using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IAuctionRepository
    {
        Task<int> CreateAuctionAsync(AuctionCreateDTO subasta);
        Task<int> CloseAuctionAsync(int id_subasta);
        Task<int> EditAuctionAsync(Auction subasta);
        Task<AuctionDTO> GetAuctionByIdAsync(int id);
        Task<List<AuctionDTO>> GetAllOpenAuctionAsync();
        Task<List<Auction>> GetAllClosedAuctionAsync();
        Task<List<Auction>> GetAllClosedAuctionWithProductsAsync();
        Task<List<Auction>> GetAllAuctionsAsync();




    }
}
