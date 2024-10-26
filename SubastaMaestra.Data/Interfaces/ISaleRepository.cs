using SubastaMaestra.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubastaMaestra.Models.DTOs.Sale;

namespace SubastaMaestra.Data.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<SaleDTO>> GetAllSalesAsync();
    }
}
