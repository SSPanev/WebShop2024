using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebShop2024.Infrastructure.Data.Entities;
using WebShop2024.Core.Contracts;
using WebShop2024.Infrastructure.Data;

namespace WebShop2024.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public List<Product> GetProductsByCategory(int id)
        {
            return _context.Products
                .Where(x => x.CategoryId == id)
                .ToList();
        }
    }
}
