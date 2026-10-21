using Microsoft.AspNetCore.Mvc;
using BackEnd_cs.Models;

namespace BackEnd_cs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostItemsController : ControllerBase
    {
        private static List<LostItem> itemsList = new List<LostItem>
        {
            new LostItem { Id = 1, Title = "Izgubljen novčanik", Description = "Crni kožni", Location = "Škola", Type = "Lost" },
            new LostItem { Id = 2, Title = "Nađeni ključevi", Description = "Sa plavim priveskom", Location = "Hol", Type = "Found" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<LostItem>> GetAll() => Ok(itemsList);

        [HttpPost]
        public ActionResult<LostItem> Create([FromBody] LostItem item)
        {
            item.Id = itemsList.Count > 0 ? itemsList.Max(i => i.Id) + 1 : 1;
            itemsList.Add(item);
            return Ok(item);
        }
    }
}