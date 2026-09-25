using AssessmentApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentApi.Controllers;

[ApiController]
[Route("api/items")]
public sealed class ItemsController(ItemStore store) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<Item>> GetAll() => Ok(store.GetAll());

    [HttpPost]
    public ActionResult<Item> Add(Item item)
    {
        if (string.IsNullOrWhiteSpace(item.Id) ||
            string.IsNullOrWhiteSpace(item.Name) ||
            string.IsNullOrWhiteSpace(item.Description))
        {
            return BadRequest("Item ID, Item Name, and Item Description are required.");
        }

        return Ok(store.Add(item));
    }
}