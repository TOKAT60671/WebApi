﻿using WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

    namespace WebApi.Controllers
    {
        [ApiController]
        [Route("data/saves")]
        [Authorize]
        public class DataController(IDatabaseRepository repo) : ControllerBase
        {
        private Guid GetUserIdFromToken()
        {
            var idString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
            if (Guid.TryParse(idString, out var guid))
                return guid;
            throw new UnauthorizedAccessException("User ID not found in access token.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorld([FromBody] CreateSaveGameRequest dto)
        {
            Guid userId = GetUserIdFromToken();

            var saveGame = new SaveGameDto
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                UserId = userId,
                Slot = dto.Slot
            };

            await repo.InsertNewSaveGame(saveGame, userId);
            return CreatedAtAction(nameof(GetWorld), new { id = saveGame.Id }, saveGame);
        }

            [HttpGet]
        public async Task<ActionResult<IEnumerable<SaveGameDto>>> GetWorlds()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { reason = "User ID not found in access token." });

            var worlds = await repo.GetSaveGames(userId);
            return Ok(worlds);
        }

            [HttpGet("{id}")]
        public async Task<ActionResult> GetWorld(Guid id)
        {
            try
            {
                var userId = GetUserIdFromToken();

                // Get all worlds for this user
                var worlds = await repo.GetSaveGames(userId);
                var world = worlds.FirstOrDefault(w => w.Id == id);

                if (world == null)
                {
                    return NotFound(new
                    {
                        World = (SaveGameDto?)null,
                        Objects = (IEnumerable<GObjectDto>?)null,
                        Error = "Save slot not found."
                    });
                }

                var objects = await repo.GetgObjects(id);

                return Ok(new
                {
                    World = world,
                    Objects = objects,
                    Error = (string?)null
                });
            }
            catch (Exception ex)
            {
                // Log ex if needed
                return StatusCode(500, new
                {
                    World = (SaveGameDto?)null,
                    Objects = (IEnumerable<GObjectDto>?)null,
                    Error = "An unexpected error occurred: " + ex.Message
                });
            }
        }

            [HttpPost("{id}/objects")]
            public async Task<IActionResult> AddObjects(Guid id, [FromBody] GObjectDto[] gObjects)
            {
                await repo.InsertgObjects(gObjects, id);
                return Created();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteWorld(Guid id)
            {
                await repo.DeleteSaveGame(id);
                return NoContent();
            }
        }
    }

