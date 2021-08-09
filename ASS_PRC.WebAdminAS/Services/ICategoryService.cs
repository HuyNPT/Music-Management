using ASS_PRC.WebAdminAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASS_PRC.WebAdminAS.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> getCategory();
    }
}
