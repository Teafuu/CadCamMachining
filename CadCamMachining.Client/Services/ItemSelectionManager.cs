using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace CadCamMachining.Client.Services
{
    public class ItemSelectionManager
    {
        private readonly ILogger<ItemSelectionManager> _logger;
        private readonly ItemFacade _itemFacade;

        public Dictionary<string, ItemDto> SelectedItems { get; private set; } = new();

        public event EventHandler<ItemDto> ItemSelectionChanged;

        public ItemSelectionManager(ILogger<ItemSelectionManager> logger, ItemFacade itemFacade, NavigationManager navigationManager)
        {
            _logger = logger;
            _itemFacade = itemFacade;
        }

        public void SelectItem(ItemDto item)
        {
            if (_itemFacade.Items[item.ItemTypeId].Contains(item))
            {
                SelectedItems[item.ItemTypeId] = item;
                ItemSelectionChanged?.Invoke(this, item);
            }
            else
            {
                _logger.LogWarning($"{item} does not exist in facade, cancelling selection");
            }
            
        }

        public void DeselectItem(string itemTypeId)
        {
            SelectedItems[itemTypeId] = null;
        }
    }
}
