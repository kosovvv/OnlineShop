using System.ComponentModel.DataAnnotations.Schema;

namespace Skinet.Core.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
