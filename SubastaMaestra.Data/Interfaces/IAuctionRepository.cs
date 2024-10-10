using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IAuctionRepository
    {
        Task<OperationResult<AuctionCreateDTO>> CreateAuctionAsync(AuctionCreateDTO subasta);
        Task<OperationResult<int>> CloseAuctionAsync(int id_subasta);
        Task<OperationResult<int>> EditAuctionAsync(AuctionDTO subasta, int id);
        Task<OperationResult<AuctionDTO>> GetAuctionByIdAsync(int id);
        Task<OperationResult<List<AuctionDTO>>> GetAllOpenAuctionAsync();
        Task<OperationResult<List<AuctionDTO>>> GetAllClosedAuctionAsync();
        Task<OperationResult<List<AuctionDTO>>> GetAllClosedAuctionWithProductsAsync();
        Task<OperationResult<List<AuctionDTO>>> GetAllAuctionsAsync();




    }
}
