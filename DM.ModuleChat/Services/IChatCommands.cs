using DM.ModuleChat.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public interface IChatCommands
    {
        CompositeCommand Connect { get; }
        CompositeCommand Disconnect { get; }
        CompositeCommand SendMessage { get; }
        CompositeCommand SendScreenShot { get; }
        CompositeCommand StartScreenCasting { get; }
        CompositeCommand StopScreenCasting { get; }
    }
}
