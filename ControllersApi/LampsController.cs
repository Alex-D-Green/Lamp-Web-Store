using System.Threading.Tasks;

using LampWebStore.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LampWebStore.ControllersApi
{
    /// <summary>
    /// An example of simple WebApiController for CRUD operations with lamps.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController] //WebApiController 
    public class LampsController: ControllerBase
    {
        private readonly LampsContext db;


        public LampsController(LampsContext db) //DI...
        {
            this.db = db;
        }


        //GET api/lamps
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await db.Lamps.AsNoTracking().ToArrayAsync());
        }

        //GET api/lamps/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Lamp lamp = await db.Lamps.FirstOrDefaultAsync(o => o.Id == id);
            
            if(lamp == null)
                return NotFound();
            
            return new ObjectResult(lamp);
        }

        //POST api/lamps
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Lamp lamp)
        {
            if(lamp is null)
                return BadRequest();

            //Here should be lamp check logic and that's why it should be in Entities types, 
            //not in Validation attributes or MVC controllers...

            db.Lamps.Add(lamp);
            await db.SaveChangesAsync();

            return Ok(lamp);
        }

        //PUT api/lamps/
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Lamp lamp)
        {
            if(lamp is null)
                return BadRequest();

            //Here should be lamp check logic and that's why it should be in Entities types, 
            //not in Validation attributes or MVC controllers...

            if(!await db.Lamps.AnyAsync(x => x.Id == lamp.Id))
                return NotFound();

            db.Update(lamp);
            await db.SaveChangesAsync();

            return Ok(lamp);
        }

        //DELETE api/lamps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Lamp lamp = await db.Lamps.FirstOrDefaultAsync(o => o.Id == id);

            if(lamp is null)
                return NotFound();

            db.Lamps.Remove(lamp);
            await db.SaveChangesAsync();
            
            return Ok(lamp);
        }
    }
}