using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleTfsWorkItems.Services
{
    public interface ITfsSettingsService
    {
        string UserName { get; set; }
        string Password { get; set; }
        string TfsUrl { get; set; }
        string QueryPath { get; set; }
    }
}
