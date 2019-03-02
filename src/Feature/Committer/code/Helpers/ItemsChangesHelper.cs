using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;
using Sitecore.Feature.Committer.Extensions;

namespace Sitecore.Feature.Committer.Helpers
{
    public class ItemsChangesHelper : IItemsChangesHelper
    {
        IDatabaseSelector _databaseSelector;

        public ItemsChangesHelper()
        {
            _databaseSelector = new DatabaseSelector();
        }

        private bool _changedSinceLastUpdate;

        private IEnumerable<ID> _changedItems;
        private IEnumerable<ID> _addedItems;
        private IEnumerable<ID> _unchangedItems;
        private Commit _countdownCommit;
        private CommitsChain _commits;

        public IEnumerable<ID> ChangedItems { get { return _changedItems; } }
        public IEnumerable<ID> AddedItems { get { return _addedItems; } }
        public IEnumerable<ID> UnchangedItems { get { return _unchangedItems; } }


        public Commit CountdownCommit
        {
            get { return _countdownCommit; }
            set
            {
                if (_countdownCommit != value)
                {
                    _changedSinceLastUpdate = true;
                    _countdownCommit = value;
                }
            }
        }
        public CommitsChain Commits
        {
            get { return _commits; }
            set
            {
                if (_commits != value)
                {
                    _changedSinceLastUpdate = true;
                    _commits = value;
                }
            }
        }

        public IItemsChangesHelper Update()
        {
            var database = _databaseSelector.GetSelectedDataBase();
            if (database == null)
                return this;

            var items = database.SelectItems("fast:/sitecore//*").RemoveWithCertainTemplates(Constants.Templates.IgnoredItemsTemplates).ToList();

            var addedItems = items.Where(x => x.AddedSince(Commits, CountdownCommit));
            var addedIDs = addedItems.Select(x => x.ID).ToList();

            var changedItems = items.Where(x => x.ChangedSince(Commits, CountdownCommit));
            var changedIDs = changedItems.Select(x => x.ID).ToList();

            var unchangedIDs = items.Select(x => x.ID).ToList();

            changedIDs.RemoveAll(x => addedIDs.Contains(x));
            unchangedIDs.RemoveAll(x => addedIDs.Contains(x) || changedIDs.Contains(x));

            _addedItems = addedIDs;
            _changedItems = changedIDs;
            _unchangedItems = unchangedIDs;


            _changedSinceLastUpdate = false;
            return this;
        }

        public IItemsChangesHelper UpdateIfChanged()
        {
            if (_changedSinceLastUpdate)
                return Update();
            else
                return this;
        }
    }
}