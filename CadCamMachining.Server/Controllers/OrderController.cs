using AutoMapper;
using CadCamMachining.Server.Data;
using CadCamMachining.Server.Hub;
using CadCamMachining.Server.Models;
using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CadCamMachining.Server.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<OrderHub, IOrderHub> _hubContext;

    public OrderController(ApplicationDbContext context, IMapper mapper, IHubContext<OrderHub, IOrderHub> hubContext)
    {
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    // GET: api/Order
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(x => x.Customer)
            .Include(x => x.Contact)
            .Include(x => x.Articles)
            .Include(x => x.Status)
            .ToListAsync();

        return Ok(_mapper.Map<List<OrderDto>>(orders));
    }

    // GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
    {
        var order = await _context.Orders
            .Include(x => x.Customer)
            .Include(x => x.Articles)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderDto>(order));
    }

    // PUT: api/Order/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(Guid id, OrderDto orderDto)
    {
        if (id != orderDto.Id)
        {
            return BadRequest();
        }

        var order = _mapper.Map<Order>(orderDto);
        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendOrderUpdate(new List<OrderDto> { orderDto });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Order
    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        orderDto.Id = order.Id; // Ensure the ID is updated
        await _hubContext.Clients.All.SendOrderUpdate(new List<OrderDto> { orderDto });

        return CreatedAtAction("GetOrder", new { id = order.Id }, orderDto);
    }

    // DELETE: api/Order/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendOrderUpdate(new List<OrderDto> { new OrderDto { Id = id } });

        return NoContent();
    }

    private bool OrderExists(Guid id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }
}