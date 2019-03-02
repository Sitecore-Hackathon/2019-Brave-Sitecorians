using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Feature.Committer.Models
{
    public class CommitsChain
    {
        IEnumerable<Commit> _commits;

        public IEnumerable<Commit> Commits
        {
            get
            {
                return _commits.OrderByDescending(x => x.CommitTime);
            }
            set
            {
                _commits = value.OrderByDescending(x => x.CommitTime);
            }
        }

        public CommitsChain CommitsTillSelected(Commit commit)
        {
            var commits = Commits.ToList();
            if (!commits.Exists(x => x.Equals(commit)))
                return new CommitsChain { Commits = new List<Commit>() };

            var commitIndex = commits.IndexOf(commit);
            commits.RemoveRange(commitIndex, commits.Count - commitIndex);
            return new CommitsChain { Commits = commits };
        }

        public CommitsChain CommitsFromTimeToSelected(Commit commit, DateTime dateTime)
        {
            if (commit == null)
                return new CommitsChain { Commits = new List<Commit>() };

            var commits = Commits.ToList();
            if (!commits.Exists(x => x.Equals(commit)))
                return new CommitsChain { Commits = new List<Commit>() };

            return new CommitsChain { Commits = commits.Where(x => x.CommitTime >= dateTime && x.CommitTime <= commit.CommitTime) };

        }
    }
}