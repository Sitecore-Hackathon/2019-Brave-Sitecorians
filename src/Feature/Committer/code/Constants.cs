using System.Collections.Generic;

namespace Sitecore.Feature.Committer
{
    public static class Constants
    {
        public static class Templates
        {
            public const string Commit = "{9FCC9AE0-CA8D-4246-B054-6DB8CD6C5E4A}";
            public const string Node = "{239F9CF4-E5A0-44E0-B342-0F32CD4C6D8B}";
            public const string SelectedDatabase = "{3C5FC5F5-0038-4A59-86AB-CC6B4B490B0E}";
            public const string SelectedCommit = "{CCD4DCEE-B263-4E2B-A964-941B88C5EDB3}";

            public static List<string> IgnoredItemsTemplates = new List<string>()
            {
                Commit,
                SelectedDatabase,
                SelectedCommit
            };
        }

        public static class Items
        {
            //public const string SystemFolderId = "{13D6D6C6-C50B-4BBD-B331-2B04F1A58F21}";
            public const string CommitterModuleFolder = "/sitecore/system/Modules/Committer";
        }

        public static class CommitConstants
        {
            public const string TimeFieldName = "DateTime";
            public const string CreationTimeFieldName = "__Created";
            public const string NameFieldName = "Name";
            public const string CommitedAllFieldName = "Commited All";
            public const string AddedItemsFieldName = "Added Items";
            public const string ChangedItemsFieldName = "Changed Items";
            public const string InvokerFieldName = "Invoker";
        }

        public static class SelectedDatabaseConstants
        {
            public const string NameFieldName = "Name";
        }

        public static class Package
        {
            public static class Messages
            {
                public const string CreateSuccess = "Package {0} succesfully created in {1}";
                public const string NoCommitSelected = "No commit selected or commit with selected ID do not exist";
                public const string CreateFailure = "Failed to create a package";
                public const string CreateException = "An error occured during creating a package: {0}";
            }

            public const string DefaultVersion = "1.0.0";
            public const string SourceName = "ArchivedItems";
            public const string DefaultAuthorName = "Default Author Name";
            public const string DefaultCommitName = "Default Commit Name";
            public const string DefaultPackageLocation = "/data/packages/";
            public const string ShellSite = "shell";
            public const string MainSite = "website";
        }

        public static class SelectedCommitConstants
        {
            public const string CommitFieldName = "Commit";
        }

        public static class Messages
        {
            public const string CannotChangeDatabaseMessageTemplate = "Cannot change database to {0}";
            public const string CurrentDatabaseMessageTemplate = "Current database is \"{0}\"";
            public const string NoDatabaseSelected = "Error receiving current database. No database is chosen right now";
        }

        public const string CommitsFolderName = "Commits";
        public const string SelectedDatabaseItemDefaultName = "CurrentDatabase";
        public const string SelectedCommitItemDefaultName = "Selected Commit";
        public const string UncommittedChangesCommitName = "Uncommitted Changes";
    }
}