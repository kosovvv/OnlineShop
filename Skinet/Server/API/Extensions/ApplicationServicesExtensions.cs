using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Interfaces;
using Skinet.Infrastructure.Data;
using Skinet.WebAPI.Errors;
using StackExchange.Redis;
using System.Text;

namespace Skinet.WebAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {   
            
            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                // Add Identity Options
            })
            .AddEntityFrameworkStores<StoreContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false,
                    };
                });

            services.AddAuthorization();

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));

                return ConnectionMultiplexer.Connect(options);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
