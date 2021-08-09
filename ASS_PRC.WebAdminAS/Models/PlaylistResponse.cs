using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASS_PRC.WebAdminAS.Models
{
    public class PlaylistResponse
    {
        public Guid Id { get; set; }
        [Required]
        public string PlaylistName { get; set; }       
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }      
        [Required]
        public string ImageUrl { get; set; }              
        public ICollection<CategoryPlaylist> CategoryPlaylists { get; set; }

    }
}
