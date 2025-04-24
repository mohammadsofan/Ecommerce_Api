using Mshop.Api.Data.models;
using Mshop.Api.Services.IService;

namespace Mshop.Api.Services
{
    public interface ICartService:IService<Cart>
    {
        Task<bool> CheckExists(Guid productId, Guid userId);
        Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);
        Task<bool> EditQuantityAsync(Guid userId, Guid productId, int newQuantity, CancellationToken cancellationToken = default);

    }
}
