using AssessmentApi.DataAccess;
using AssessmentApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentApi.Controllers;

/// <summary>
/// Endpoints the data entry page uses to list and save items.
/// </summary>
/// <param name="itemData">Data access for the SQLite item table.</param>
[ApiController]
[Route("api/items")]
public sealed class ItemsController(ItemData itemData) : ControllerBase
{
    /// <summary>
    /// GET api/items: returns all stored items as a JSON array.
    /// </summary>
    [HttpGet]
    public ActionResult<List<Item>> GetAll() => Ok(itemData.GetItems());

    /// <summary>
    /// POST api/items: saves a new item sent as JSON in the request body.
    /// </summary>
    /// <param name="item">The item to save; ID, name and description are all required.</param>
    /// <returns>
    /// 200 with the saved item, 400 if any field is blank,
    /// or 409 if an item with the same ID already exists.
    /// </returns>
    [HttpPost]
    public ActionResult<Item> Add(Item item)
    {
        if (string.IsNullOrWhiteSpace(item.Id) ||
            string.IsNullOrWhiteSpace(item.Name) ||
            string.IsNullOrWhiteSpace(item.Description))
        {
            return BadRequest("Item ID, Item Name, and Item Description are required.");
        }

        if (!itemData.AddItem(item))
        {
            return Conflict($"An item with ID '{item.Id}' already exists.");
        }

        return Ok(item);
    }
}
