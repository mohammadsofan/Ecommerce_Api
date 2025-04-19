using Mshop.Api.Data.models;
using Mshop.Api.Data;
using Mshop.Api.Services.IService;

namespace Mshop.Api.Services
{
    public class BrandService: Service<Brand>,IBrandService
    {
        private readonly ApplicationDbContext context;

        public BrandService(ApplicationDbContext context):base(context)
        {
            this.context = context;
        }

    }
}
