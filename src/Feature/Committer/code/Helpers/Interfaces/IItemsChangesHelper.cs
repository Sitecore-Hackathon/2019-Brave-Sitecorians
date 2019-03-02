using Sitecore.Data;
using Sitecore.Feature.Committer.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.Committer.Helpers.Interfaces
{
    public interface IItemsChangesHelper
    {
        Commit CountdownCommit { get; set; }
        CommitsChain Commits { get; set; }
        IEnumerable<ID> ChangedItems { get; }
        IEnumerable<ID> AddedItems { get; }
        IEnumerable<ID> UnchangedItems { get; }
        IItemsChangesHelper Update();
        IItemsChangesHelper UpdateIfChanged();
    }
}
