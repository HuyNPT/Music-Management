using System.ComponentModel.DataAnnotations;

namespace ASS_PRC.WebAdminAS.Services.Requests
{
    public class AuthenticateModelWebAdmin
    {
        [Required]
        public string IdToken { get; set; }
        
    }
}
