using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Domain.Services;

namespace Volo.Docs.GitHub.Documents
{
    public class GithubPatchAnalyzer : DomainService, IGithubPatchAnalyzer
    {
        public bool HasPatchSignificantChanges(string patch)
        {
            var changes = GetChanges(patch);

            return IsChangesSignificant(changes);
        }


        private bool IsChangesSignificant(CommitChanges change)
        {
            if (CompareLineCount(change))
            {
                return true;
            }

            if (CompareWordCount(change))
            {
                return true;
            }

            if (CompareWords(change))
            {
                return true;
            }

            return false;
        }

        private static bool CompareLineCount(CommitChanges change)
        {
            return Math.Abs(change.NewLines.Count - change.OldLines.Count) >= 3;
        }

        private static bool CompareWordCount(CommitChanges change)
        {
            var wordCountInNewLines =
                string.Join(" ", change.NewLines).Split(" ").Count(s => !string.IsNullOrWhiteSpace(s));
            var wordCountInOldLines =
                string.Join(" ", change.OldLines).Split(" ").Count(s => !string.IsNullOrWhiteSpace(s));

            return Math.Abs(wordCountInNewLines - wordCountInOldLines) >= 15;
        }

        private bool CompareWords(CommitChanges change)
        {
            var wordsInNewLines = GetDistinctWordsFromLineList(change.NewLines);
            var wordsInOldLines = GetDistinctWordsFromLineList(change.OldLines);

            var differentWordsInNewLines = wordsInNewLines.Except(wordsInOldLines).Count();
            var differentWordsInOldLines = wordsInOldLines.Except(wordsInNewLines).Count();

            return differentWordsInNewLines + differentWordsInOldLines >= 10;
        }

        private CommitChanges GetChanges(string patch)
        {
            var changes = new List<CommitChanges>();
            var pathSplited = patch.Split("@@");
            var changeList = pathSplited.Where(s => !string.IsNullOrWhiteSpace(s)).Where((c, i) => i % 2 == 1).ToList();

            foreach (var change in changeList)
            {
                var commitChange = new CommitChanges();
                var lines = change.Split("\n");

                commitChange.OldLines.AddRange(lines.Where(l => l.StartsWith("-")).Select(l => l.Substring(1)).Where(l => !string.IsNullOrWhiteSpace(l)));
                commitChange.NewLines.AddRange(lines.Where(l => l.StartsWith("+")).Select(l => l.Substring(1)).Where(l => !string.IsNullOrWhiteSpace(l)));

                changes.Add(commitChange);
            }

            return MergeChanges(changes);
        }
        private CommitChanges MergeChanges(List<CommitChanges> changes)
        {
            var mergedChanges = new CommitChanges();

            foreach (var commitChanges in changes)
            {
                mergedChanges.NewLines.AddRange(commitChanges.NewLines);
                mergedChanges.OldLines.AddRange(commitChanges.OldLines);
            }

            return mergedChanges;
        }

        private List<string> GetDistinctWordsFromLineList(List<string> lines)
        {
            return string.Join(" ", lines).Split(" ").Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(TrimAndRemovePunctuation).Distinct().ToList();
        }

        private string TrimAndRemovePunctuation(string str)
        {
            return new string(str.Trim().ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray());
        }

        private class CommitChanges
        {
            public List<string> OldLines { get; set; }

            public List<string> NewLines { get; set; }

            public CommitChanges()
            {
                OldLines = new List<string>();
                NewLines = new List<string>();
            }
        }
    }
}
