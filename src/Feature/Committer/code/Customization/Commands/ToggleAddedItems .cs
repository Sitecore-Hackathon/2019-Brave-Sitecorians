using Sitecore.Shell.Framework.Commands;
using System;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class ToggleAddedItems : Command
    {
        public override void Execute(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public override string GetClick(CommandContext context, string click)
        {
            return "AddedItems_Click";
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (!ChangesOptions.ShowAdded)
            {
                return CommandState.Enabled;
            }
            return CommandState.Down;
        }
    }
}