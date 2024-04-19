using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Common;
using OnlineShop.Data.Common.Repositories;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Order;

namespace OnlineShop.Services.Data.Implementations
{
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<DeliveryMethod> dmRepository;

        public DeliveryMethodService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.dmRepository = unitOfWork.GetRepository<DeliveryMethod>();
        }
        public async Task<IEnumerable<ReturnDeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var methods = await dmRepository.AllAsNoTracking().ToListAsync();
            return this.mapper.Map<ICollection<DeliveryMethod>, ICollection<ReturnDeliveryMethodDto>>(methods);
        }
        public async Task<ReturnDeliveryMethodDto> GetDeliveryMethodByIdAsync(int id)
        {
            var method = await dmRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            return this.mapper.Map<DeliveryMethod, ReturnDeliveryMethodDto>(method);
        }
    }
}
