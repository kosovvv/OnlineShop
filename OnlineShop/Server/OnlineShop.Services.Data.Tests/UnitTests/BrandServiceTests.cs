using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Data.Common;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Implementations;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Services.Mapping;
using OnlineShop.Web.ViewModels.Brand;

namespace OnlineShop.Services.Data.Tests.UnitTests
{
    [TestFixture]
    public class BrandServiceTests
    {
        private DbContextOptions<StoreContext> options;
        private StoreContext context;
        private IBrandService brandService;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new StoreContext(options);
            context.Database.EnsureCreated();

            // Clear the database
            context.ProductBrands.RemoveRange(context.ProductBrands);
            context.SaveChanges();

            // Seed ProductBrands
            context.ProductBrands.Add(new ProductBrand { Id = 1, Name = "Brand 1", PictureUrl = "brand1.jpg" });
            context.ProductBrands.Add(new ProductBrand { Id = 2, Name = "Brand 2", PictureUrl = "brand2.jpg" });
            context.SaveChanges();

            var unitOfWork = new UnitOfWork(context);
            var mapperMock = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles(false));
            }));
            brandService = new BrandService(mapperMock, unitOfWork);
        }

        [Test]
        public async Task GetProductBrandsAsync_ShouldReturnListOfProductBrandDtos()
        {
            // Arrange

            // Act
            var result = await brandService.GetProductBrandsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2)); // Assuming 2 product brands in test data
            var expectedNames = new[] { "Brand 1", "Brand 2" };

            Assert.That(result.Select(p => p.Name), Is.EquivalentTo(expectedNames));
            // Add assertions for other properties if necessary
        }

        [Test]
        public async Task CreateProductBrandAsync_WhenProductBrandDoesNotExist_ShouldCreateProductBrand()
        {
            // Arrange
            var productBrandToCreate = new CreateProductBrandDto
            {
                Name = "New Brand",
                PictureUrl = "new-brand.jpg"
            };

            // Act
            var result = await brandService.CreateProductBrandAsync(productBrandToCreate);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ReturnProductBrandDto>());
            Assert.That(context.ProductBrands.Count(), Is.EqualTo(3)); // Assuming 3 product brands after creation
        }

        [Test]
        public void CreateProductBrandAsync_WhenProductBrandAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var productBrandToCreate = new CreateProductBrandDto
            {
                Name = "Brand 1",
                PictureUrl = "brand1.jpg"
            };

            // Act & Assert
            Assert.ThrowsAsync<CreateExistringEntityException>(() => brandService.CreateProductBrandAsync(productBrandToCreate));
        }

        [Test]
        public async Task UpdateProductBrandAsync_WhenProductBrandExists_ShouldEditProductBrand()
        {
            // Arrange
            var productBrandId = 1;
            var productBrandToEdit = new CreateProductBrandDto
            {
                Name = "Edited Brand",
                PictureUrl = "edited-brand.jpg"
            };

            // Act
            var result = await brandService.UpdateProductBrandAsync(productBrandId, productBrandToEdit);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ReturnProductBrandDto>());
            Assert.That(context.ProductBrands.Count(), Is.EqualTo(2)); // Assuming no additional product brands are created or deleted
        }

        [Test]
        public void UpdateProductBrandAsync_WhenProductBrandDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var productBrandId = 999; // Non-existent product brand ID
            var productBrandToEdit = new CreateProductBrandDto
            {
                Name = "Edited Brand",
                PictureUrl = "edited-brand.jpg"
            };

            // Act & Assert
            Assert.ThrowsAsync<EntityNotExistingException>(() => brandService.UpdateProductBrandAsync(productBrandId, productBrandToEdit));
        }

        [Test]
        public async Task DeleteProductBrandAsync_WhenProductBrandExists_ShouldDeleteProductBrand()
        {
            // Arrange
            var productBrandId = 1;

            // Act
            var result = await brandService.DeleteProductBrandAsync(productBrandId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(context.ProductBrands.Find(productBrandId)?.IsDeleted);
        }

        [Test]
        public async Task DeleteProductBrandAsync_WhenProductBrandDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var productBrandId = 999; // Non-existent product brand ID

            // Act
            var result = await brandService.DeleteProductBrandAsync(productBrandId);

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
