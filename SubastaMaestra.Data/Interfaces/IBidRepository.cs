using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Bid;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IBidRepository
    {
        Task<OperationResult<BidCreateDTO>> CreateBid(BidCreateDTO bid);
        Task<OperationResult<List<BidDTO>>> ObtenerOfertasPorProducto(int id_producto);

        Task<OperationResult<List<BidderDTO>>> GetBiddersByProduct(int id_product);
        Task<OperationResult<List<BidDTO>>> GetBidsByUser(int bidder_id);


    }
}
