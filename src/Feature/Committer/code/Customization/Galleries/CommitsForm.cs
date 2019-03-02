using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.XmlControls;
using Sitecore.Feature.Committer.Extensions;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using System;
using System.Linq;
using Sitecore.Shell.Applications.ContentManager.Galleries;

namespace Sitecore.Feature.Committer.Customization.Galleries
{
    public class CommitsForm : GalleryForm
    {
        ICommitRepository _commitRepository;
        protected Scrollbox Commits;

        public CommitsForm() : base()
        {
            _commitRepository = new CommitRepository();
        }

        public override void HandleMessage(Sitecore.Web.UI.Sheer.Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            Invoke(message, true);
            message.CancelBubble = true;
            message.CancelDispatch = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                var searchedItems = _commitRepository.GetAllCommits().OrderBy(x => x.ToModelCommit().CommitTime);

                if (searchedItems == null)
                    return;

                foreach (var item in searchedItems)
                {
                    XmlControl control = ControlFactory.GetControl("Gallery.Commits.Option") as XmlControl;
                    Assert.IsNotNull((object)control, typeof(XmlControl));
                    Context.ClientPage.AddControl(Commits, control);
                    var lastOrSelected = _commitRepository.GetSelectedOrLastCommit();

                    control["Header"] = item.DisplayName;
                    control["Description"] = item.Created.ToString();
                    control["ClassName"] = lastOrSelected.ID == item.ID ? "scMenuPanelItemSelected" : "scMenuPanelItem";
                    control["Click"] = $"javascript:return scForm.invoke('gallery:selectcommit(id={item.ID})', event)";
                    control["Icon"] = ThemeManager.GetIconImage(item, 32, 32, "", "");
                }
            }
        }

    }
}