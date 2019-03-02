using System.Linq;
using Sitecore.Feature.Committer.Models;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;

namespace Sitecore.Feature.Committer.Extensions
{
    public static class ItemsCommitChangesExtensions
    {
        public static bool IsSavedAsAddedInCommit(this Item item, Commit commit)
        {
            return !item.IsAddedAfterTime(commit.CommitTime) && (commit.CommitedAll || commit.AddedItems.Contains(item.ID));
        }

        public static bool IsSavedAsChangedInCommit(this Item item, Commit commit)
        {
            return !item.IsChangedAfterTime(commit.CommitTime) && (commit.CommitedAll || commit.ChangedItems.Contains(item.ID) || commit.AddedItems.Contains(item.ID));
        }

        public static bool IsSavedAsAddedInCommitsChain(this Item item, CommitsChain chain)
        {
            if (chain == null)
                return false;

            var commits = chain.Commits?.ToList();
            if (commits == null || commits.Count() == 0)
                return false;

            foreach (var commit in commits)
            {
                if (item.IsSavedAsAddedInCommit(commit))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSavedAsChangedInCommitsChain(this Item item, CommitsChain chain)
        {
            if (chain == null)
                return false;

            var commits = chain.Commits?.ToList();
            if (commits == null || commits.Count() == 0)
                return false;

            foreach (var commit in commits)
            {
                if (item.IsSavedAsChangedInCommit(commit))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AddedSince(this Item item, CommitsChain chain, Commit countdownCommit)
        {
            var commits = chain.CommitsFromTimeToSelected(countdownCommit, item.Created);
            if (commits == null || commits.Commits == null || commits.Commits.Count() == 0)
            {
                return true;
            }
            return item.IsAddedAfterTime(commits.Commits.ToList()[commits.Commits.Count() - 1].CommitTime) || !item.IsSavedAsAddedInCommitsChain(commits);
        }

        public static bool ChangedSince(this Item item, CommitsChain chain, Commit countdownCommit)
        {
            var changedTime = ((DateField)item.Fields["__Updated"]).DateTime;
            var commits = chain.CommitsFromTimeToSelected(countdownCommit, changedTime);
            if (commits == null || commits.Commits == null || commits.Commits.Count() == 0)
            {
                return true;
            }
            return item.IsChangedAfterTime(commits.Commits.ToList()[commits.Commits.Count() - 1].CommitTime) || !item.IsSavedAsChangedInCommitsChain(commits);
        }

        public static bool ChangedIn(this Item item, Commit commit)
        {
            return commit.ChangedItems.Contains(item.ID);
        }

        public static bool AddedIn(this Item item, Commit commit)
        {
            return commit.AddedItems.Contains(item.ID);
        }

        public static bool HaveChildChangedIn(this Item item, Commit commit)
        {
            var hasChangedChildren = false;
            var childrens = item.Axes.GetDescendants();
            foreach (var child in childrens)
            {
                if (child.ChangedIn(commit) && !child.AddedIn(commit))
                {
                    hasChangedChildren = true;
                    break;
                }
            }
            return hasChangedChildren;
        }

        public static bool HaveChildAddedIn(this Item item, Commit commit)
        {
            var hasAddedChildren = false;
            var childrens = item.Axes.GetDescendants();
            foreach (var child in childrens)
            {
                if (child.AddedIn(commit))
                {
                    hasAddedChildren = true;
                    break;
                }
            }
            return hasAddedChildren;
        }

        public static bool HaveChildChangedSince(this Item item, CommitsChain chain, Commit countdownCommit)
        {
            var hasChangedChildren = false;
            var childrens = item.Axes.GetDescendants();
            foreach (var child in childrens)
            {
                if (!child.HaveOneOfCertainTemplates(Constants.Templates.IgnoredItemsTemplates) && child.ChangedSince(chain, countdownCommit) && !child.AddedSince(chain, countdownCommit))
                {
                    hasChangedChildren = true;
                    break;
                }
            }
            return hasChangedChildren;
        }

        public static bool HaveChildAddedSince(this Item item, CommitsChain chain, Commit countdownCommit)
        {
            var hasChangedChildren = false;
            var childrens = item.Axes.GetDescendants();
            foreach (var child in childrens)
            {
                if (!child.HaveOneOfCertainTemplates(Constants.Templates.IgnoredItemsTemplates) && child.AddedSince(chain, countdownCommit))
                {
                    hasChangedChildren = true;
                    break;
                }
            }
            return hasChangedChildren;
        }
    }
}