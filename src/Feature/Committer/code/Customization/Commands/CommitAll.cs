using Sitecore.Shell.Framework.Commands;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class CommitAll : Command
    {
        public override void Execute(CommandContext context)
        {
            new CommitRepository().CreateAndDeployCommit(new CommitParameters() { CommitAll = true });
        }
    }
}