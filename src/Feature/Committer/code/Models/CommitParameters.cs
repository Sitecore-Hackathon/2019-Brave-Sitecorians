using Sitecore.Data;
using System;
using System.Collections.Generic;

namespace Sitecore.Feature.Committer.Models
{
    public class CommitParameters
    {
        public bool CommitAll { get; set; } = true;
        public DateTime CommitTime { get; set; } = DateTime.Now;
        public IEnumerable<ID> ChangedItems { get; set; } = new List<ID>();
        public IEnumerable<ID> AddedItems { get; set; } = new List<ID>();
    }
}