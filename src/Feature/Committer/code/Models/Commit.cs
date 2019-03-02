using Sitecore.Data;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Feature.Committer.Models
{
    public class Commit : IEquatable<Commit>
    {
        public DateTime CommitTime { get; set; } = DateTime.Now;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public User Invoker { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<ID> ChangedItems { get; set; } = new List<ID>();
        public IEnumerable<ID> AddedItems { get; set; } = new List<ID>();
        public bool CommitedAll { get; set; } = true;

        public bool Equals(Commit other)
        {
            return CommitTime == other.CommitTime &&
                CreationTime == other.CreationTime &&
                Invoker == other.Invoker &&
                Name == other.Name &&
                ChangedItems.SequenceEqual(other.ChangedItems) &&
                AddedItems.SequenceEqual(other.AddedItems)  &&
                CommitedAll == other.CommitedAll;
        }
    }
}