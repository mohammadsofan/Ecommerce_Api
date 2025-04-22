using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using Mshop.Api.Services.IService;

namespace Mshop.Api.Services
{
    public class CartService:Service<Cart>, ICartService
    {
        private readonly ApplicationDbContext _contetx;

        public CartService(ApplicationDbContext contetx):base(contetx) 
        {
            this._contetx = contetx;
        }

        public async Task<bool> CheckExists(Guid productId,Guid userId)
        {
            return await _contetx.Carts.AnyAsync(c=>c.ProductId == productId&&c.ApplicationUserId==userId); 
        }
    }
}
