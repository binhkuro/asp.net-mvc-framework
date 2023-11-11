using FinalEcormer2023.Data;
using FinalEcormer2023.Interfaces;
using FinalEcormer2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalEcormer2023.Service
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }

        public async Task Add(Order model)
        {
            _context.Orders.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Order model)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(c => c.Id == model.Id);

            if (existingOrder != null)
            {
                existingOrder.Total = model.Total;
                existingOrder.ShipName = model.ShipName;
                existingOrder.ShipAddress = model.ShipAddress;
                existingOrder.ShipEmail = model.ShipEmail;
                existingOrder.ShipPhoneNumber = model.ShipPhoneNumber;

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order not found for update");
            }
        }

        public async Task Delete(Order model)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == model.Id);

            if (existingOrder != null)
            {
                _context.Orders.Remove(existingOrder);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order not found for deletion");
            }
        }

        public async Task<Order> GetById(int id)
        {
            var order = await GetAll();
            return order.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
