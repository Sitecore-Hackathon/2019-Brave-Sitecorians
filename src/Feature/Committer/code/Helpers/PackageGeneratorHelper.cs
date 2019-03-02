using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Install;
using Sitecore.Install.Items;
using Sitecore.Install.Zip;
using Sitecore.Security.Accounts;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.Feature.Committer.Helpers
{
    public class PackageGeneratorHelper
    {
        public string PathToFile = Constants.Package.DefaultPackageLocation;

        IDatabaseSelector _databaseSelector;

        public PackageGeneratorHelper()
        {
            _databaseSelector = new DatabaseSelector();
        }

        public PackageGenerationResult Generate(Commit commit, bool getAdded = true, bool getUpdated = true)
        {
            var result = new PackageGenerationResult();

            StringBuilder commitNameBuilder = new StringBuilder();
            commitNameBuilder.Append(commit.Name);
            if (getAdded)
                commitNameBuilder.Append("-added");

            if (getUpdated)
                commitNameBuilder.Append("-updated");

            commitNameBuilder.Append(".zip");
            var commitName = commitNameBuilder.ToString();

            var path = PathToFile;

            result.Name = commitName;
            result.Path = path;
            result.Successful = false;

            var db = _databaseSelector.GetSelectedDataBase();
            var document = new PackageProject();
            document.Sources.Clear();
            var commitUserName = commit.Invoker?.Name;
            document.Metadata.PackageName = !string.IsNullOrWhiteSpace(commit.Name) ? commit.Name : Constants.Package.DefaultCommitName;
            document.Metadata.Author = !string.IsNullOrWhiteSpace(commitUserName) ? commitUserName : Constants.Package.DefaultAuthorName;
            document.Metadata.Publisher = !string.IsNullOrWhiteSpace(User.Current.Name) ? User.Current.Name : Constants.Package.DefaultAuthorName;
            document.Metadata.Version = Constants.Package.DefaultVersion;


            var itemsIds = new List<ID>();
            if (getAdded)
                itemsIds = itemsIds.Union(commit.AddedItems).ToList();
            if (getUpdated)
                itemsIds = itemsIds.Union(commit.ChangedItems).ToList();

            var items = new List<Item>();

            itemsIds.ForEach(x =>
            {
                var item = db.GetItem(x);
                if (item != null)
                    items.Add(item);
            });

            ExplicitItemSource source = new ExplicitItemSource();
            source.Name = Constants.Package.SourceName;
            source.SkipVersions = true;

            foreach (Item item in items)
            {
                if (item != null)
                    source.Entries.Add(new ItemReference(item.Uri, false).ToString());
            }

            document.Sources.Add(source);
            document.SaveProject = true;

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                using (var writer = new PackageWriter(MainUtil.MapPath($"{path}{commitName}")))
                {
                    Context.SetActiveSite(Constants.Package.ShellSite);
                    writer.Initialize(Installer.CreateInstallationContext());
                    PackageGenerator.GeneratePackage(document, writer);
                    Context.SetActiveSite(Constants.Package.MainSite);
                    result.Successful = true;
                }
            }

            return result;
        }
    }
}