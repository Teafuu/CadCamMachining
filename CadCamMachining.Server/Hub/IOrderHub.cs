using CadCamMachining.Shared.Models;

namespace CadCamMachining.Server.Hub
{
    public interface IOrderHub
    {
        Task SendOrderUpdate(ICollection<OrderDto> updatedOrders);
    }
}
