using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ASS_PRC.Api.Helpers;
using ASS_PRC.Api.Models.Request;
using ASS_PRC.Api.Models.RequestAdmin;
using ASS_PRC.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASS_PRC.Api.Controllers
{
    [Authorize]
    [Route(Helpers.SettingVersionAPI.ApiVersion)]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
      

        [Authorize(Roles = Role.Admin)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetPlaylistsAdmin([FromQuery] GetPlaylistAdminRequest request)
        {           
            var result =await _playlistService.GetPlayListWebAdmin(request.SortType, request.IsPaging, request.PageNumber, request.PageLimitItem, request.SearchKey);
            return Ok(JsonConvert.SerializeObject(result));
        }
        
       
        [Authorize(Roles = Role.Admin)]
        [HttpPost("admin")]

        public async Task<IActionResult> PostPlaylistAdmin([FromBody] PostPlaylistRequestcs request)
        {
            Guid UserID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);           
            var rs=await _playlistService.PostPlayListWebAdmin(request.PlaylistName, request.ImageUrl, request.Category, UserID);
            if (rs)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpDelete]
        [Route("{PlaylistID}")]
        public async Task<IActionResult> DeletePlaylistAdmin(Guid PlaylistID)
        {           
            var rs=await _playlistService.DeletePlayListWebAdmin(PlaylistID);
            if(rs == "Unauthorized")
            {
                return Unauthorized();
            }
            else if(rs == "Delete fail")
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet("Count")]
        public async Task<IActionResult> GetPlaylistCount()
        {
            return Ok(_playlistService.GetPlaylistCount());
        }
        [AllowAnonymous]
        [HttpGet("GetByID/{PlaylistID}")]       
        public async Task<IActionResult> GetPlaylistByID(Guid PlaylistID)
        {
            var rs=await _playlistService.GetPlaylistById(PlaylistID);
            if (rs == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(JsonConvert.SerializeObject(rs));
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpPut]
        public async Task<IActionResult> PutPlaylist([FromBody] PutPlaylistRequest rq)
        {
            Guid UserID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);           
            var rs = await _playlistService.PutPlayListWebAdmin(rq.PlaylistID, rq.PlaylistName, rq.ImageUrl, rq.Category, UserID);
            if (rs == "Unauthorized")
            {
                return Unauthorized();
            }
            else if (rs == "Fail")
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
    }
}