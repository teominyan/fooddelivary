using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fooddelivary.Shared.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int RiderId { get; set; }
        public int ShopId { get; set; }
        public string Status { get; set; } = null!;
        public decimal Total { get; set; }
    }
}
