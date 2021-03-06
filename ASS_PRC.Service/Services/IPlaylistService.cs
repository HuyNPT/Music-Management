using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASS_PRC.Services.Services
{
    public interface IPlaylistService
    {       
        Task<IList<DTO.PlaylistWebAdmin>> GetPlayListWebAdmin(Enum SortType, bool isPaging, int page, int pageLimitItem, string searchKey);

        Task<bool> PostPlayListWebAdmin(string Name, string ImageUrl, List<Guid> Category,Guid UserId);

        Task<string> DeletePlayListWebAdmin(Guid PlaylistId);

        Task<int> GetPlaylistCount();

        Task<DTO.PlaylistWebAdmin> GetPlaylistById(Guid PlaylistId);

        Task<string> PutPlayListWebAdmin(Guid PlaylistId,string Name, string ImageUrl, List<Guid> Category, Guid UserId);

    }
}
