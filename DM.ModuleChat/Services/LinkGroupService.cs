using DM.Core.Interfaces;
using DM.ModuleChat.Views;
using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public class LinkGroupService : ILinkGroupService
    {
        public LinkGroup GetLinkGroup()
        {
            LinkGroup linkGroup = new LinkGroup
            {
                DisplayName = "Chat"
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "Chat",
                Source = new Uri($"/DM.ModuleChat;component/Views/{nameof(MainView)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "Screen Casting",
                Source = new Uri($"/DM.ModuleChat;component/Views/{nameof(ScreenCasting)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "Settings",
                Source = new Uri($"/DM.ModuleChat;component/Views/{nameof(ChatSettings)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "Host",
                Source = new Uri($"/DM.ModuleChat;component/Views/{nameof(ChatHost)}.xaml", UriKind.Relative)
            });

            return linkGroup;
        }
    }
}
