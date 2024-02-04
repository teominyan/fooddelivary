using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fooddelivary.Server.IRepository;
using fooddelivary.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using fooddelivary.Server.Models;

namespace fooddelivary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()

        {
            if(User == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound();
            }
            var userId = user.Id;


            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            var userOrders = orders.Where(o => o.UserId == userId);
            if (orders == null)
            {
                return NotFound();
            }
            return userOrders.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            try
            {
                string shopId = order.Products[0].ApplicationUserId;
                Console.WriteLine($"ShopId{0}",shopId);
                order.ShopId = shopId;

                var user = await _userManager.GetUserAsync(User);
                if(user.UserType == "ApplicationUser")
                {
                    order.UserId = user.Id;
                }

                // Fetch the existing Products from the database
                for (int i = 0; i < order.Products.Count; i++)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(order.Products[i].Id);
                    if (product == null)
                    {
                        return NotFound($"Product with id {order.Products[i].Id} not found");
                    }
                    order.Products[i] = product;
                }

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine(ex.ToString());
                // Return a 500 error with the exception message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _unitOfWork.OrderRepository.UpdateAsync(order);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                if (await _unitOfWork.OrderRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _unitOfWork.OrderRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            if (orders == null)
            {
                return NotFound();
            }
            return orders.ToList();
        }
    }
}