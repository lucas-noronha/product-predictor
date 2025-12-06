using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductPrediction.API.AIService.Dtos;
using ProductPrediction.API.AIService.Interfaces;
using ProductPrediction.API.Infrastructure;

namespace ProductPrediction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictionController : ControllerBase
{
    private readonly IPurchasePredictionService _service;
    private readonly AppDbContext _db;

    public PredictionController(IPurchasePredictionService service, AppDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpGet("history/{userId:guid}")]
    public async Task<ActionResult<List<PredictedItemFromHistory>>> FromHistory(Guid userId)
    {
        var purchases = await _db.Purchases
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return Ok(_service.PredictFromHistory(userId, purchases));
    }
}
