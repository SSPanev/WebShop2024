using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebShop2024.Infrastructure.Data.Entities;

namespace WebShop2024.Core.Contracts
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        Category GetCategoryById(int categoryId);
        List<Product> GetProductsByCategory(int categoryId);
    }
}
