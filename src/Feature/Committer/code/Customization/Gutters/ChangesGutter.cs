using Sitecore.Shell.Applications.ContentEditor.Gutters;
using System;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Extensions;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Customization.Gutters
{
    public class ChangesGutter : GutterRenderer
    {
        ICommitRepository _commitRepository;
        IGutterDisplaySelector _gutterSelector;

        public ChangesGutter()
        {
            _commitRepository = new CommitRepository();
            _gutterSelector = new GutterDisplaySelector();
        }

        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            GutterIconDescriptor gutterIconDescriptor = new GutterIconDescriptor();

            try
            {
                ChangeState state;
                var commit = _commitRepository.GetSelectedOrLastCommit().ToModelCommit();
                if (commit == null)
                    return base.GetIconDescriptor(item);

                if (commit.Name == Constants.UncommittedChangesCommitName)
                {
                    var lastCommit = _commitRepository.GetLastCommit().ToModelCommit();
                    var commits = _commitRepository.GetAllCommits()?.ToList().ConvertAll(x => x.ToModelCommit());
                    if (commits == null || commits.Count() == 0)
                        return base.GetIconDescriptor(item);

                    state = item.GetChangeState(new CommitsChain() { Commits = commits }, lastCommit);
                }
                else
                {
                    state = item.GetCommitState(commit);
                }
                
                var gutter = _gutterSelector.GetGutterIconDescriptor(state);
                return gutter ?? base.GetIconDescriptor(item);
            }
            catch (Exception)
            {
                return base.GetIconDescriptor(item);
            }
        }
    }
}