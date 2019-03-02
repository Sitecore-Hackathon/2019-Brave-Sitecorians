using Sitecore.Shell.Framework.Commands;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Helpers.Interfaces;

namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class ShowCurrentDatabase : Command
    {
        IDatabaseSelector _databaseSelector;

        public ShowCurrentDatabase()
        {
            _databaseSelector = new DatabaseSelector();
        }

        public override void Execute(CommandContext context)
        {
            var db = _databaseSelector.GetSelectedDataBase()?.Name;
            if (string.IsNullOrEmpty(db))
            {
                Context.ClientPage.ClientResponse.Alert(Constants.Messages.NoDatabaseSelected);
            }
            else
            {
                Context.ClientPage.ClientResponse.Alert(string.Format(Constants.Messages.CurrentDatabaseMessageTemplate, db));
            }
        }
        
    }
}