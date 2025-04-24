using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using Mshop.Api.Services.IService;

namespace Mshop.Api.Services
{
    public class CartService:Service<Cart>, ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context) :base(context) 
        {
            this._context = context;
        }

        public async Task<bool> CheckExists(Guid productId,Guid userId)
        {
            return await _context.Carts.AnyAsync(c=>c.ProductId == productId&&c.ApplicationUserId==userId); 
        }
        public async Task<bool> DeleteAsync(Guid userId,Guid productId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Carts.FindAsync(productId, userId);
            if (entity is null)
                return false;
            _context.Carts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
