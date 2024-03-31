using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;
using System.Linq;


namespace OnlineShop.WebAPI.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductsController(IProductService productService,
            IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Cached(600)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
            ([FromQuery] ProductParams productParams)
        {
            var totalItems = await productService.GetProductsCountAsync(productParams);

            var products = await productService.GetProductsAsync(productParams);


            var data = mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>
                (productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //[Cached(600)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await productService.GetProductByIdAsync(id);

            if (product == null) return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct(ProductToCreateDto product)
        {
            Product productToCreate = this.mapper.Map<Product>(product);

            var productType = await productService.GetProductTypeByNameAsync(product.ProductType);

            if (productType != null)
            {
                productToCreate.ProductType = productType;
            }

            var productBrand = await productService.GetProductBrandByNameAsync(product.ProductBrand);

            if (productBrand != null)
            {
                productToCreate.ProductBrand = productBrand;
            }

            var result = await productService.CreateProduct(productToCreate);

            return Ok(this.mapper.Map<ProductToReturnDto>(result));
        }

        [HttpPut("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> EditProduct(int id, ProductToCreateDto product)
        {
            Product productToEdit = this.mapper.Map<Product>(product);

            var productType = await productService.GetProductTypeByNameAsync(product.ProductType);

            if (productType != null)
            {
                productToEdit.ProductType = productType;
            }

            var productBrand = await productService.GetProductBrandByNameAsync(product.ProductBrand);

            if (productBrand != null)
            {
                productToEdit.ProductBrand = productBrand;
            }
            productToEdit.Id = id;

            var result = await this.productService.EditProduct(id, productToEdit);
            return Ok(this.mapper.Map<ProductToReturnDto>(result));
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var isDeleted = await productService.DeleteProduct(id);
            return isDeleted ? Ok() : NotFound();
        }

        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productService.GetProductBrandsAsync());
        }


        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductTypes()
        {
            return Ok(await productService.GetProductTypesAsync());
        }
    }
}
