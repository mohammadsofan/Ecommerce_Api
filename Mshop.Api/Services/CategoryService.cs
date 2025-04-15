using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using System.Linq.Expressions;

namespace Mshop.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Category Add(Category category)
        {
            category.Id = Guid.NewGuid();
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public bool Delete(Guid id)
        {
            var category = context.Categories.Find(id);
            if (category is null)
                return false;
            context.Categories.Remove(category);
            context.SaveChanges();
            return true;

        }

        public Category? Get(Expression<Func<Category, bool>> expression)
        {
            var category = context.Categories.FirstOrDefault(expression);
            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public bool Edit(Guid id, Category category)
        {
            var categoryInDB = context.Categories.AsNoTracking().FirstOrDefault(c=>c.Id==id);
            if (categoryInDB is null) 
                return false;
            category.Id = categoryInDB.Id;
            context.Categories.Update(category);
            context.SaveChanges();
            return true;
        }
    }
}
