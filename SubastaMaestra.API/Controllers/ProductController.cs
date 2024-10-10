﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubastaMaestra.Data.Interfaces;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Product;

namespace SubastaMaestra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper mapper;
        public ProductController(IProductRepository productoRepository, IMapper mapper)
        {
            _productRepository = productoRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProducto([FromBody] ProductCreateDTO productDTO)
        {
            


            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo invalido: " + ModelState);
            }
            //var product = mapper.Map<Product>(productDTO);
            var result = await _productRepository.CreateProductAsync(productDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message); 
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductById(int id) {
             var product = await _productRepository.GetProductByIdAsync(id);

            if (product.Value == null)
            {
                return NotFound(product);
            }
            //var productDTO = mapper.Map<ProductDTO>(product);
            
            return Ok(product);
        }
        [HttpGet("list")]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var productos = await _productRepository.GetAllProductsAsync();
            
            if (productos.Value == null)
            {
                return NotFound(productos);
            }
           // var productsDTO = mapper.Map<List<ProductCreateDTO>>(productos);
            return Ok(productos.Value);
        }
        [HttpGet("activos")]
        public async Task<ActionResult> GetAllActiveProducts()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            if (products.Value == null)
            {
                return NotFound(products);
            }
            return Ok(products);
        }
        [HttpGet("subasta/{id:int}")]
        public async Task<ActionResult> GetProductByBid(int id)
        {
            var products = await _productRepository.GetProductsByAuctionAsync(id);
            if (products.Value == null)
            {
                return NotFound(products);
            }
            return Ok(products);
        }

        [HttpPost("deshabilitar/{id:int}")]
        public async Task<ActionResult> DisableProduct(int id)
        {
            var result = await _productRepository.DisableProductAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Producto Deshabilitado con éxito.");
        }

        [HttpPost("habilitar/{id:int}")]
        public async Task<ActionResult> EnableProducto(int id)
        {
            var result = await _productRepository.EnableProductAsync(id);
            if (!result.Success)
            { 
                return BadRequest(result.Message);
            }
            return Ok("Producto Habilitado con éxito.");
        }

        [HttpPut("edit/{id:int}")]
        public async Task<ActionResult> EditProducto(ProductDTO product, int id)
        {
            var result = await _productRepository.EditProductAsync(product, id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok("Producto modificado con éxito.");
        }
    }
}
