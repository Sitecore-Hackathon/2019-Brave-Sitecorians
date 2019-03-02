using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Helpers.Interfaces
{
    public interface ICommitNameGenerator
    {
        string AutoGenerateCommitName(Commit commit);
    }
}
