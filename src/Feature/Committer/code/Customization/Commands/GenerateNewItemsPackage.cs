namespace Sitecore.Feature.Committer.Customization.Commands
{
    public class GenerateNewItemsPackage : GeneratePackageBase
    {
        public GenerateNewItemsPackage() 
            : base()
        {
            getAdded = true;
            getUpdated = false;
        }
    }
}