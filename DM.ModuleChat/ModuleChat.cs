using DM.ModuleChat.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace DM.ModuleChat
{
    public class ModuleChat : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IChatCommands, ChatCommands>();
        }
    }
}
