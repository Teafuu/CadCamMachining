using AspNetCore.Identity.MongoDbCore.Infrastructure;
using CadCamMachining.Server.Models.Layouts;
using CadCamMachining.Server.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CadCamMachining.Server.Repositories
{
    public class LayoutRepository : ILayoutRepository
    {
        private readonly IMongoCollection<Layout> _layouts;

        public LayoutRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _layouts = database.GetCollection<Layout>("Layouts");
        }

        public async Task<List<Layout>> GetAllAsync()
        {
            return await _layouts.Find(layout => true).ToListAsync();
        }

        public async Task<Layout> GetByIdAsync(string id)
        {
            return await _layouts.Find<Layout>(layout => layout.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Layout layout)
        {
            await _layouts.InsertOneAsync(layout);
        }

        public async Task UpdateAsync(string id, Layout layout)
        {
            await _layouts.ReplaceOneAsync(layout => layout.Id == id, layout);
        }

        public async Task DeleteAsync(string id)
        {
            await _layouts.DeleteOneAsync(layout => layout.Id == id);
        }
    }
}
