using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Core.Events
{
    public class AppExitEvent : PubSubEvent<int> { }
}
