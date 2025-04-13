using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrderApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public OrdersController(OrderDbContext context)
        {
            _context = context;
        }

        // 获取所有订单
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails).ToListAsync();
            return Ok(orders);
        }

        // 获取单个订单
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                                              .FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // 创建新订单
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order newOrder)
        {
            if (newOrder == null)
            {
                return BadRequest();
            }
            foreach (var item in newOrder.OrderDetails)
            {
                // 计算并设置 TotalAmount
                item.TotalAmount = item.Quantity * item.UnitPrice;
            }
            newOrder.CalculateTotalAmount();  // 计算总金额
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.ID }, newOrder);
        }

        // 更新订单
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            var existingOrder = await _context.Orders.Include(o => o.OrderDetails)
                                                      .FirstOrDefaultAsync(o => o.ID == id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.Customer = updatedOrder.Customer;
            existingOrder.OrderDetails = updatedOrder.OrderDetails;
            existingOrder.CalculateTotalAmount(); // 更新总金额
            await _context.SaveChangesAsync();

            return NoContent(); // 无内容响应，表示成功
        }

        // 删除订单
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent(); // 无内容响应，表示删除成功
        }
    }
}
