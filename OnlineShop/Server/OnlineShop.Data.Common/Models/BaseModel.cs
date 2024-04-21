using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Data.Common.Models
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey? Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }
    }
}
