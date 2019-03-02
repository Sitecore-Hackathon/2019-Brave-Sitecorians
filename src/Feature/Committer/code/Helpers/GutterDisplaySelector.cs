using Sitecore.Shell.Applications.ContentEditor.Gutters;
using System;
using System.Collections.Generic;
using Sitecore.Feature.Committer.Helpers.Interfaces;
using Sitecore.Feature.Committer.Models;

namespace Sitecore.Feature.Committer.Helpers
{
    public class GutterDisplaySelector : IGutterDisplaySelector
    {
        private Dictionary<ChangeState, GutterIconDescriptor> descriptors;

        public GutterDisplaySelector()
        {
            //Don't actually know where to initialize this dictionary better way. 
            //TODO: Discuss this with Vadzim.

            descriptors = new Dictionary<ChangeState, GutterIconDescriptor>()
            {
                {
                    ChangeState.Added,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/add2.png",
                        Tooltip = "Item created"
                    }
                },
                {
                    ChangeState.Changed,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/edit.png",
                        Tooltip = "Item updated"
                    }
                },
                {
                    ChangeState.HaveAddedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/folder_add.png",
                        Tooltip = "Item have created children"
                    }
                },
                {
                    ChangeState.HaveChangedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/folder_edit.png",
                        Tooltip = "Item have updated children"
                    }
                },
                {
                    ChangeState.HaveChangedAndAddedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/folder_add.png",
                        Tooltip = "Item have added and updated children"
                    }
                },
                {
                    ChangeState.AddedAndHaveAddedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/add2.png",
                        Tooltip = "Item added and have added children"
                    }
                },
                {
                    ChangeState.AddedAndHaveChangedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/add2.png",
                        Tooltip = "Item added and have changed children"
                    }
                },
                {
                    ChangeState.AddedAndHaveChangedAndAddedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/add2.png",
                        Tooltip = "Item added and have added and changed children"
                    }
                },
                {
                    ChangeState.ChangedAndHaveAddedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/edit.png",
                        Tooltip = "Item changed and have added children"
                    }
                },
                {
                    ChangeState.ChangedAndHaveChangedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/edit.png",
                        Tooltip = "Item changed and have changed children"
                    }
                },
                {
                    ChangeState.ChangedAndHaveChangedAndAddedChildren,
                    new GutterIconDescriptor()
                    {
                        Icon = "Applications/32x32/edit.png",
                        Tooltip = "Item changed and have added and changed children"
                    }
                },
                {
                    ChangeState.None,
                    null
                },

            };
        }

        public GutterIconDescriptor GetGutterIconDescriptor(ChangeState state)
        {

            GutterIconDescriptor descriptor = null;

            try
            {
                descriptor = descriptors?[state];
            }
            catch (Exception)
            {
            }

            return descriptor;
        }
    }
}