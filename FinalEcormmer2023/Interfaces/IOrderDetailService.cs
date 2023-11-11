using FinalEcormer2023.Models;

namespace FinalEcormer2023.Interfaces
{
    public interface IOrderDetailService
    {
        public Task<IEnumerable<OrderDetail>> GetAll();
        public Task<OrderDetail> GetById(int id);
        Task Add(OrderDetail model);
        Task Update(OrderDetail model);
        Task Delete(OrderDetail model);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(int orderId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByProductId(int productId);
        Task<int> GetOrderDetailCountByProductId(int productId);
        Task<int> GetOrderDetailCountByOrderId(int orderId);
    }
}
