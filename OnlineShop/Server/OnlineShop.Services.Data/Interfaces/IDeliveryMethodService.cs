using OnlineShop.Web.ViewModels.Order;


namespace OnlineShop.Services.Data.Interfaces
{
    public interface IDeliveryMethodService
    {
        Task<IEnumerable<ReturnDeliveryMethodDto>> GetDeliveryMethodsAsync();

        Task<ReturnDeliveryMethodDto> GetDeliveryMethodByIdAsync(int id);
    }
}
