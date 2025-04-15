using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using Mshop.Api.DTOs.Requests;
using System.Linq.Expressions;

namespace Mshop.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Product Add(Product product, IFormFile file)
        {
            if (file is null || file.Length <= 0)
            {
                throw new InvalidDataException("Invalid File");
            }
            var extension = Path.GetExtension(file.FileName);
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidDataException("Invalid File Format");

            }
            var fileName = Guid.NewGuid() + file.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            product.MainImage = fileName;
            product.Id = Guid.NewGuid();
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public bool Delete(Guid id)
        {
            var product = context.Products.Find(id);
            if (product is null)
            {
                return false;
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", product.MainImage);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return true;
        }

        public bool Edit(Guid id, Product product,IFormFile? file)
        {
            var productInDb = context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
            if (productInDb is null)
            {
                return false;
            }

            if(file is not null && file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidDataException("Invalid File Format");

                }
                var currentPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", product.MainImage);
                if (System.IO.File.Exists(currentPath))
                {
                    System.IO.File.Delete(currentPath);
                }
                var fileName = Guid.NewGuid() + file.FileName;
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);
                using (var stream = System.IO.File.Create(newPath))
                {
                    file.CopyTo(stream);
                }
                product.MainImage = fileName;
            }
            else
            {
                product.MainImage = productInDb.MainImage;
            }
            product.Id = productInDb.Id;
            context.Products.Update(product);
            context.SaveChanges();
            return true;
        }

        public Product? Get(Expression<Func<Product, bool>> expression)
        {
            return context.Products.FirstOrDefault(expression);
        }

        public IEnumerable<Product> GetAll(string? query, int page = 1, int limit=10)
        {
            if (limit <= 0) limit = 10;
            if (page <= 0) page = 1;
            IQueryable<Product> products = context.Products;
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => (p.Name.Contains(query) || p.Description.Contains(query)));
            }

            return products.Skip((page-1) * limit).Take(limit).ToList();
        }
    }
}
