using SubastaMaestra.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IBidRepository
    {
        public Task<int> RealizarOferta(Bid oferta);
        Task<List<Bid>> ObtenerOfertasPorProducto(int id_producto);

        

    }
}
