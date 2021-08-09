using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASS_PRC.WebAdminAS.Models;

namespace ASS_PRC.WebAdminAS.Services
{
    public interface IPlaylistsServiceWebAdmin
    {
        Task<List<PlaylistResponse>> getPlaylist(int Page,string jwt);

        Task<bool> PostPlaylist(string PlaylistName, string ImageUrl, List<Guid> Category, string jwt);
        Task<int> GetPlaylistCount(string jwt);
        Task<PlaylistResponse> GetPlaylistById(string PlaylistId);
        Task<bool> DeletePlaylist(string PlaylistID, string jwt);

        Task<string> PutPlayListWebAdmin(string PlaylistId, string Name, string ImageUrl, List<Guid> Category, string jwt);
    }
        
}
