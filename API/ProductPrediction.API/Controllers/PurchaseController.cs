using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductPrediction.API.Infrastructure;
using ProductPrediction.API.Utils;

namespace ProductPrediction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext Db;

        public PurchaseController(AppDbContext db)
        {
            Db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPurchases(string userId)
        {
            Guid userGuid;
            if (Guid.TryParse(userId, out userGuid) == false)
            {
                return BadRequest("Invalid userId format.");
            }
            var purchases = await Db.Purchases
                .Where(p => p.UserId == userGuid)
                .ToListAsync();
            if (!purchases.Any())
            {
                return NoContent();
            }
            var result = purchases.Select(p => new Dtos.PurchaseResponseDto
            (
                p.Id,
                p.UserId,
                p.Date,
                p.Items.Split(',')
                    .Select(item => item.Trim())
                    .ToList()
            ));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody] Dtos.PurchaseCreateDto purchaseInfo)
        {
            Guid userGuid;
            if (Guid.TryParse(purchaseInfo.UserId, out userGuid) == false)
            {
                return BadRequest("Invalid userId format.");
            }
            var newPurchase = new Entities.Purchase
            {
                Id = Guid.NewGuid(),
                UserId = userGuid,
                Date = purchaseInfo.Date,
                Items = string.Join(',', purchaseInfo.Items.Select(i => StringNormalizer.NormalizeItem(i.Trim())))
            };

            Db.Purchases.Add(newPurchase);
            await Db.SaveChangesAsync();
            return Ok("Purchase recorded successfully.");
        }

    }
}
