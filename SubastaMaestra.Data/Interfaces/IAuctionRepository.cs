using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using SubastaMaestra.Models.DTOs.Auction;
using SubastaMaestra.Models.DTOs.Reports;
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
        Task<OperationResult<int>> ActivateAuctionAsync(int idid_subasta);
        Task<OperationResult<int>> DisableAuctionAsync(int id_subasta);
        Task<OperationResult<int>> EditAuctionAsync(AuctionUpdateDTO subasta, int id);
        Task<OperationResult<AuctionDTO>> GetAuctionByIdAsync(int id);
        Task<OperationResult<List<AuctionDTO>>> GetAllOpenAuctionAsync(); // eliminar
        Task<OperationResult<List<AuctionDTO>>> GetAllClosedAuctionAsync();// eliminar
        Task<OperationResult<List<AuctionDTO>>> GetAllClosedAuctionWithProductsAsync(); // eliminar
        Task<OperationResult<List<AuctionDTO>>> GetAllAuctionsAsync();
        Task<OperationResult<List<AuctionDTO>>> GetAllPendingAuctionsAsync(); // eliminar

        Task<OperationResult<List<AuctionDTO>>> GetAllAuctionByCurrentStateAsync(AuctionState currentState);
        // repote
        Task<List<AuctionReportDTO>> ObtenerSubastasMasPopulares(DateTime inicio, DateTime fin);
        Task<List<ProfitReportDTO>> GetProfitReport();
    }
}
