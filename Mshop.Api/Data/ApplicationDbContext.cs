using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data.models;

namespace Mshop.Api.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
    }
}
