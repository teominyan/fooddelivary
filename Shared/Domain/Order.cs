using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fooddelivary.Shared.Domain
{
    public class Order : BaseModel
    {

        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; } = 0;

        public string? UserId { get; set; }
        public string? RiderId { get; set; }
        public string? ShopId { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
