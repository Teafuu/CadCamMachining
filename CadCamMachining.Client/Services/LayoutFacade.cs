using CadCamMachining.Shared.Models;
using System.Net.Http.Json;
using CadCamMachining.Client.Components;

namespace CadCamMachining.Client.Services
{
    public class LayoutFacade
    {
        private readonly ILogger<LayoutFacade> _logger;
        private readonly HttpClient _httpClient;

        public List<LayoutDto> Layouts { get; set; } = new();

        public event EventHandler<List<LayoutDto>> LayoutsUpdated;

        public event EventHandler<LayoutDto> LayoutSelected;

        public LayoutFacade(ILogger<LayoutFacade> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public void SelectLayout(LayoutDto layout)
        {
            LayoutSelected?.Invoke(this, layout);
        }

        public async Task CreateLayout(LayoutDto layout)
        {
            var result = await _httpClient.PostAsJsonAsync("api/layouts", layout);
            result.EnsureSuccessStatusCode();

            var createdLayout = await result.Content.ReadFromJsonAsync<LayoutDto>();
            if (createdLayout is null)
            {
                throw new InvalidOperationException("Layout creation failed");
            }
            Layouts.Add(createdLayout);
            LayoutsUpdated?.Invoke(this, Layouts);
        }

        public async Task UpdateLayout(LayoutDto layout)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/layouts/{layout.Id}", layout);
            result.EnsureSuccessStatusCode();

            LayoutsUpdated?.Invoke(this, Layouts);
        }

        public async Task RemoveLayout(LayoutDto layout)
        {
            var result = await _httpClient.DeleteAsync($"api/Layouts/{layout.Id}");
            result.EnsureSuccessStatusCode();

            Layouts.Remove(layout);
            LayoutsUpdated?.Invoke(this, Layouts);
        }

        public async Task<List<LayoutDto>> GetLayouts()
        {
            var result = await _httpClient.GetAsync("api/Layouts");
            
            result.EnsureSuccessStatusCode();
            Layouts.Clear();

            var layouts = await result.Content.ReadFromJsonAsync<List<LayoutDto>>();

            if (layouts is null)
            {
                return new List<LayoutDto>();
            }

            Layouts.AddRange(layouts);
            LayoutsUpdated?.Invoke(this, Layouts);

            return Layouts;
        }
    }
}
