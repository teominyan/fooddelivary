using fooddelivary.Shared.Domain;
namespace fooddelivary.Shared.DTO

{
    public class ShopDTO
    {
        public string ShopId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
