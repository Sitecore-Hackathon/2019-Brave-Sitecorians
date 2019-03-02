using Sitecore.Shell.Framework.Commands;
using Sitecore.Feature.Committer.Extensions;
using Sitecore.Feature.Committer.Helpers;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using System;

namespace Sitecore.Feature.Committer.Customization.Commands
{
    public abstract class GeneratePackageBase : Command
    {
        protected ICommitRepository _commitRepository;
        protected PackageGeneratorHelper _packageGeneratorHelper;
        protected bool getAdded;
        protected bool getUpdated;

        public GeneratePackageBase()
        {
            _commitRepository = new CommitRepository();
            _packageGeneratorHelper = new PackageGeneratorHelper();
        }

        public override void Execute(CommandContext context)
        {
            var selectedCommit = _commitRepository.GetSelectedCommit().ToModelCommit();
            if (selectedCommit == null)
            {
                Context.ClientPage.ClientResponse.Alert(Constants.Package.Messages.NoCommitSelected);
                return;
            }

            try
            {
                var result = _packageGeneratorHelper.Generate(selectedCommit, getAdded, getUpdated);
                if (result.Successful)
                {
                    var message = string.Format(Constants.Package.Messages.CreateSuccess, result.Name, result.Path);
                    //Context.ClientPage.ClientResponse.Alert(message);
                    return;
                }
                else
                {
                    //Context.ClientPage.ClientResponse.Alert(Constants.Package.Messages.CreateFailure);
                    return;
                }
            }
            catch (Exception e)
            {
                //Context.ClientPage.ClientResponse.Alert(string.Format(Constants.Package.Messages.CreateException, e.Message));
            }
        }
    }
}