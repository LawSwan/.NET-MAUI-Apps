using Microsoft.AspNetCore.Mvc;
using RemoteDataStorage.DataAccess;
using RemoteDataStorage.Models;

namespace RemoteDataStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Person> Get()
    {
        return new PersonData().GetPeople();
    }

    [HttpGet("{id:int}")]
    public ActionResult<Person> Get(int id)
    {
        var person = new PersonData().GetPerson(id);
        if (person is null)
        {
            return NotFound();
        }

        return person;
    }

    [HttpPost]
    public ActionResult<Person> Post([FromBody] Person person)
    {
        var data = new PersonData();
        data.SavePerson(person);

        return CreatedAtAction(nameof(Get), new { id = person.ID }, person);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var data = new PersonData();
        var person = data.GetPerson(id);

        if (person is null)
        {
            return NotFound();
        }

        data.DeletePerson(person);
        return NoContent();
    }
}