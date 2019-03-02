using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Extensions
{
    public static class CommitExtensions
    {
        public static Commit ToModelCommit(this Item item)
        {
            if (item == null || item.TemplateID != new TemplateID(new ID(Constants.Templates.Commit)))
                return null;

            var commit = new Commit();
            try
            {
                commit.CommitTime = ((DateField)item.Fields[Constants.CommitConstants.TimeFieldName]).DateTime;
                commit.CreationTime = ((DateField)item.Fields[Constants.CommitConstants.CreationTimeFieldName]).DateTime;
                commit.Name = item.Fields[Constants.CommitConstants.NameFieldName].Value;
                commit.CommitedAll = ((CheckboxField)item.Fields[Constants.CommitConstants.CommitedAllFieldName]).Checked;
                commit.AddedItems = ((MultilistField)item.Fields[Constants.CommitConstants.AddedItemsFieldName]).TargetIDs;
                commit.ChangedItems = ((MultilistField)item.Fields[Constants.CommitConstants.ChangedItemsFieldName]).TargetIDs;
                return commit;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Item DeployCommit(this Item item, Commit commit, string name = null)
        {
            if (name == null || commit.Name == null || string.IsNullOrWhiteSpace(commit.Name))
            {
                name = new CommitNameGenerator().AutoGenerateCommitName(commit);
            }
            var newItem = item.Add(name, new TemplateID(new ID(Constants.Templates.Commit)));
            newItem.Editing.BeginEdit();
            newItem[Constants.CommitConstants.TimeFieldName] = commit.CommitTime.ToSitecoreField();
            newItem[Constants.CommitConstants.CreationTimeFieldName] = commit.CreationTime.ToSitecoreField();
            newItem[Constants.CommitConstants.NameFieldName] = commit.Name;
            newItem[Constants.CommitConstants.CommitedAllFieldName] = commit.CommitedAll.ToSitecoreField();
            newItem[Constants.CommitConstants.AddedItemsFieldName] = commit.AddedItems.ToSitecoreField();
            newItem[Constants.CommitConstants.ChangedItemsFieldName] = commit.ChangedItems.ToSitecoreField();

            newItem.Editing.EndEdit();

            return newItem;
        }

        public static bool IsCommit(this Item item)
        {
            return item.TemplateID == new TemplateID(new ID(Constants.Templates.Commit));
        }
    }
}