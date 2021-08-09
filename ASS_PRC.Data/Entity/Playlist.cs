using System;
using System.Collections.Generic;

namespace ASS_PRC.Data.Entity
{
    public partial class Playlist
    {
        public Playlist()
        {
            CategoryPlaylist = new HashSet<CategoryPlaylist>();
            PlaylistDetail = new HashSet<PlaylistDetail>();
        }

        public Guid Id { get; set; }
        public string PlaylistName { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public string ImageUrl { get; set; }

        public virtual Account CreateByNavigation { get; set; }
        public virtual ICollection<CategoryPlaylist> CategoryPlaylist { get; set; }
        public virtual ICollection<PlaylistDetail> PlaylistDetail { get; set; }
    }
}
