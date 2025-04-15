using Mshop.Api.Data.models;
using Mshop.Api.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Mshop.Api.Services
{
    public class BrandService:IBrandService
    {
        private readonly ApplicationDbContext context;

        public BrandService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Brand Add(Brand brand)
        {
            brand.Id = Guid.NewGuid();
            context.Brands.Add(brand);
            context.SaveChanges();
            return brand;
        }

        public bool Delete(Guid id)
        {
            var brand = context.Brands.Find(id);
            if (brand is null)
                return false;
            context.Brands.Remove(brand);
            context.SaveChanges();
            return true;

        }

        public Brand? Get(Expression<Func<Brand, bool>> expression)
        {
            var brand = context.Brands.FirstOrDefault(expression);
            return brand;
        }

        public IEnumerable<Brand> GetAll()
        {
            return context.Brands.ToList();
        }

        public bool Edit(Guid id, Brand brand)
        {
            var brandInDB = context.Brands.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (brandInDB is null)
                return false;
            brand.Id = brandInDB.Id;
            context.Brands.Update(brand);
            context.SaveChanges();
            return true;
        }
    }
}
