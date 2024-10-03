using AutoMapper;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Auction;
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

            
            CreateMap<Product, ProductDTO>()
                .ReverseMap();// mapea desde Product a ProductDT y viceversa
            // mapea desde Product a ProductDT.
            CreateMap<ProductCreateDTO, Product>();
            //.ForMember(dest => dest.Buyer, opt => opt.Ignore()) // Ignora propiedades no mapeadas si es necesario
            //.ForMember(dest => dest.Auction, opt => opt.Ignore())
            //.ForMember(dest => dest.Seller, opt => opt.Ignore());

            // Auction Mappin must be After Product, because Auction have a list of Products
            CreateMap<Auction,AuctionDTO>()
                .ReverseMap();
            CreateMap<AuctionCreateDTO, Auction>();

        }

    }
}
