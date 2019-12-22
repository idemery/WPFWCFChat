using DM.Core.Interfaces;
using DM.ModuleTfsWorkItems.Views;
using FirstFloor.ModernUI.Presentation;
using System;

namespace DM.ModuleTfsWorkItems.Services
{
    public class LinkGroupService : ILinkGroupService
    {
        public LinkGroup GetLinkGroup()
        {
            LinkGroup linkGroup = new LinkGroup
            {
                DisplayName = "TFS"
            };

            linkGroup.Links.Add(new Link
            {
                DisplayName = "TFS",
                Source = new Uri($"/DM.ModuleTfsWorkItems;component/Views/{nameof(MainView)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "All",
                Source = new Uri($"/DM.ModuleTfsWorkItems;component/Views/{nameof(AllView)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "Bugs",
                Source = new Uri($"/DM.ModuleTfsWorkItems;component/Views/{nameof(BugsView)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "Tasks",
                Source = new Uri($"/DM.ModuleTfsWorkItems;component/Views/{nameof(TasksView)}.xaml", UriKind.Relative)
            });
            linkGroup.Links.Add(new Link
            {
                DisplayName = "Pull Requests",
                Source = new Uri($"/DM.ModuleTfsWorkItems;component/Views/{nameof(PullRequestsView)}.xaml", UriKind.Relative)
            });

            return linkGroup;
        }
    }
}
