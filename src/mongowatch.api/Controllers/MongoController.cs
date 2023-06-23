using Microsoft.AspNetCore.Mvc;
using mongowatch.api.Data;
using mongowatch.api.Models;

namespace mongowatch.api.Controllers;

[ApiController]
[Route("[controller]")]
public class MongoController : ControllerBase
{

    private readonly IMongoData _mongoData;
    public MongoController(IMongoData mongoData)
    {
        _mongoData = mongoData;
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        _mongoData.AddPosition(new Models.Position { Name = "Monguinho" });

        return Ok();
    }

    [HttpGet("positions")]
    public IEnumerable<Position> ListAllPositions()
    {
        return _mongoData.ListAllPositions();
    }

    [HttpGet("expirations")]
    public IEnumerable<PositionExpiration> ListAllPositionsExpiration()
    {
        return _mongoData.ListAllPositionsExpiration();
    }
}
