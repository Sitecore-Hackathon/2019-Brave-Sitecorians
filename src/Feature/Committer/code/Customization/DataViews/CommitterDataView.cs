using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;
using Sitecore.Feature.Committer.Extensions;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Buckets.Forms;

namespace Sitecore.Feature.Committer.Customization.DataViews
{
    public class CommitterDataView : BucketDataView
    {
        ICommitRepository _commitRepository;

        public CommitterDataView()
        {
            _commitRepository = new CommitRepository();
        }

        protected override void GetChildItems(ItemCollection items, Item item)
        {
            base.GetChildItems(items, item);
            filterByChanges(items);
        }

        private void filterByChanges(ItemCollection items)
        {
            bool checkCurrentCommit;
            Commit commit = _commitRepository.GetSelectedOrLastCommit()?.ToModelCommit();
            Commit lastCommit = _commitRepository.GetLastCommit()?.ToModelCommit();
            if (commit == null)
                return;

            checkCurrentCommit = commit.Name != Constants.UncommittedChangesCommitName;

            if (ChangesOptions.ShowUnchanged && ChangesOptions.ShowChanged && ChangesOptions.ShowAdded)
                return;

            for (int index = items.Count - 1; index >= 0; --index)
            {
                bool? IsAdded = null;
                bool? IsChanged = null;
                bool? IsUnchanged = null;
                bool? HaveAddedChild = null;
                bool? HaveChangedChild = null;
                bool? HaveUnchangedChild = null;
                bool HaveChildren = items[index].HasChildren;
                bool haveReasonToBeHidden = false;

                if (!ChangesOptions.ShowAdded)
                {
                    haveReasonToBeHidden = haveReasonToBeHidden ? haveReasonToBeHidden : getIsAdded();
                }

                if (!ChangesOptions.ShowChanged)
                {
                    haveReasonToBeHidden = haveReasonToBeHidden ? haveReasonToBeHidden : getIsChanged();
                }

                if (!ChangesOptions.ShowUnchanged)
                {
                    haveReasonToBeHidden = haveReasonToBeHidden ? haveReasonToBeHidden : getIsUnchanged();
                }

                if (!haveReasonToBeHidden)
                    continue;
                else
                {
                    if (!ChangesOptions.ShowAdded)
                    {
                        if (HaveChildren && ((ChangesOptions.ShowChanged && getHaveChangedChild()) || (ChangesOptions.ShowUnchanged && getHaveUnchangedChild())))
                        {
                            continue;
                        }
                        else
                        {
                            items.RemoveAt(index);
                            continue;
                        }
                    }

                    if (!ChangesOptions.ShowChanged)
                    {
                        if (HaveChildren && ((ChangesOptions.ShowAdded && getHaveAddedChild()) || (ChangesOptions.ShowUnchanged && getHaveUnchangedChild())))
                        {
                            continue;
                        }
                        else
                        {
                            items.RemoveAt(index);
                            continue;
                        }
                    }

                    if (!ChangesOptions.ShowUnchanged)
                    {
                        if (HaveChildren && ((ChangesOptions.ShowChanged && getHaveChangedChild()) || (ChangesOptions.ShowAdded && getHaveAddedChild())))
                        {
                            continue;
                        }
                        else
                        {
                            items.RemoveAt(index);
                            continue;
                        }
                    }
                }

                bool getIsAdded()
                {
                    if (checkCurrentCommit)
                        IsAdded = IsAdded ?? items[index].AddedIn(commit);
                    else
                        IsAdded = IsAdded ?? items[index].IsAddedAfterTime(lastCommit.CommitTime);

                    return IsAdded.Value;
                }

                bool getIsChanged()
                {
                    if (checkCurrentCommit)
                        IsChanged = IsChanged ?? items[index].ChangedIn(commit) && !getIsAdded();
                    else
                        IsChanged = IsChanged ?? items[index].IsChangedAfterTime(lastCommit.CommitTime) && !getIsAdded();
                    return IsChanged.Value;
                }

                bool getIsUnchanged()
                {
                    if (IsUnchanged.HasValue)
                        return IsUnchanged.Value;

                    bool added = getIsAdded();
                    if (added)
                    {
                        IsUnchanged = false;
                        return IsUnchanged.Value;
                    }
                    else
                    {
                        bool changed = getIsChanged();
                        if (changed)
                        {
                            IsUnchanged = false;
                            return IsUnchanged.Value;
                        }
                    }
                    IsUnchanged = true;
                    return IsUnchanged.Value;
                }

                bool getHaveAddedChild()
                {
                    if (checkCurrentCommit)
                        HaveAddedChild = HaveAddedChild ?? items[index].HaveChildAddedIn(commit);
                    else
                        HaveAddedChild = HaveAddedChild ?? items[index].HasAddedChildren(lastCommit.CommitTime);

                    return HaveAddedChild.Value;
                }

                bool getHaveChangedChild()
                {
                    if (checkCurrentCommit)
                        HaveChangedChild = HaveChangedChild ?? items[index].HaveChildChangedIn(commit);
                    else
                        HaveChangedChild = HaveChangedChild ?? items[index].HasChangedChildren(lastCommit.CommitTime);

                    return HaveChangedChild.Value;
                }

                bool getHaveUnchangedChild()
                {
                    if (HaveUnchangedChild.HasValue)
                        return HaveUnchangedChild.Value;

                    bool added = getHaveAddedChild();
                    if (added)
                    {
                        HaveUnchangedChild = false;
                        return HaveUnchangedChild.Value;
                    }
                    else
                    {
                        bool changed = getHaveChangedChild();
                        if (changed)
                        {
                            HaveUnchangedChild = false;
                            return HaveUnchangedChild.Value;
                        }
                    }
                    HaveUnchangedChild = true;
                    return HaveUnchangedChild.Value;
                }

            }
        }


    }
}