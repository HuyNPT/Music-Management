
using ASS_PRC.WebAdminAS.Models.Media;
using System.Collections.Generic;

namespace ASS_PRC.WebAdminAS.Models.PlaylistModel
{
    public class Playlist
    {
        public PlaylistResponse PlaylistSelected { get; set; }
        public List<MediaModel> ListMedia { get; set; }
    }
}
