namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class GenerateUpdatedItemsPackage : GeneratePackageBase
    {
        public GenerateUpdatedItemsPackage()
            : base()
        {
            getAdded = false;
            getUpdated = true;
        }
    }
}