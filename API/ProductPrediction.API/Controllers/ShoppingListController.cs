using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductPrediction.API.Dtos;
using ProductPrediction.API.Entities;
using ProductPrediction.API.Infrastructure;
using ProductPrediction.API.Utils;

namespace ProductPrediction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        public AppDbContext Db { get; }
        public ShoppingListController(AppDbContext db)
        {
            Db = db;
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetShoppingLists(string userId)
        {
            Guid userGuid;
            if(Guid.TryParse(userId, out userGuid) == false)
            {
                return BadRequest("Invalid userId format.");
            }

            var lists = await Db.ShoppingLists.Where(x => x.UserId == userGuid).ToListAsync();
            
            if (!lists.Any() || lists is null)
            {
                return NoContent();
            }

            var result = lists.Select(l => new ShoppingListResponseDto
            (
                l.Id, 
                l.UserId, 
                l.Title,
                l.Items.Split(',')
                    .Select(x => x.Trim())
                    .ToList())
            );

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShoppingList([FromBody] ShoppingListCreateDto shoppingList)
        {
            Guid userGuid;
            if (Guid.TryParse(shoppingList.UserId, out userGuid) == false)
            {
                return BadRequest("Invalid userId format.");
            }

            var newList = new Entities.ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = userGuid,
                Title = shoppingList.Title,
                Items = string.Join(",", shoppingList.Items.Select(i => i.Trim()))
            };

            await Db.ShoppingLists.AddAsync(newList);
            await Db.SaveChangesAsync();
            
            return Ok("Shopping list created successfully.");
        }

        [HttpPatch("update_items/{listId:guid}")]
        public async Task<IActionResult> UpdateShoppingList(string listId, [FromBody] ShoppingListPatchDto dto)
        {
            Guid listGuid;
            if (Guid.TryParse(listId, out listGuid) == false)
            {
                return BadRequest("Invalid userId format.");
            }

            var existingList = await Db.ShoppingLists.FindAsync(listGuid);
            if (existingList == null)
            {
                return NotFound("Shopping list not found.");
            }

            existingList.Title = dto.Title ?? existingList.Title;
            if (dto.Items != null && dto.Items.Any())
            {
                existingList.Items = string.Join(",", dto.Items.Select(i => i.Trim()));
            }

            Db.ShoppingLists.Update(existingList);
            await Db.SaveChangesAsync();
            return Ok("Shopping list updated successfully.");
        }
    }
}
