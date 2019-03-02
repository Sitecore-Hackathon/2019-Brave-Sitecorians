using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.Feature.Committer.Extensions
{
    public static class FieldsExtensions
    {
        public static string ToSitecoreField(this bool field)
        {
            return field ? "1" : "0";
        }

        public static string ToSitecoreField(this DateTime field)
        {
            return DateUtil.ToIsoDate(field);
        }

        public static string ToSitecoreField(this IEnumerable<ID> field)
        {
            var fieldList = field.ToList();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < fieldList.Count; i++)
            {
                builder.Append(fieldList[i].ToString());
                if (i != fieldList.Count - 1)
                    builder.Append("|");
            }
            return builder.ToString();
        }
    }
}