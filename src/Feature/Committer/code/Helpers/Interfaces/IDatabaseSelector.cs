using Sitecore.Data;

namespace Sitecore.Feature.Committer.Helpers.Interfaces
{
    public interface IDatabaseSelector
    {
        Database GetSelectedDataBase();
        Database GetContextDataBase();
    }
}
