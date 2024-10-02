using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Implements
{
    public class BidRepository : IBidRepository
    {
        private readonly SubastaContext _context;
        public BidRepository(SubastaContext context) {
            _context = context;
        }

        public async Task<int> RealizarOferta(Bid oferta)
        {
            try
            {
                await _context.Bids.AddAsync(oferta);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
               
                return -1;  // Devuelve -1 si ocurre un error
            }
        }

        public async Task<List<Bid>> ObtenerOfertasPorProducto(int id_producto)
        {
            try
            {
                var productos = await _context.Bids.Where(
                    of => of.ProductId == id_producto).ToListAsync();
                if (productos.Any())
                {
                    return productos;
                }
                return null;
            }
            catch (Exception ex)
            {
                return new List<Bid>();
            }
        }

       
    }
}

