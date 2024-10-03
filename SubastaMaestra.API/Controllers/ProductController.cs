using AutoMapper;
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
        public async Task<ActionResult<ProductCreateDTO>> CreateProducto([FromBody] ProductCreateDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo invalido: " + ModelState);
            }
            //var product = mapper.Map<Product>(productDTO);
            var result = await _productRepository.CreateProductAsync(productDTO);
            if (result == -1)
            {
                return BadRequest("No se pudo crear el producto");
            }
            return Ok(); 
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductById(int id) {
             var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            //var productDTO = mapper.Map<ProductDTO>(product);
            
            return Ok(product);
        }
        [HttpGet("todos")]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var productos = await _productRepository.GetAllProductsAsync();
            
            if (productos == null)
            {
                return NotFound();
            }
           // var productsDTO = mapper.Map<List<ProductCreateDTO>>(productos);
            return Ok(productos);
        }
        [HttpGet("activos")]
        public async Task<ActionResult> GetAllActiveProducts()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpGet("subasta/{id:int}")]
        public async Task<ActionResult> GetProductByBid(int id)
        {
            var products = await _productRepository.GetProductsByBidAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost("deshabilitar/{id:int}")]
        public async Task<ActionResult> DisableProduct(int id)
        {
            var result = await _productRepository.DisableProductAsync(id);
            if (result == 0)
            {
                return NotFound();
            }
            else if (result == -1)
            {
                return BadRequest("No se puede efectuar la operación");
            }
            return Ok("Producto deshabilitado.");
        }

        [HttpPost("habilitar/{id:int}")]
        public async Task<ActionResult> EnableProducto(int id)
        {
            var result = await _productRepository.EnableProductAsync(id);
            if (result == 0)
            {
                return NotFound();
            }
            else if (result == -1)
            {
                return BadRequest("No se puede efectuar la operación");
            }
            return Ok("Producto Habilitado con éxito.");
        }

        [HttpPut("edit")]
        public async Task<ActionResult> EditProducto(Product product)
        {
            var result = await _productRepository.EditProductAsync(product);
            if (result == 0)
            {
                return NotFound();
            }
            else if (result == -1)
            {
                return BadRequest("No se puede efectuar la operación");
            }
            return Ok("Producto modificado con éxito.");
        }
    }
}
