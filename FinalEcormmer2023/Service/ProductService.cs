using FinalEcormer2023.Data;
using FinalEcormer2023.Interfaces;
using FinalEcormer2023.Models;
using FinalEcormmer2023.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalEcormer2023.Service {
	public class ProductService : IProductService {
		private readonly ApplicationDbContext _context;
		public ProductService(ApplicationDbContext context) {
			_context = context;
		}

		public async Task Add(Product model) {
            _context.Products.Add(model);
            await _context.SaveChangesAsync();
        }

		public async Task Delete(Product model) {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Product not found for deletion");
            }
        }

		public async Task<IEnumerable<Product>> GetAll() {
			var products = await _context.Products.Include(x => x.category).ToListAsync();
			return products;
		}

		public async Task<Product> GetById(int id) {
			var product = await GetAll();
			return product.Where(x => x.Id == id).FirstOrDefault();
		}

		public async Task<IEnumerable<Product>> Search(string? keyword, int? categoryId = 0) {
			var products = await _context.Products.Include(X => X.category).ToListAsync();
			if (!String.IsNullOrEmpty(keyword)) {
				products = products.Where(p => p.Name.ToLower().Contains(keyword.ToLower())).ToList();
			}
			if (categoryId != 0) {
				products = products.Where(p => p.CategoryId.Equals(categoryId)).ToList();
			}

			return products;
		}

		public async Task Update(Product model) {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = model.Name;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Product not found for update");
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            var productsInCategory = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            return productsInCategory;
        }

        public async Task<int> GetProductCountByCategoryId(int categoryId)
        {
            return await _context.Products.CountAsync(p => p.CategoryId == categoryId);
        }
    }
}
