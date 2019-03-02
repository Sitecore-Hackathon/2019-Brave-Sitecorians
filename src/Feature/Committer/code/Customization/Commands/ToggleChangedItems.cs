using Sitecore.Shell.Framework.Commands;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class ToggleChangedItems : Command
    {

        public override void Execute(CommandContext context)
        {

        }

        public override string GetClick(CommandContext context, string click)
        {
            return "ChangedItems_Click";
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (!ChangesOptions.ShowChanged)
            {
                return CommandState.Enabled;
            }
            return CommandState.Down;
        }

    }
}