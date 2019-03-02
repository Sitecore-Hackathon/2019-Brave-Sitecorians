using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Feature.Committer.Extensions;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Helpers
{

    public class CommitRepository : ICommitRepository
    {
        IItemsChangesHelper _itemsChangesHelper;
        ICommitNameGenerator _commitNameGenerator;
        IDatabaseSelector _databaseSelector;

        public CommitRepository()
        {
            _databaseSelector = new DatabaseSelector();
            _itemsChangesHelper = new ItemsChangesHelper();
            _commitNameGenerator = new CommitNameGenerator();
        }

        public Item GetOrCreateCommitsFolder()
        {
            var database = _databaseSelector?.GetSelectedDataBase();
            if (database == null)
                return null;

            var systemItem = database.GetItem(Constants.Items.CommitterModuleFolder);
            var commitsFolder = systemItem.GetChildren(Sitecore.Collections.ChildListOptions.None).Where(x => x.Name == Constants.CommitsFolderName && x.TemplateID == new TemplateID(new ID(Constants.Templates.Node)))?.FirstOrDefault();
            if (commitsFolder != null)
                return commitsFolder;
            else
            {
                commitsFolder = systemItem.Add(Constants.CommitsFolderName, new TemplateID(new ID(Constants.Templates.Node)));
            }

            return commitsFolder;
        }

        public Item GetSelectedOrLastCommit()
        {
            return GetSelectedCommit() ?? GetLastCommit();
        }

        public Item GetLastCommit()
        {
            var commitsFolder = GetOrCreateCommitsFolder();
            if (commitsFolder == null)
                return null;
            var commits = commitsFolder.Children.Where(x => x.IsCommit() && x.Name != Constants.UncommittedChangesCommitName);
            if (commits == null || commits.Count() == 0)
                return null;
            var lastCommit = commits.OrderByDescending(x => x.Created).FirstOrDefault();

            return lastCommit;
        }

        public IEnumerable<Item> GetAllCommits()
        {
            var commitsFolder = GetOrCreateCommitsFolder();
            if (commitsFolder == null)
                return null;
            var commits = commitsFolder.Children.Where(x => x.IsCommit());

            return commits;
        }

        private Item getOrCreateCurrentCommitItem()
        {
            var database = _databaseSelector.GetSelectedDataBase();
            var systemItem = database.GetItem(Constants.Items.CommitterModuleFolder);
            var selectedCommit = systemItem.GetChildren(Sitecore.Collections.ChildListOptions.None).Where(x => x.TemplateID == new TemplateID(new ID(Constants.Templates.SelectedCommit)))?.FirstOrDefault();
            if (selectedCommit != null)
                return selectedCommit;
            else
            {
                selectedCommit = systemItem.Add(Constants.SelectedCommitItemDefaultName, new TemplateID(new ID(Constants.Templates.SelectedCommit)));
            }
            return selectedCommit;
        }
        
        public Item GetSelectedCommit()
        {
            var database = _databaseSelector.GetSelectedDataBase();
            var selectedCommit = getOrCreateCurrentCommitItem();

            var selectedCommitId = selectedCommit[Constants.SelectedCommitConstants.CommitFieldName];
            if (ID.TryParse(selectedCommitId, out ID id))
            {
                var commitItem = database.GetItem(id);
                return commitItem?.TemplateID == new TemplateID(new ID(Constants.Templates.Commit)) ? commitItem : null;
            }
            else
                return null;
        }

        public Commit CreateCommit(CommitParameters parameters)
        {
            if (parameters == null || _commitNameGenerator == null)
                return null;

            var commit = new Commit();

            if (parameters.CommitAll)
            {
                if (_itemsChangesHelper != null)
                {
                    var lastCommit = GetLastCommit().ToModelCommit();
                    _itemsChangesHelper.CountdownCommit = lastCommit;
                    _itemsChangesHelper.Commits = new CommitsChain() { Commits = GetAllCommits().ToList().ConvertAll(x => x.ToModelCommit()) };
                    _itemsChangesHelper.Update();

                    commit.CommitedAll = true;
                    commit.AddedItems = _itemsChangesHelper.AddedItems;
                    commit.ChangedItems = _itemsChangesHelper.ChangedItems;
                }
            }
            else
            {
                commit.CommitedAll = false;
                commit.AddedItems = parameters.AddedItems;
                commit.ChangedItems = parameters.ChangedItems;
            }

            commit.Name = _commitNameGenerator.AutoGenerateCommitName(commit);

            return commit;

        }

        public bool CreateAndDeployCommit(CommitParameters parameters)
        {
            var commit = CreateCommit(parameters);
            if (commit == null)
                return false;

            var commitFolder = GetOrCreateCommitsFolder();

            var commitItem = commitFolder.DeployCommit(commit);

            return commitItem == null;
        }

        public bool SetCommitAsSelected(ID id)
        {
            return SetCommitAsSelected(_databaseSelector.GetSelectedDataBase()?.GetItem(id));
        }

        public bool SetCommitAsSelected(string id)
        {
            if (ID.TryParse(id, out ID guid))
            {
                return SetCommitAsSelected(guid);
            }
            else
            {
                return false;
            }
        }

        public bool SetCommitAsSelected(Item item)
        {
            if (item == null || item.TemplateID != new TemplateID(new ID(Constants.Templates.Commit)))
                return false;

            var selectedCommitItem = getOrCreateCurrentCommitItem();
            if (selectedCommitItem == null)
                return false;

            selectedCommitItem.Editing.BeginEdit();
            selectedCommitItem[Constants.SelectedCommitConstants.CommitFieldName] = item.ID.ToString();
            selectedCommitItem.Editing.EndEdit();
            return true;
        }
    }
}