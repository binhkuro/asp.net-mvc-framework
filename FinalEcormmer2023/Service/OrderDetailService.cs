using FinalEcormer2023.Data;
using FinalEcormer2023.Interfaces;
using FinalEcormer2023.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalEcormer2023.Service
{
    public class OrderDetailService : IOrderDetailService 
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(OrderDetail model)
        {
            _context.OrderDetails.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(OrderDetail model)
        {
            var existingOrderDetail = await _context.OrderDetails.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (existingOrderDetail != null)
            {
                _context.OrderDetails.Remove(existingOrderDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order detail not found for deletion");
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAll()
        {
            var orderdetails = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(y => y.Product)
                .ToListAsync();
            return orderdetails;
        }

        public async Task<OrderDetail> GetById(int id)
        {
            var orderDetail = await GetAll();
            return orderDetail.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task Update(OrderDetail model)
        {
            var existingOrderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.Id == model.Id);

            if (existingOrderDetail != null)
            {
                existingOrderDetail.Quantity = model.Quantity;

                _context.OrderDetails.Update(existingOrderDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order Detail not found for update");
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetailsInOrder = await _context.OrderDetails
                .Include(x => x.Product)
                .Where(p => p.OrderId == orderId)
                .ToListAsync();

            return orderDetailsInOrder;
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByProductId(int productId)
        {
            var orderDetailsInProduct = await _context.OrderDetails
                .Where(p => p.ProductId == productId)
                .ToListAsync();

            return orderDetailsInProduct;
        }

        public async Task<int> GetOrderDetailCountByProductId(int productId)
        {
            return await _context.OrderDetails.CountAsync(p => p.ProductId == productId);
        }

        public async Task<int> GetOrderDetailCountByOrderId(int orderId)
        {
            return await _context.OrderDetails.CountAsync(p => p.OrderId == orderId);
        }
    }
}
