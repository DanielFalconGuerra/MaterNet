namespace MasterNet.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("Demo")]
public class DemoController : ControllerBase
{
    [HttpGet("getstring")]
    public string GetNombre()
    {
        return "MasterNet";
    }
}