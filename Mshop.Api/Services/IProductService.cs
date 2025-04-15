using Mshop.Api.Data.models;
using System.Linq.Expressions;

namespace Mshop.Api.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(string? query, int page = 1, int limit= 10);
        Product? Get(Expression<Func<Product, bool>> expression);
        Product Add(Product product,IFormFile image);
        bool Delete(Guid id);
        bool Edit(Guid id, Product product, IFormFile? image);
    }
}
