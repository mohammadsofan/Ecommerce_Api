using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data.models;
using Mshop.Api.DTOs.Requests;
using Mshop.Api.DTOs.Responses;
using Mshop.Api.Services;

namespace Mshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartService cartService,UserManager<ApplicationUser> userManager) {
            this.cartService = cartService;
            this.userManager = userManager;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                if (Guid.TryParse(userManager.GetUserId(User), out Guid id))
                {
                    var cart = await cartService.GetAsync(c => c.ApplicationUserId==id,false);
                    return Ok(cart.Adapt<IEnumerable<CartResponse>>());
                }
                else
                {
                    return Unauthorized();
                }
                

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest cartRequest)
        {
            try
            {
                if (Guid.TryParse(userManager.GetUserId(User), out Guid id))
                {
                    var isExists = await cartService.CheckExists(cartRequest.ProductId, id);
                    if (isExists) {
                        return Conflict(new { Message = "This product is already in the user's cart." });
                    }
                    var cart = cartRequest.Adapt<Cart>();
                    cart.ApplicationUserId = id;
                    cart = await cartService.AddAsync(cart);
                    return Ok(cart);
                }

                return Unauthorized();

            }

            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
