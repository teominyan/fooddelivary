using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using fooddelivary.Server.Models;
using fooddelivary.Shared.DTO;
using fooddelivary.Shared.Domain;
using fooddelivary.Server.Data;
using Microsoft.EntityFrameworkCore;


namespace fooddelivary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context; 
        }
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<ApplicationUserDTO>>> GetApplicationUser()
        {
            var users = _userManager.Users;
            var user = users.Where(u => u.UserType == "ApplicationUser");

            var userDTOs = user.Select(u => new ApplicationUserDTO
            {
                UserId = u.Id,
                Name = u.Name,
                Address = u.Address,
            });

            return Ok(userDTOs);
        }

        [HttpGet("shops")]
        public async Task<ActionResult<IEnumerable<ShopDTO>>> GetShops()
        {
            var users = _userManager.Users;
            var shops = users.Where(u => u.UserType == "Shop").Cast<Shop>();

            var shopDTOs = shops.Select(u => new ShopDTO
            {
                ShopId = u.Id,
                Name = u.Name,
                Address = u.Address,
                Products = u.Products
            });

            return Ok(shopDTOs);
        }

        [HttpGet("riders")]
        public async Task<ActionResult<IEnumerable<RiderDTO>>> GetRider()
        {
            var users = _userManager.Users;
            var rider = users.Where(u => u.UserType == "Shop").Cast<Rider>();

            var riderDTOs = rider.Select(u => new RiderDTO
            {
                RiderId = u.Id,
                Name = u.Name,
                Address = u.Address,
            });

            return Ok(riderDTOs);
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<ApplicationUserDTO>> GetApplicationUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.UserType != "ApplicationUser")
            {
                return NotFound();
            }

            var userDTO = new ApplicationUserDTO
            {
                UserId = user.Id,
                Name = user.Name,
                Address = user.Address,
            };

            return Ok(userDTO);
        }
        [HttpGet("cart")]
        public async Task<ActionResult<IEnumerable<Product>>> GetUserCart()
        {
            var user = await _userManager.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            if (user == null || user.UserType != "ApplicationUser")
            {
                return NotFound();
            }

            var cartProducts = user.Cart;
            // Console log the cartProducts to see what it looks like
            Console.WriteLine("Card: {0}", cartProducts);
            if(cartProducts == null)
            {
                return NotFound();
            }
            Console.WriteLine("Card: {0}", cartProducts);
            // DTO mapping
            var productDTOs = cartProducts.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ShopId = p.ApplicationUserId,
            });

            return Ok(productDTOs);
        }

        [HttpPost("cart")]
        public async Task<ActionResult> AddToCart([FromBody] int productId)
        {
            Console.WriteLine("productId: {0}", productId);
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.UserType != "ApplicationUser")
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(productId);
            Console.WriteLine("product: {0}", product.Id);
            if (product == null)
            {
                return NotFound();
            }

            if (user.Cart == null)
            {
                user.Cart = new List<Product>();
            }

            user.Cart.Add(product);

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var user = await _userManager.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            if (user == null || user.UserType != "ApplicationUser")
            {
                return NotFound();
            }

            var product = user.Cart.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            user.Cart.Remove(product);

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("shops/{id}")]
        public async Task<ActionResult<ShopDTO>> GetShop(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.UserType != "Shop")
            {
                return NotFound();
            }

            var shop = user as Shop;
            var shopDTO = new ShopDTO
            {
                ShopId = shop.Id,
                Name = shop.Name,
                Address = shop.Address,
                Products = shop.Products
            };

            return Ok(shopDTO);
        }

        [HttpGet("riders/{id}")]
        public async Task<ActionResult<RiderDTO>> GetRider(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.UserType != "Rider")
            {
                return NotFound();
            }

            var rider = user as Rider;
            var riderDTO = new RiderDTO
            {
                RiderId = rider.Id,
                Name = rider.Name,
                Address = rider.Address,
            };

            return Ok(riderDTO);
        }
    }
}