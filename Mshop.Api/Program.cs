using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using Mshop.Api.Services;
using Scalar.AspNetCore;

namespace Mshop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = "openapi/{documentName}.json";
                });
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "Api";
                    opt.Theme = ScalarTheme.DeepSpace;
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
