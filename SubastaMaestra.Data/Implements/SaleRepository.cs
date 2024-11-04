using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Product;
using SubastaMaestra.Models.DTOs.Sale;
using SubastaMaestra.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Implements
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SubastaContext _subastaContext;
        private readonly IMapper _mapper;
        public SaleRepository( SubastaContext context, IMapper mapper)
        {
            _subastaContext = context;
            _mapper = mapper;
        }
        public async Task<List<SaleDTO>> GetAllSalesAsync()
        {
            List<Sale> saleList = await _subastaContext.Sales.ToListAsync();
            var saleDTOList = new List<SaleDTO>();

            if (saleList != null) {
                foreach(var sale in saleList)
                {
                    // validar nulos
                    var product = await _subastaContext.Products.Where(p=> p.Id == sale.ProductId).FirstOrDefaultAsync();
                    var buyer = await _subastaContext.Users.Where(u=> u.Id == sale.BuyerId).FirstOrDefaultAsync();
                    if (product != null && buyer != null)
                    {
                        var seller = await _subastaContext.Users.Where(u => u.Id == product.SellerId).FirstOrDefaultAsync();
                        var productDto = _mapper.Map<ProductDTO>(product);
                        var buyerDto = _mapper.Map<UserDTO>(buyer);
                        var sellerDto = _mapper.Map<UserDTO>(seller);

                        var saleDto = new SaleDTO()
                        {
                            Amount = sale.Amount,
                            Product = productDto,
                            Buyer = buyerDto,
                            Seller = sellerDto,
                            Deduccion = sale.Deduccion,
                            PaymentMethod = sale.PaymentMethod,
                            SaleDate = sale.SaleDate,
                            Id = sale.Id
                        };
                        saleDTOList.Add(saleDto);
                    }
                }
            }
            return saleDTOList;

        }
    }
}
