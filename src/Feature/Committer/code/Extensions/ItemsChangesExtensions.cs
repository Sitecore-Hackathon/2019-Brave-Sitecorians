using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Extensions
{
    public static class ItemsChangesExtensions
    {
        public static bool IsAddedAfterTime(this Item item, DateTime time)
        {
            DateTime created = ((DateField)item.Fields["__Created"]).DateTime;
            return created > time;
        }

        public static bool IsChangedAfterTime(this Item item, DateTime time)
        {
            DateTime updated = ((DateField)item.Fields["__Updated"]).DateTime;
            return updated > time;
        }

        public static bool HasAddedChildren(this Item item, DateTime time)
        {
            var hasAddedChildren = false;
            var childrens = item.Axes.GetDescendants();
            foreach (var child in childrens)
            {
                if (!child.HaveOneOfCertainTemplates(Constants.Templates.IgnoredItemsTemplates) && child.IsAddedAfterTime(time))
                {
                    hasAddedChildren = true;
                    break;
                }
            }
            return hasAddedChildren;
        }

        public static bool HasChangedChildren(this Item item, DateTime time)
        {
            var hasChangedChildren = false;
            var childrens = item.Axes.GetDescendants();
            foreach (var child in childrens)
            {
                if (!child.HaveOneOfCertainTemplates(Constants.Templates.IgnoredItemsTemplates) && child.IsChangedAfterTime(time) && !child.IsAddedAfterTime(time))
                {
                    hasChangedChildren = true;
                    break;
                }
            }
            return hasChangedChildren;
        }

        public static ChangeState GetChangeState(this Item item, CommitsChain chain, Commit countdownCommit)
        {
            if (item.HaveOneOfCertainTemplates(Constants.Templates.IgnoredItemsTemplates))
                return ChangeState.None;

            var isAdded = ChangesOptions.ShowAdded ? item.AddedSince(chain, countdownCommit) : false;
            var isChanged = ChangesOptions.ShowChanged ? !isAdded && item.ChangedSince(chain, countdownCommit) : false;
            var hasAddedChildren = ChangesOptions.ShowAdded ? item.HaveChildAddedSince(chain, countdownCommit) : false;
            var hasChangedChildren = ChangesOptions.ShowChanged ? item.HaveChildChangedSince(chain, countdownCommit) : false;

            return ChangeStateByStatus(isAdded, isChanged, hasAddedChildren, hasChangedChildren);
        }

        public static ChangeState GetCommitState(this Item item, Commit commit)
        {
            var isAdded = ChangesOptions.ShowAdded ? item.AddedIn(commit) : false;
            var isChanged = ChangesOptions.ShowChanged ? !isAdded && item.ChangedIn(commit) : false;
            var hasAddedChildren = ChangesOptions.ShowAdded ? item.HaveChildAddedIn(commit) : false;
            var hasChangedChildren = ChangesOptions.ShowChanged ? item.HaveChildChangedIn(commit) : false;

            return ChangeStateByStatus(isAdded, isChanged, hasAddedChildren, hasChangedChildren);
        }

        private static ChangeState ChangeStateByStatus(bool isAdded, bool isChanged, bool hasAddedChildren, bool hasChangedChildren)
        {
            if (isAdded)
            {
                if (hasAddedChildren && hasChangedChildren)
                {
                    return ChangeState.AddedAndHaveChangedAndAddedChildren;
                }
                if (hasAddedChildren && !hasChangedChildren)
                {
                    return ChangeState.AddedAndHaveAddedChildren;
                }
                if (!hasAddedChildren && hasChangedChildren)
                {
                    return ChangeState.AddedAndHaveChangedChildren;
                }
                return ChangeState.Added;
            }

            if (isChanged)
            {
                if (hasAddedChildren && hasChangedChildren)
                {
                    return ChangeState.ChangedAndHaveChangedAndAddedChildren;
                }
                if (hasAddedChildren && !hasChangedChildren)
                {
                    return ChangeState.ChangedAndHaveAddedChildren;
                }
                if (!hasAddedChildren && hasChangedChildren)
                {
                    return ChangeState.ChangedAndHaveChangedChildren;
                }
                return ChangeState.Changed;
            }

            if (hasAddedChildren)
            {
                if (hasChangedChildren)
                {
                    return ChangeState.HaveChangedAndAddedChildren;
                }
                return ChangeState.HaveAddedChildren;
            }

            if (hasChangedChildren)
            {
                return ChangeState.HaveChangedChildren;
            }


            return ChangeState.None;
        }
    }
}