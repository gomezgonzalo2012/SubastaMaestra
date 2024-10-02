using AutoMapper;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Product, ProductDTO>().ReverseMap();// mapea desde Product a ProductDT y viceversa
            CreateMap<ProductCreateDTO, Product>();// mapea desde Product a ProductDT.
        }

    }
}
