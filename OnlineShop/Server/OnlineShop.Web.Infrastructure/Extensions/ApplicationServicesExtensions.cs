using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OnlineShop.Data;
using OnlineShop.Data.Common;
using OnlineShop.Services.Data.Implementations;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Services.Mapping;
using OnlineShop.Web.Infrasctucture.Helpers;
using OnlineShop.Web.ViewModels;
using StackExchange.Redis;

namespace OnlineShop.Web.Infrastructure
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {

            var connectionString = config.GetSection("MongoDB")["ConnectionString"];
            var databaseName = config.GetSection("MongoDB")["DatabaseName"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddSingleton<IMongoDatabase>(database);

            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });

            services.Configure<ImageDatabaseSettings>(config.GetSection("MongoDB"));

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ITypeService, TypeService>();
            services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage);

                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            return services;
        }
    }
}
