using Sitecore.Shell.Applications.ContentEditor.Gutters;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Helpers.Interfaces
{
    interface IGutterDisplaySelector
    {
        GutterIconDescriptor GetGutterIconDescriptor(ChangeState state);
    }
}
