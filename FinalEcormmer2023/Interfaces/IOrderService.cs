using FinalEcormer2023.Models;

namespace FinalEcormer2023.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<Order>> GetAll();
        public Task<Order> GetById(int id);
        Task Add(Order model);
        Task Update(Order model);
        Task Delete(Order model);
    }
}
