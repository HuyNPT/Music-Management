using System;
using System.Collections.Generic;

namespace ASS_PRC.Data.Entity
{
    public partial class Account
    {
        public Account()
        {
            Media = new HashSet<Media>();
            Playlist = new HashSet<Playlist>();
        }

        public Guid Id { get; set; }
        public string FirebaseProvider { get; set; }
        public string FirebaseUid { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual ICollection<Media> Media { get; set; }
        public virtual ICollection<Playlist> Playlist { get; set; }
    }
}
