using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OnlineShop.Data;
using OnlineShop.Data.Common;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Implementations;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Services.Mapping;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Services.Data.Tests.UnitTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private DbContextOptions<StoreContext> options;
        private StoreContext context;
        private IProductService productService;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new StoreContext(options);
            context.Database.EnsureCreated();

            // Clear the database
            context.Products.RemoveRange(context.Products);
            context.ProductTypes.RemoveRange(context.ProductTypes);
            context.ProductBrands.RemoveRange(context.ProductBrands);
            context.SaveChanges();

            // Seed ProductType and ProductBrand
            context.ProductTypes.Add(new ProductType { Id = 1, Name = "Type 1", PictureUrl = "type1.jpg" });
            context.ProductTypes.Add(new ProductType { Id = 2, Name = "Type 2", PictureUrl = "type2.jpg" });

            context.ProductBrands.Add(new ProductBrand { Id = 1, Name = "Brand 1", PictureUrl = "brand1.jpg" });
            context.ProductBrands.Add(new ProductBrand { Id = 2, Name = "Brand 2", PictureUrl = "brand2.jpg" });
            context.SaveChanges();

            // Insert test data
            context.Products.Add(new Product { Id = 1, Name = "Product 1", Description = "Description 1", PictureUrl = "url1.jpg", Price = 10.99m, ProductTypeId = 1, ProductBrandId = 1 });
            context.Products.Add(new Product { Id = 2, Name = "Product 2", Description = "Description 2", PictureUrl = "url2.jpg", Price = 20.99m, ProductTypeId = 2, ProductBrandId = 2 });
            context.SaveChanges();

            var unitOfWork = new UnitOfWork(context);
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();

            var mapperMock = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles(false));
            }));
            productService = new ProductService(unitOfWork, mapperMock);
        }


        [Test]
        public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProductDto()
        {
            // Arrange
            var productId = 1;

            // Act
            var result = await productService.GetProductByIdAsync(productId);

            // Assert
            Assert.IsNotNull(result);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(productId));
                Assert.That(result.Name, Is.EqualTo("Product 1"));
                Assert.That(result.Description, Is.EqualTo("Description 1"));
                Assert.That(result.PictureUrl, Is.EqualTo("url1.jpg"));
                Assert.That(result.Price, Is.EqualTo(10.99m));
                Assert.That(result.ProductType, Is.EqualTo("Type 1"));
                Assert.That(result.ProductBrand, Is.EqualTo("Brand 1"));
            });
            // Add assertions for other properties if necessary
        }

        [Test]
        public async Task GetProductByIdAsync_WhenProductDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var productId = 999; // Non-existent product ID

            // Act & Assert
            Assert.ThrowsAsync<EntityNotExistingException>(async () => await productService.GetProductByIdAsync(productId));
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnListOfProductDtos()
        {
            // Arrange

            // Act
            var result = await productService.GetProductsAsync(new ProductParams());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2)); // Assuming 2 products in test data
            Assert.Multiple(() =>
            {
                Assert.That(result.First().Name, Is.EqualTo("Product 1"));
                Assert.That(result.Last().Name, Is.EqualTo("Product 2"));
            });
            // Add assertions for other properties if necessary
        }

        [Test]
        public async Task GetProductsCountAsync_ShouldReturnCorrectCount()
        {
            // Arrange

            // Act
            var result = await productService.GetProductsCountAsync(new ProductParams());

            // Assert
            Assert.That(result, Is.EqualTo(2)); // Assuming 2 products in test data
        }

        [Test]
        public async Task CreateProduct_WhenProductDoesNotExist_ShouldCreateProduct()
        {
            // Arrange
            var productToCreate = new ProductToCreateDto
            {
                Name = "New Product",
                Description = "New Description",
                PictureUrl = "new-url.jpg",
                Price = 30.99m,
                ProductType = "Type 1",
                ProductBrand = "Brand 1"
            };

            // Act
            var result = await productService.CreateProduct(productToCreate);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ProductToReturnDto>());
            Assert.That(context.Products.Count(), Is.EqualTo(3)); // Assuming 3 products after creation
        }

        [Test]
        public void CreateProduct_WhenProductAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var productToCreate = new ProductToCreateDto
            {
                Name = "Product 1",
                Description = "Description 1",
                PictureUrl = "url1.jpg",
                Price = 10.99m,
                ProductType = "Type 1",
                ProductBrand = "Brand 1"
            };

            // Act & Assert
            Assert.ThrowsAsync<CreateExistringEntityException>(() => productService.CreateProduct(productToCreate));
        }

        [Test]
        public async Task EditProduct_WhenProductExists_ShouldEditProduct()
        {
            // Arrange
            var productId = 1;
            var productToEdit = new ProductToCreateDto
            {
                Name = "Edited Product",
                Description = "Edited Description",
                PictureUrl = "edited-url.jpg",
                Price = 40.99m,
                ProductType = "Type 1",
                ProductBrand = "Brand 1"
            };

            // Act
            var result = await productService.EditProduct(productId, productToEdit);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ProductToReturnDto>());
            Assert.That(context.Products.Count(), Is.EqualTo(2)); // Assuming no additional products are created or deleted
        }

        [Test]
        public void EditProduct_WhenProductDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var productId = 999; // Non-existent product ID
            var productToEdit = new ProductToCreateDto
            {
                Name = "Edited Product",
                Description = "Edited Description",
                PictureUrl = "edited-url.jpg",
                Price = 40.99m,
                ProductType = "Type 1",
                ProductBrand = "Brand 1"
            };

            // Act & Assert
            Assert.ThrowsAsync<EntityNotExistingException>(() => productService.EditProduct(productId, productToEdit));
        }

        [Test]
        public async Task DeleteProduct_WhenProductExists_ShouldDeleteProduct()
        {
            // Arrange
            var productId = 1;

            // Act
            var result = await productService.DeleteProduct(productId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(context.Products.Find(productId)?.IsDeleted);
        }

        [Test]
        public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var productId = 999; // Non-existent product ID

            // Act
            var result = await productService.DeleteProduct(productId);

            // Assert
            Assert.That(result, Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
