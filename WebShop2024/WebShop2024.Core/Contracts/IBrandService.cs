using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebShop2024.Infrastructure.Data.Entities;

namespace WebShop2024.Core.Contracts
{
    public interface IBrandService
    {
        List<Brand> GetBrands();
        Brand GetBrandById(int brandId);
        List<Product> GetProductsByBrand(int brandId);
    }
}
