using FinalEcormer2023.Data;
using FinalEcormer2023.Interfaces;
using FinalEcormer2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalEcormer2023.Service
{
	public class CategoryService : ICategoryService
	{
		private readonly ApplicationDbContext _context;

		public CategoryService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Category>> GetAll()
		{
			var categories = await _context.Categories.ToListAsync();
			return categories;
		}

        public async Task Add(Category model)
        {
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category model)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = model.Name;

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Category not found for update");
            }
        }

        public async Task Delete(Category model)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == model.Id);

            if (existingCategory != null)
            {
                _context.Categories.Remove(existingCategory);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Category not found for deletion");
            }
        }

        public async Task<Category> GetById(int id)
		{
			var category = await GetAll();
			return category.Where(x => x.Id == id).FirstOrDefault();
		}
    }
}
