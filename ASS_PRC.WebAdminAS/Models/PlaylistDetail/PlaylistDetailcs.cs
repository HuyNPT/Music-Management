using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASS_PRC.WebAdminAS.Models.PlaylistDetail
{
    public class PlaylistDetailcs
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid MediaId { get; set; }
        public int NumericalOrder { get; set; }
    }
}
