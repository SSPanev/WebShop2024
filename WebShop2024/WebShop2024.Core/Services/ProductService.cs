using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

using WebShop2024.Core.Contracts;
using WebShop2024.Infrastructure.Data;
using WebShop2024.Infrastructure.Data.Entities;

namespace WebShop2024.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount)
        {
            Product item = new Product()
            {
                ProductName = name,
                Brand = _context.Brands.Find(brandId),
                Category = _context.Categories.Find(categoryId),

                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount
            };

            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }
        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }
        public List<Product> GetProducts(string searchStringCategoryName, string searchStringBrandName)
        {
            List<Product> products = _context.Products.ToList();

            if (!String.IsNullOrEmpty(searchStringCategoryName) && !String.IsNullOrEmpty(searchStringBrandName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower())
                    && x.Brand.BrandName.ToLower().Contains(searchStringBrandName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringBrandName))
                products = products.Where(x => x.Brand.BrandName.ToLower().Contains(searchStringBrandName.ToLower())).ToList();

            else if (!String.IsNullOrEmpty(searchStringCategoryName))
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower())).ToList();

            return products;
        }
        public bool RemoveById(int productId)
        {
            var product = _context.Products.Find(productId);

            if (product == default(Product))
                return false;

            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }
        public bool Update(int productId, string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount)
        {
            var product = GetProductById(productId);

            if (product == default(Product))
                return false;

            product.ProductName = !string.IsNullOrEmpty(name) ? name : product.ProductName;
            product.Brand = brandId != 0 ? _context.Brands.Find(brandId) : product.Brand;
            product.Category = categoryId != 0 ? _context.Categories.Find(categoryId) : product.Category;

            product.Picture = !string.IsNullOrEmpty(picture) ? picture : product.Picture;
            product.Quantity = quantity != 0 ? quantity : product.Quantity;
            product.Price = price != 0 ? price : product.Price;
            product.Discount = discount != 0 ? discount : product.Discount;

            _context.Update(product);
            return _context.SaveChanges() != 0;
        }
    }
}
