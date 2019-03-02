using Sitecore.Data;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Committer.Helpers
{
    public class DatabaseSelector : IDatabaseSelector
    {
        private bool recordDatabaseName(Item databaseDataItem, string name)
        {
            if (databaseDataItem == null || databaseDataItem.TemplateID != new TemplateID(new ID(Constants.Templates.SelectedDatabase)))
                return false;

            databaseDataItem.Editing.BeginEdit();
            databaseDataItem[Constants.SelectedDatabaseConstants.NameFieldName] = name;
            databaseDataItem.Editing.EndEdit();

            return true;
        }

        private string getDatabaseName(Item databaseDataItem)
        {
            if (databaseDataItem == null || databaseDataItem.TemplateID != new TemplateID(new ID(Constants.Templates.SelectedDatabase)))
                return string.Empty;

            string databaseName = databaseDataItem.Fields[Constants.SelectedDatabaseConstants.NameFieldName]?.Value;

            return databaseName ?? string.Empty;
        }

        public Database GetSelectedDataBase()
        {
            var db = Sitecore.Context.Items["sc_ContentDatabase"] as Sitecore.Data.DefaultDatabase;
            return db;            
        }

        public Database GetContextDataBase()
        {
            return Sitecore.Context.Database;
        }
    }
}