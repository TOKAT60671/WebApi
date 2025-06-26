using WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Saves")]
    [Authorize]
    public class DataController(IDatabaseRepository repo, IAuthenticationService auth) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateWorld([FromBody] SaveGameDto saveGame)
        {
            await repo.InsertNewSaveGame(saveGame, auth.GetCurrentAuthenticatedUserID());
            return CreatedAtAction(nameof(GetWorld), new { id = saveGame.Id }, saveGame);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaveGameDto>>> GetWorlds()
        {
            var result = await repo.GetSaveGames(auth.GetCurrentAuthenticatedUserID());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaveGameDto>> GetWorld(Guid id)
        {
            var worlds = await repo.GetSaveGames(auth.GetCurrentAuthenticatedUserID());
            var world = worlds.FirstOrDefault(w => w.Id == id);
            if (world == null)
                return NotFound();

            var objects = await repo.GetgObjects(id.ToString());
            return Ok(new { World = world, Objects = objects });
        }

        [HttpPost("{id}/objects")]
        public async Task<IActionResult> AddObjects(Guid id, [FromBody] GObjectDto[] gObjects)
        {
            await repo.InsertgObjects(gObjects, id.ToString());
            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorld(Guid id)
        {
            await repo.DeleteSaveGame(id.ToString());
            return NoContent();
        }
    }
}
