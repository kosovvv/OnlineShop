using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Data;
using OnlineShop.Data.Common;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Implementations;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Services.Mapping;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.Services.Data.Tests.UnitTests
{
    [TestFixture]
    public class TypeServiceTests
    {
        private DbContextOptions<StoreContext> options;
        private StoreContext context;
        private ITypeService typeService;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new StoreContext(options);
            context.Database.EnsureCreated();

            // Clear the database
            context.ProductTypes.RemoveRange(context.ProductTypes);
            context.SaveChanges();

            // Seed ProductTypes
            context.ProductTypes.Add(new ProductType { Id = 1, Name = "Type 1", PictureUrl = "type1.jpg" });
            context.ProductTypes.Add(new ProductType { Id = 2, Name = "Type 2", PictureUrl = "type2.jpg" });
            context.SaveChanges();

            var unitOfWork = new UnitOfWork(context);
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();

            var mapperMock = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles(false));
            }));
            typeService = new TypeService(unitOfWork, mapperMock);
        }

        [Test]
        public async Task GetProductTypesAsync_ShouldReturnListOfProductTypeDtos()
        {
            // Arrange

            // Act
            var result = await typeService.GetProductTypesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2)); // Assuming 2 product types in test data
            var expectedNames = new[] { "Type 1", "Type 2" };

            Assert.That(result.Select(p => p.Name), Is.EquivalentTo(expectedNames));
            // Add assertions for other properties if necessary
        }

        [Test]
        public async Task CreateProductTypeAsync_WhenProductTypeDoesNotExist_ShouldCreateProductType()
        {
            // Arrange
            var productTypeToCreate = new CreateProductTypeDto
            {
                Name = "New Type",
                PictureUrl = "new-type.jpg"
            };

            // Act
            var result = await typeService.CreateProductTypeAsync(productTypeToCreate);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ReturnProductTypeDto>());
            Assert.That(context.ProductTypes.Count(), Is.EqualTo(3)); // Assuming 3 product types after creation
        }

        [Test]
        public void CreateProductTypeAsync_WhenProductTypeAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var productTypeToCreate = new CreateProductTypeDto
            {
                Name = "Type 1",
                PictureUrl = "type1.jpg"
            };

            // Act & Assert
            Assert.ThrowsAsync<CreateExistringEntityException>(() => typeService.CreateProductTypeAsync(productTypeToCreate));
        }

        [Test]
        public async Task UpdateProductTypeAsync_WhenProductTypeExists_ShouldEditProductType()
        {
            // Arrange
            var productTypeId = 1;
            var productTypeToEdit = new CreateProductTypeDto
            {
                Name = "Edited Type",
                PictureUrl = "edited-type.jpg"
            };

            // Act
            var result = await typeService.UpdateProductTypeAsync(productTypeId, productTypeToEdit);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ReturnProductTypeDto>());
            Assert.That(context.ProductTypes.Count(), Is.EqualTo(2)); // Assuming no additional product types are created or deleted
        }

        [Test]
        public void UpdateProductTypeAsync_WhenProductTypeDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var productTypeId = 999; // Non-existent product type ID
            var productTypeToEdit = new CreateProductTypeDto
            {
                Name = "Edited Type",
                PictureUrl = "edited-type.jpg"
            };

            // Act & Assert
            Assert.ThrowsAsync<EntityNotExistingException>(() => typeService.UpdateProductTypeAsync(productTypeId, productTypeToEdit));
        }

        [Test]
        public async Task DeleteProductTypeAsync_WhenProductTypeExists_ShouldDeleteProductType()
        {
            // Arrange
            var productTypeId = 1;

            // Act
            var result = await typeService.DeleteProductTypeAsync(productTypeId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(context.ProductTypes.Find(productTypeId)?.IsDeleted);
        }

        [Test]
        public async Task DeleteProductTypeAsync_WhenProductTypeDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var productTypeId = 999; // Non-existent product type ID

            // Act
            var result = await typeService.DeleteProductTypeAsync(productTypeId);

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
