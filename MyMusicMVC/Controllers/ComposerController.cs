using Microsoft.AspNetCore.Mvc;
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
    public class ComposerController : Controller
    {
        private readonly IConfiguration _Config;
        private string URLBase
        {
            get { return _Config.GetSection("URLBase").GetSection("URL").Value; }
        }

        public ComposerController(IConfiguration Conf)
        {
            _Config = Conf;
        }
        public async Task<IActionResult> Index()
        {
            var composerViewModel = new ListComposerViewModel();
            List<Composer> composerList = new  List<Composer>();

            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(URLBase + "Composer"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    composerList = JsonConvert.DeserializeObject<List<Composer>>(apiResponse);
                }
            }
            composerViewModel.ListComposer = composerList;
            return View(composerViewModel);
            
        }
    }
}
