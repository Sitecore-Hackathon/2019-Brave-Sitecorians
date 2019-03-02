using Sitecore.Data.Items;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using System;
using Sitecore.Feature.Committer.Models;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Extensions;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Shell.Applications.ContentManager.Sidebars;

namespace Sitecore.Feature.Committer.Customization.ContentEditorForms
{

    // decompiled from Sitecore to override methods
    public class CommitterContentEditorForm : ContentEditorForm
    {
        ICommitRepository _commitRepository;
        const string CommitterStripName = "CommitterStrip";

        public CommitterContentEditorForm()
        {
            _commitRepository = new CommitRepository();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override Sidebar GetSidebar()
        {
            Tree result = new Tree();
            result.ID = "Tree";
            result.DataContext = new DataContext()
            {
                DataViewName = "Master"
            };
            return (Sidebar)Assert.ResultNotNull<Tree>(result);
        }

        public void UnchangedItems_Click()
        {
            bool checkCurrentCommit;
            bool displayItem;
            var commit = _commitRepository.GetSelectedOrLastCommit()?.ToModelCommit();
            if (commit == null)
                return;

            checkCurrentCommit = commit.Name != Constants.UncommittedChangesCommitName;

            if (!SheerResponse.CheckModified())
                return;

            bool flag = !ChangesOptions.ShowUnchanged;
            ChangesOptions.ShowUnchanged = flag;
            Item folder = this.ContentEditorDataContext.GetFolder();


            if (checkCurrentCommit)
            {
                displayItem = !folder.AddedIn(commit) && !folder.ChangedIn(commit) && !folder.HaveChildAddedIn(commit) && !folder.HaveChildChangedIn(commit);
            }
            else
            {
                var lastCommit = _commitRepository.GetLastCommit().ToModelCommit();
                displayItem = !folder.IsChangedAfterTime(lastCommit.CommitTime) && !folder.IsAddedAfterTime(lastCommit.CommitTime) && !folder.HasAddedChildren(lastCommit.CommitTime) && !folder.HasChangedChildren(lastCommit.CommitTime);
            }

            if (folder != null && (flag || displayItem))
            {
                UrlString urlString = new UrlString(WebUtil.GetRawUrl());
                folder.Uri.AddToUrlString(urlString);
                urlString["pa" + WebUtil.GetQueryString("pa", "0")] = folder.Uri.ToString();
                urlString["ras"] = CommitterStripName;
                SheerResponse.SetLocation(urlString.ToString());
            }
            else
                SheerResponse.SetLocation(string.Empty);
        }

        public void ChangedItems_Click()
        {
            bool checkCurrentCommit;
            bool displayItem;
            var commit = _commitRepository.GetSelectedOrLastCommit()?.ToModelCommit();
            if (commit == null)
                return;

            checkCurrentCommit = commit.Name != Constants.UncommittedChangesCommitName;

            if (!SheerResponse.CheckModified())
                return;

            bool flag = !ChangesOptions.ShowChanged;
            ChangesOptions.ShowChanged = flag;
            Item folder = this.ContentEditorDataContext.GetFolder();

            if (checkCurrentCommit)
            {
                displayItem = folder.ChangedIn(commit) || folder.HaveChildChangedIn(commit);
            }
            else
            {
                var lastCommit = _commitRepository.GetLastCommit().ToModelCommit();
                displayItem = folder.IsChangedAfterTime(lastCommit.CommitTime) || folder.HasChangedChildren(lastCommit.CommitTime);
            }

            if (folder != null && (flag || displayItem))
            {
                UrlString urlString = new UrlString(WebUtil.GetRawUrl());
                folder.Uri.AddToUrlString(urlString);
                urlString["pa" + WebUtil.GetQueryString("pa", "0")] = folder.Uri.ToString();
                urlString["ras"] = CommitterStripName;
                SheerResponse.SetLocation(urlString.ToString());
            }
            else
                SheerResponse.SetLocation(string.Empty);
        }

        public void AddedItems_Click()
        {
            bool checkCurrentCommit;
            bool displayItem;
            var commit = _commitRepository.GetSelectedOrLastCommit()?.ToModelCommit();
            if (commit == null)
                return;

            checkCurrentCommit = commit.Name != Constants.UncommittedChangesCommitName;

            if (!SheerResponse.CheckModified())
                return;

            bool flag = !ChangesOptions.ShowAdded;
            ChangesOptions.ShowAdded = flag;
            Item folder = this.ContentEditorDataContext.GetFolder();

            if (checkCurrentCommit)
            {
                displayItem = folder.AddedIn(commit) || folder.HaveChildAddedIn(commit);
            }
            else
            {
                var lastCommit = _commitRepository.GetLastCommit().ToModelCommit();
                displayItem = folder.IsAddedAfterTime(lastCommit.CommitTime) || folder.HasAddedChildren(lastCommit.CommitTime);
            }

            if (folder != null && (flag || displayItem))
            {
                UrlString urlString = new UrlString(WebUtil.GetRawUrl());
                folder.Uri.AddToUrlString(urlString);
                urlString["pa" + WebUtil.GetQueryString("pa", "0")] = folder.Uri.ToString();
                urlString["ras"] = CommitterStripName;
                SheerResponse.SetLocation(urlString.ToString());
            }
            else
                SheerResponse.SetLocation(string.Empty);
        }

    }
}