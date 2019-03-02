using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Feature.Committer.Extensions
{
    public static class ItemsExtensions
    {
        public static IEnumerable<Item> RemoveWithCertainTemplates(this IEnumerable<Item> items, IEnumerable<string> templatesIds)
        {
            var ids = templatesIds.Select(x => new ID(x));
            return items.Where(x => !ids.Contains(x.TemplateID));


        }

        public static bool HaveOneOfCertainTemplates(this Item item, IEnumerable<string> templatesIds)
        {
            var ids = templatesIds.Select(x => new ID(x));
            return ids.Contains(item.TemplateID);


        }
    }
}