using fooddelivary.Shared.Domain;
using Microsoft.AspNetCore.Identity;

namespace fooddelivary.Server.Models
{
    public class Shop : ApplicationUser
    {
        public List<Order> ShopOrders { get; set; } = new List<Order>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
