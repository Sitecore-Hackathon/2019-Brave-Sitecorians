using Sitecore.Shell.Framework.Commands;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Helpers.Interfaces;

namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class SelectCommit : Command
    {
        ICommitRepository _commitRepository;

        public SelectCommit()
        {
            _commitRepository = new CommitRepository();
        }

        public override void Execute(CommandContext context)
        {
            var commitId = context.Parameters["id"];
            if (commitId == null)
                return;

            _commitRepository.SetCommitAsSelected(commitId);
        }
    }
}