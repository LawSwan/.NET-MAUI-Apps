using Microsoft.AspNetCore.Mvc;

namespace BasicAuthWS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    [BasicAuthentication]
    public IEnumerable<string> Get()
    {
        return ["value1", "value2"];
    }
}