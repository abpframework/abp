using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Domain.Services;

namespace Volo.Docs.GitHub.Documents
{
    public class GithubPatchAnalyzer : DomainService, IGithubPatchAnalyzer
    {
        private const string OldChangeStart = "\\n-";
        private const string NewChangeStart = "\\n+";

        public bool HasPatchSignificantChanges(string patch)
        {
            var changes = GetChanges(patch);

            foreach (var change in changes)
            {
                var isSignificant = IsChangeSignificant(change);

                if (isSignificant)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsChangeSignificant(CommitChanges change)
        {
            throw new NotImplementedException();
        }

        private List<CommitChanges> GetChanges(string patch)
        {
            var changes = new List<CommitChanges>();
            var changeList = patch.Split("@@").Where(s => !string.IsNullOrWhiteSpace(s)).Where((c, i) => i % 2 == 0).ToList();

            foreach (var change in changeList)
            {
                var commitChange = new CommitChanges();
                var lines = change.Split("\\n");

                commitChange.OldLines.AddRange(lines.Where(l => l.StartsWith("-")).Select(l => l.Substring(1)).Where(l => !string.IsNullOrWhiteSpace(l)));
                commitChange.NewLines.AddRange(lines.Where(l => l.StartsWith("+")).Select(l => l.Substring(1)).Where(l=>!string.IsNullOrWhiteSpace(l)));

                changes.Add(commitChange);
            }

            return changes;
        }

        private class CommitChanges
        {
            public List<string> OldLines { get; set; }

            public List<string> NewLines { get; set; }

            public CommitChanges()
            {
                OldLines = new List<string>();
                NewLines= new List<string>();
            }
        }
    }
}
