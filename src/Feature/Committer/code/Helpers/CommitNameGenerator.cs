using System.Linq;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Helpers
{
    public class CommitNameGenerator : ICommitNameGenerator
    {
        public string AutoGenerateCommitName(Commit commit)
        {
            // TODO: allow commite only Selected items
            string countItemsString = commit.CommitedAll ? "All" : "Selected";
            string commitDate = commit.CommitTime.ToString("yyyy-MM-dd-HH-mm");

            return $"Commit-{commitDate}-Added-{commit.AddedItems?.Count()}-Updated-{commit.ChangedItems?.Count()}";
        }
    }
}