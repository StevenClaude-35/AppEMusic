using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Ressources;
using MyMusic.API.Validation;
using MysMusic.Core.Models;
using MysMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artisteService;
        private readonly IMapper _mapperService;

        public ArtistController(IArtistService artistService,IMapper mapperService)
        {
            _artisteService = artistService;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ArtistResourse>>> GetAllArtist()
        {
            var artists = await _artisteService.GetAllArtists();
            var artistResources = _mapperService.Map<IEnumerable<Artist>, IEnumerable<ArtistResourse>>(artists);
            return Ok(artistResources);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistResourse>> GetArtistById(int id)
        {
            try
            {
                var artist = await _artisteService.GetArtistById(id);
                if (artist == null) return BadRequest("Cette artiste n'existe pas");
                var artisteResource = _mapperService.Map<Artist, ArtistResourse>(artist);
                return Ok(artisteResource);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost("")]

        public async Task<ActionResult<ArtistResourse>> CreateArtiste(SaveArtistResourse saveArtistResource)
        {
            //validation depuis le front
            var validation = new SaveArtistResourceValidator();
            var validationResult = await validation.ValidateAsync(saveArtistResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            //Mappage
            var artist = _mapperService.Map<SaveArtistResourse, Artist>(saveArtistResource);
            //Creation artist
            var artistNew = await _artisteService.CreateArtist(artist);
            //Mappage
            var artistResource = _mapperService.Map<Artist, ArtistResourse>(artistNew);
            return Ok(artistResource);

        }
        [HttpPut("")]
        public async Task<ActionResult<ArtistResourse>> UpdateArtiste(int id,SaveArtistResourse saveArtistResource)
        {
            //validation depuis le front
            var validation = new SaveArtistResourceValidator();
            var validationResult = await validation.ValidateAsync(saveArtistResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            //Get artiste by Id
            var artistUpdate = await _artisteService.GetArtistById(id);
            if (artistUpdate == null) return NotFound();

            //Mappage
            var artist = _mapperService.Map<SaveArtistResourse, Artist>(saveArtistResource);

            //Update de l'artist
            await _artisteService.UpdateArtist(artistUpdate, artist);
            //Get Artist by Id
            var artistNew = await _artisteService.GetArtistById(id);
            var artisteNewResource = _mapperService.Map<Artist, ArtistResourse>(artistNew);
            return Ok(artisteNewResource);
            
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteArtist(int id)
        {
            var artist = await _artisteService.GetArtistById(id);
            if (artist == null) return NotFound();
            await _artisteService.DeleteArtist(artist);
            return NoContent();
        }
    }
}
