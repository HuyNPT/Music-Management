using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASS_PRC.WebAdminAS.Models;
using ASS_PRC.WebAdminAS.Models.PlaylistModel;
using ASS_PRC.WebAdminAS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASS_PRC.WebAdminAS.Controllers
{
    [Authorize]
    public class CreatePlaylistController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPlaylistsServiceWebAdmin _playlistsServiceWebAdmin;
        public CreatePlaylistController(ICategoryService categoryService,IPlaylistsServiceWebAdmin playlistsServiceWebAdmin)
        {
            _categoryService = categoryService;
            _playlistsServiceWebAdmin = playlistsServiceWebAdmin;
        }
        public async Task<IActionResult> Index()
        {           
            var listCategory =await _categoryService.getCategory();
            CreatePlaylist a = new CreatePlaylist()
            {
                Category = listCategory,

            };
            return View(a);
        }
        public async Task<IActionResult> Create(CreatePlaylist model)
        {
            var sessions = HttpContext.Session.GetString("Token");
            List<Guid> cate = new List<Guid>();
            Request.Form["category"].ToList().ForEach(e =>
            {
                cate.Add(new Guid(e.ToString()));
            });            
            var rs= await _playlistsServiceWebAdmin.PostPlaylist(model.PlaylistName, model.ImageUrl, cate, sessions);
            if (rs)
            {
                return RedirectToAction("Index", "Playlists");
            }
            else
            {
                return Content("Error");
            }

        }
    }
}
