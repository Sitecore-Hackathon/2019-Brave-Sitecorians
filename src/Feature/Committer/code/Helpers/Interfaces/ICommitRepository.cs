using Sitecore.Data.Items;
using System.Collections.Generic;
using Sitecore.Feature.Committer.Models;
using Sitecore.Data;

namespace Sitecore.Feature.Committer.Helpers.Interfaces
{
    public interface ICommitRepository
    {
        Item GetOrCreateCommitsFolder();
        Item GetSelectedOrLastCommit();
        bool SetCommitAsSelected(ID id);
        bool SetCommitAsSelected(string id);
        bool SetCommitAsSelected(Item item);
        Item GetLastCommit();
        IEnumerable<Item> GetAllCommits();
        Item GetSelectedCommit();
        Commit CreateCommit(CommitParameters parameters);
        bool CreateAndDeployCommit(CommitParameters parameters);
    }
}
