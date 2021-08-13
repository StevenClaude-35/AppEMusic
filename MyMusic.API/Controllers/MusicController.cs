using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IArtistService _artisteService;
        private readonly IMapper _mapperService;

        public MusicController(IMusicService musicService, IMapper mapperService, IArtistService artisteService)
        {
            _musicService = musicService;
            _mapperService = mapperService;
            _artisteService = artisteService;
        }
        [HttpGet("")]

        public async Task<ActionResult<IEnumerable<MusicResourse>>> GetAllMusic()
        {
            try
            {
                var musics = await _musicService.GetAllWithArtist();
                var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResourse>>(musics);
                return Ok(musicResources);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

           
           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicResourse>> GetMusicById(int id)
        {
            try
            {
                var music = await _musicService.GetMusicById(id);
                if (music == null) return NotFound();
                var musicResource = _mapperService.Map<Music, MusicResourse>(music);
                return Ok(musicResource);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MusicResourse>> CreateMusic(SaveMusicResourse saveMusicResource)
        {
            //Get Current user
            var userId = User.Identity.Name;
            //Validation des données d'entrée
            var validation = new SaveMusicResourceValidator();
            var validationResult = await validation.ValidateAsync(saveMusicResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            //Mappage
            var music = _mapperService.Map<SaveMusicResourse, Music>(saveMusicResource);
            //Creation de Music
            var newMusic = await _musicService.CreateMusic(music);
            //Mappage
            var musicResource = _mapperService.Map<Music, MusicResourse>(newMusic);
            return Ok(musicResource);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<MusicResourse>> UpdateMusic(int id,SaveMusicResourse saveMusicResource)
        {
            //Validation
            var validation = new SaveMusicResourceValidator();
            var validationResult = await validation.ValidateAsync(saveMusicResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            //si la music existe depuis le id
            var musicUpdate = await _musicService.GetMusicById(id);
            if (musicUpdate == null) return BadRequest("La music n'existe pas ");
            var music = _mapperService.Map<SaveMusicResourse, Music>(saveMusicResource);
            await _musicService.UpdateMusic(musicUpdate, music);
            var musicUpdateNew = await _musicService.GetMusicById(id);
            var musicResourceUpdate = _mapperService.Map<Music, SaveMusicResourse>(musicUpdateNew);
            
            return Ok(musicResourceUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMusic(int id)
        {
            var music = await _musicService.GetMusicById(id);
            if (music == null) return BadRequest("La music n'existe pas");

            await _musicService.DeleteMusic(music);
            return NoContent();
        }

        [HttpGet("Artist/id")]
        public async Task<ActionResult<IEnumerable<MusicResourse>>> GetAllMusicByIdArtist(int id)
        {
            try
            {
                var musics = await _musicService.GetMusicsByArtistId(id);
                if (musics == null) return BadRequest("Pour cet artist il n'ya pas de music ");
                var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResourse>>(musics);
                return Ok(musicResources);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }
    }
}
