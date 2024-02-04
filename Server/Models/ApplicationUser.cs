using fooddelivary.Shared.Domain;
using Microsoft.AspNetCore.Identity;

namespace fooddelivary.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Order> ActiveOrders { get; set; } = new List<Order>();
        public List<Order> PreviousOrders { get; set; } = new List<Order>();
        public List<Product> Cart { get; set; } = new List<Product>();
        public string Address { get; set; } = string.Empty;

        public string UserType { get; set; } = "ApplicationUser";


    }
}
