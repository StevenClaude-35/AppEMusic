using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyMusicMVC.Controllers
{
    public class MusicController : Controller
    {
        private readonly IConfiguration _Conf;
        private string URLBase
        {
            get { return _Conf.GetSection("BaseURL").GetSection("URL").Value; }
        }

        public MusicController(IConfiguration Conf)
        {
            _Conf = Conf;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddMusic()
        {
            var musicViewModel = new MusicViewModel();
            List<Artist> artistList = new List<Artist>();

            using(var httpclient=new HttpClient())
            {
                using(var response=await httpclient.GetAsync(URLBase + "Artist"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    artistList = JsonConvert.DeserializeObject<List<Artist>>(apiResponse);
                }
            }
            musicViewModel.ArtistList = new SelectList(artistList, "Id", "Name");
            return View(musicViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> AddMusic(MusicViewModel musicViewModel)
        {
            if (ModelState.IsValid)
            {
                using(var client=new HttpClient())
                {
                    var music = new Music()
                    {
                        ArtistId = int.Parse(musicViewModel.ArtistId),
                        Name = musicViewModel.Music.Name
                    };
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "You must be authenticate";
                        return View(musicViewModel);
                    }
                    string stringData = JsonConvert.SerializeObject(music);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PostAsync(URLBase + "Music", contentData);
                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(musicViewModel);
                }
            }
            return View(musicViewModel);
        }
        
    }
}
