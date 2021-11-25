package internal

import (
	"fmt"
	"regexp"
	"sort"

	"github.com/google/go-github/github"
)

type GroupedIssues struct {
	Title  string
	Issues []*github.Issue
}

func GroupIssues(groups []*Group, issues []*github.Issue) []*GroupedIssues {
	if issues == nil {
		return nil
	}
	var result []*GroupedIssues
	grouped := make(map[string][]*github.Issue)

	for _, issue := range issues {
		if i, ok := containsAny(issue.Labels, AllLabels(groups)); ok {
			grouped[groups[i].Title] = append(grouped[groups[i].Title], issue)
		} else {
			grouped["no_label"] = append(grouped["no_label"], issue)
		}
	}

	for _, group := range groups {
		if len(group.Labels) == 0 {
			result = append(result, &GroupedIssues{Title: group.Title, Issues: grouped["no_label"]})
			continue
		}
		result = append(result, &GroupedIssues{Title: group.Title, Issues: grouped[group.Title]})
	}

	return result
}

func containsAny(gls []github.Label, cls map[int][]string) (int, bool) {
	var keys []int
	for k := range cls {
		keys = append(keys, k)
	}
	sort.Ints(keys)

	for _, gl := range gls {
		for _, k := range keys {
			for _, l := range cls[k] {
				if match(gl.GetName(), l) {
					return k, true
				}
			}
		}
	}
	return 0, false
}

func match(a, rx string) bool {
	if a == rx {
		return true
	}
	re, err := regexp.Compile(fmt.Sprintf("^%s$", rx))
	if err != nil {
		return false
	}
	return re.MatchString(a)
}
