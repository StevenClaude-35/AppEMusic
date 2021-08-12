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
    public class ComposerController : ControllerBase
    {
        private readonly IComposerService _composerService;
        private readonly IMapper _mapperService;

        public ComposerController(IComposerService composerService,IMapper maperService)
        {
            _composerService = composerService;
            _mapperService = maperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ComposerResourse>>> GetAllComposers()
        {
            var composers = await  _composerService.GetAllComposers();
            var composerResourses = _mapperService.Map<IEnumerable<Composer>, IEnumerable<ComposerResourse>>(composers);
            return Ok(composerResourses);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ComposerResourse>> GetComposerById(string id)
        {
            try
            {
                var composer = await _composerService.GetComposerById(id);
                if (composer == null) return NotFound();
                var composerResourse = _mapperService.Map<Composer, ComposerResourse>(composer);
                return Ok(composerResourse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("")]

        public async Task<ActionResult<ComposerResourse>> CreateComposer(SaveComposerResourse saveComposerResourse)
        {
            var validation = new SaveComposerResourseValidator();
            var validationResult = await validation.ValidateAsync(saveComposerResourse);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            //Mappage
            var composer = _mapperService.Map<SaveComposerResourse, Composer>(saveComposerResourse);
            //create composer

            var composerNew=await _composerService.Create(composer);
            var composerResourse = _mapperService.Map<Composer, ComposerResourse>(composerNew);
            return Ok(composerResourse);
            
        }

        [HttpPut("")]

        public async Task<ActionResult<ComposerResourse>> UpdateComposer(string id,SaveComposerResourse saveComposerResourse)
        {
            var validation = new SaveComposerResourseValidator();
            var validationResult = await validation.ValidateAsync(saveComposerResourse);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            //si le id existe
            var composerUpdate = _composerService.GetComposerById(id);
            if (composerUpdate == null) return NotFound();
            //Mappage
            var composer = _mapperService.Map<SaveComposerResourse, Composer>(saveComposerResourse);

             _composerService.Update(id, composer);
            //mappage

            var composerNewUpdate = await _composerService.GetComposerById(id);
            var composerResourse = _mapperService.Map<Composer, ComposerResourse>(composerNewUpdate);
            return Ok(composerResourse);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComposerResourse>> DeleteComposeById(string id)
        {
            var composer = await _composerService.GetComposerById(id);
            if (composer == null) return NotFound();
            await _composerService.Delete(id);
            return NoContent();
        }

    }
}
