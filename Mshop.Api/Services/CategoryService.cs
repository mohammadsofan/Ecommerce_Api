using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using Mshop.Api.Services.IService;
using System.Linq.Expressions;

namespace Mshop.Api.Services
{
    public class CategoryService : Service<Category>,ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

    }
}
