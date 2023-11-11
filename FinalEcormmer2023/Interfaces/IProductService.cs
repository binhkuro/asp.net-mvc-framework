using FinalEcormer2023.Models;

namespace FinalEcormer2023.Interfaces {
	public interface IProductService {
		public Task<IEnumerable<Product>>  GetAll();
		public Task<Product> GetById(int id);
		Task Add(Product model);
		Task Update(Product model);
		Task Delete(Product model);
		Task<IEnumerable<Product>> Search(string? keyword, int? categoryId);
		Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);
        Task<int> GetProductCountByCategoryId(int categoryId);
    }
}
