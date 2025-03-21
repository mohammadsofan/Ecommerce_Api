using Mshop.Api.Data.models;
using System.Linq.Expressions;

namespace Mshop.Api.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category? Get(Expression<Func<Category, bool>> expression);
        Category Add(Category category);
        bool Delete(Guid id);
        bool Edit(Guid id,Category category);
    }
}
