using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public class ConnectionSettingsService : IConnectionSettingsService
    {
        public string UserName { get; set; }
        public string IP { get; set; }
        public string EndPointAddress { get; set; }
    }
}
