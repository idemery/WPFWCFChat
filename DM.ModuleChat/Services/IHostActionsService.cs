using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public interface IHostActionsService
    {
        void Start();
        void Stop();
        string EndPointAddress { get; }
    }
}
