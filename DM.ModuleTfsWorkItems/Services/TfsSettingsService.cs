using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleTfsWorkItems.Services
{
    public class TfsSettingsService : ITfsSettingsService
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TfsUrl { get; set; }
        public string QueryPath { get; set; }
    }
}
