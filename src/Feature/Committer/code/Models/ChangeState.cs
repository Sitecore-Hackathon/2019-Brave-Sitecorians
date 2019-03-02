namespace Sitecore.Feature.Committer.Models
{
    /// <summary>
    /// Used for defining icon for item gutter.
    /// </summary>
    public enum ChangeState
    {
        None,
        Changed,
        Added,
        HaveAddedChildren,
        HaveChangedChildren,
        HaveChangedAndAddedChildren,
        ChangedAndHaveAddedChildren,
        ChangedAndHaveChangedChildren,
        ChangedAndHaveChangedAndAddedChildren,
        AddedAndHaveAddedChildren,
        AddedAndHaveChangedChildren,
        AddedAndHaveChangedAndAddedChildren
    }
}