using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public interface IConnectionSettingsService
    {
        string UserName { get; set; }
        string IP { get; set; }
        string EndPointAddress { get; set; }
    }
}
