using AutoMapper;
using CadCamMachining.Server.Hub;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Services;
using CadCamMachining.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly ItemService _itemService;
    private readonly IMapper _mapper;

    public ItemsController(ItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    [HttpGet("ByType/{itemTypeId}")]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems(string itemTypeId)
    {
        var items = await _itemService.GetAllByItemTypeAsync(itemTypeId);
        return Ok(_mapper.Map<List<ItemDto>>(items));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(string id)
    {
        var item = await _itemService.GetItemByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ItemDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(ItemDto itemDto)
    {
        try
        {
            var createdItem = await _itemService.CreateItemAsync(itemDto);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(string id, ItemDto itemDto)
    {
        try
        {
            var existingItem = await _itemService.GetItemByIdAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            await _itemService.UpdateItemAsync(id, itemDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        var existingItem = await _itemService.GetItemByIdAsync(id);

        if (existingItem == null)
        {
            return NotFound();
        }

        await _itemService.DeleteItemAsync(existingItem);
        return NoContent();
    }
}