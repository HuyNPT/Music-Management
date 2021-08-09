using ASS_PRC.WebAdminAS.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASS_PRC.WebAdminAS.Services
{
    public interface IUsersServices
    {
        Task<string> Authenticate(String token);
    }
}
