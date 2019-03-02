namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class GenerateNewAndUpdatedItemsPackage : GeneratePackageBase
    {
        public GenerateNewAndUpdatedItemsPackage()
            : base()
        {
            getAdded = true;
            getUpdated = true;
        }
    }
}