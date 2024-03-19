namespace Skinet.Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }

        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }
        public string Id { get; set; }
        public ICollection<BasketItem> Items { get; set; } 
    }
}
