using fooddelivary.Shared.Domain;
using Microsoft.AspNetCore.Identity;

namespace fooddelivary.Server.Models
{
    public class Rider : ApplicationUser
    {
        public List<Order> RiderOrders { get; set; } = new List<Order>();
    }
}
