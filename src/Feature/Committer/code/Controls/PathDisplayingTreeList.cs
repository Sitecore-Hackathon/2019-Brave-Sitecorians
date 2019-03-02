using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Committer.Controls
{
    public class PathDisplayingTreeList : TreeList
    {
        protected override string GetHeaderValue(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            string text = item.Paths.FullPath;
            return text ?? item.DisplayName;
        }
    }
}