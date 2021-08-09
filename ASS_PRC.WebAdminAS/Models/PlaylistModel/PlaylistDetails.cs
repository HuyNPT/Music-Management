using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASS_PRC.WebAdminAS.Models.PlaylistModel
{
    public class PlaylistDetails
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid MusicId { get; set; }
        public int? NumericalOrder { get; set; }
    }
}
