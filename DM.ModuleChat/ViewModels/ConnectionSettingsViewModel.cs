using DM.ModuleChat.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.ViewModels
{
    public class ConnectionSettingsViewModel : BindableBase
    {
        private readonly IConnectionSettingsService connectionSettings;

        public ConnectionSettingsViewModel(IConnectionSettingsService connectionSettings)
        {
            this.connectionSettings = connectionSettings;
            this.connectionSettings.UserName = Environment.UserName;
        }

        public IConnectionSettingsService ConnectionSettings { get { return connectionSettings; } }
    }
}
