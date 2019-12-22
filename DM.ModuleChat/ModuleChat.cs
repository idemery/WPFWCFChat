using DM.ModuleChat.Services;
using Prism.Ioc;
using Prism.Modularity;
using SecuredChat;

namespace DM.ModuleChat
{
    public class ModuleChat : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHostActionsService, HostService>();
            containerRegistry.RegisterSingleton<IClientService, ClientService>();
            containerRegistry.RegisterSingleton<IConnectionSettingsService, ConnectionSettingsService>();
            containerRegistry.RegisterSingleton<IProxyService, ProxyService>();
        }
    }
}
