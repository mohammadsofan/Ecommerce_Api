using Mshop.Api.Data.models;
using Mshop.Api.Services.IService;

namespace Mshop.Api.Services
{
    public interface ICartService:IService<Cart>
    {
        Task<bool> CheckExists(Guid productId, Guid userId);
    }
}
