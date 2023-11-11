using FinalEcormer2023.Models;
using FinalEcormmer2023.Models;

namespace FinalEcormer2023.Interfaces {
	public interface ICategoryService {
		public Task<IEnumerable<Category>> GetAll();
		public Task<Category> GetById(int id);
		Task Add(Category model);
		Task Update(Category model);
		Task Delete(Category model);
    }
}
