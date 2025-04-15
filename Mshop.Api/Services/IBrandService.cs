using Mshop.Api.Data.models;
using System.Linq.Expressions;

namespace Mshop.Api.Services
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAll();
        Brand? Get(Expression<Func<Brand, bool>> expression);
        Brand Add(Brand brand);
        bool Delete(Guid id);
        bool Edit(Guid id, Brand brand);
    }
}
