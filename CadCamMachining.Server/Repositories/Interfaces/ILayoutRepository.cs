using CadCamMachining.Server.Models.Layouts;

namespace CadCamMachining.Server.Repositories.Interfaces
{
    public interface ILayoutRepository
    {
        Task<List<Layout>> GetAllAsync();
        Task<Layout> GetByIdAsync(string id);
        Task CreateAsync(Layout layout);
        Task UpdateAsync(string id, Layout layout);
        Task DeleteAsync(string id);
    }
}
