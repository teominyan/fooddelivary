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
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/Products
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            if(userId == null)
            {
                return NotFound();
            }

            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var userProducts = products.Where(p => p.ApplicationUserId == userId);


            return userProducts.ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ProductRepository.UpdateAsync(product);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                if (await _unitOfWork.ProductRepository.GetByIdAsync(id) == null)
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

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)

        {
            var user = await _userManager.GetUserAsync(User);
            product.ApplicationUserId = user.Id;

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _unitOfWork.ProductRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpGet ("all")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user.UserType != "ApplicationUser")
            {
                return NotFound();
            }

            var products = await _unitOfWork.ProductRepository.GetAllAsync();


            return products.ToList();
        }

    }
}