using API.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Skinet.WebAPI.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
