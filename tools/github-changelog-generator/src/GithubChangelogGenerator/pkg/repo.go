package pkg

import (
	"context"
	"fmt"
	"os"
	"strconv"
	"strings"
	"time"

	"github.com/google/go-github/github"
	"golang.org/x/oauth2"
)

type GitHubRepo struct {
	repoOwner string
	repoName  string
	token     string
	repo      *github.Repository
	milestone *github.Milestone
}

func NewRepo(repo string, token ...string) (*GitHubRepo, error) {
	gr := &GitHubRepo{}
	sx := strings.Split(repo, "/")
	if len(sx) != 2 {
		return nil, fmt.Errorf("repo must be in organization/repository format")
	}
	gr.repoOwner = sx[0]
	gr.repoName = sx[1]

	client := github.NewClient(nil)
	if token != nil && token[0] != "" {
		gr.token = token[0]
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: token[0]},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}

	var err error
	gr.repo, _, err = client.Repositories.Get(context.Background(), sx[0], sx[1])
	if err != nil {
		return nil, err
	}

	return gr, nil
}

func FilterMilestone(issues []*github.Issue, milestone string) []*github.Issue {
	var fi []*github.Issue
	if milestone != "" {
		for _, issue := range issues {
			if issue.Milestone.GetTitle() == milestone {
				fi = append(fi, issue)
			}
		}
	}
	return fi
}

func FilterSince(issues []*github.Issue, since time.Time) []*github.Issue {
	var fi []*github.Issue
	if !since.IsZero() {
		for _, issue := range issues {
			if issue.GetClosedAt().After(since) {
				fi = append(fi, issue)
			}
		}
	}
	return fi
}

func FilterUntil(issues []*github.Issue, until time.Time) []*github.Issue {
	var fi []*github.Issue
	if !until.IsZero() {
		for _, issue := range issues {
			if issue.GetClosedAt().Before(until) {
				fi = append(fi, issue)
			}
		}
	}
	return fi
}

func (gr *GitHubRepo) AllIssues(state ...string) ([]*github.Issue, error) {
	client := github.NewClient(nil)

	if gr.token != "" {
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: gr.token},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}

	var allIssues []*github.Issue
	opt := &github.IssueListByRepoOptions{State: "closed", ListOptions: github.ListOptions{PerPage: 100}}
	if len(state) == 1 {
		if state[0] == "open" || state[0] == "closed" || state[0] == "all" {
			opt.State = state[0]
		}
	}
	for {
		issues, resp, err := client.Issues.ListByRepo(context.Background(), gr.repoOwner, gr.repoName, opt)
		if err != nil {
			return nil, err
		}
		allIssues = append(allIssues, issues...)
		if resp.NextPage == 0 {
			break
		}
		opt.Page = resp.NextPage
	}
	return allIssues, nil
}

func (gr *GitHubRepo) IssuesByMilestone(milestone string, state ...string) ([]*github.Issue, error) {
	if milestone == "" {
		return nil, nil
	}
	client := github.NewClient(nil)

	if gr.token != "" {
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: gr.token},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}

	var allIssues []*github.Issue
	mil, err := gr.Milestone(milestone)
	if err != nil {
		return nil, err
	}
	opt := &github.IssueListByRepoOptions{Milestone: strconv.Itoa(mil.GetNumber()), State: "closed", ListOptions: github.ListOptions{PerPage: 100}}
	if len(state) == 1 {
		if state[0] == "open" || state[0] == "closed" || state[0] == "all" {
			opt.State = state[0]
		}
	}
	for {
		issues, resp, err := client.Issues.ListByRepo(context.Background(), gr.repoOwner, gr.repoName, opt)
		if err != nil {
			return nil, err
		}
		allIssues = append(allIssues, issues...)
		if resp.NextPage == 0 {
			break
		}
		opt.Page = resp.NextPage
	}
	return allIssues, nil
}

func (gr *GitHubRepo) IssuesSince(time time.Time, state ...string) ([]*github.Issue, error) {
	if time.IsZero() {
		return nil, nil
	}
	client := github.NewClient(nil)

	if gr.token != "" {
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: gr.token},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}

	var allIssues []*github.Issue
	opt := &github.IssueListByRepoOptions{Since: time, State: "closed", ListOptions: github.ListOptions{PerPage: 100}}
	if len(state) == 1 {
		if state[0] == "open" || state[0] == "closed" || state[0] == "all" {
			opt.State = state[0]
		}
	}
	for {
		issues, resp, err := client.Issues.ListByRepo(context.Background(), gr.repoOwner, gr.repoName, opt)
		if err != nil {
			return nil, err
		}
		allIssues = append(allIssues, issues...)
		if resp.NextPage == 0 {
			break
		}
		opt.Page = resp.NextPage
	}
	return allIssues, nil
}

func (gr *GitHubRepo) Tags() ([]*github.RepositoryTag, error) {
	client := github.NewClient(nil)
	if gr.token != "" {
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: gr.token},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}
	var allTags []*github.RepositoryTag
	opt := &github.ListOptions{PerPage: 100}
	for {
		tags, resp, err := client.Repositories.ListTags(
			context.Background(),
			gr.repoOwner, gr.repoName,
			opt,
		)
		if err != nil {
			return nil, err
		}
		allTags = append(allTags, tags...)
		if resp.NextPage == 0 {
			break
		}
		opt.Page = resp.NextPage
	}
	return allTags, nil
}

func (gr *GitHubRepo) Milestones() ([]*github.Milestone, error) {
	client := github.NewClient(nil)
	if gr.token != "" {
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: gr.token},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}
	var allMilestones []*github.Milestone
	opt := &github.MilestoneListOptions{State: "all", ListOptions: github.ListOptions{PerPage: 100}}
	for {
		milestones, resp, err := client.Issues.ListMilestones(
			context.Background(),
			gr.repoOwner, gr.repoName,
			opt,
		)
		if err != nil {
			return nil, err
		}
		allMilestones = append(allMilestones, milestones...)
		if resp.NextPage == 0 {
			break
		}
		opt.Page = resp.NextPage
	}
	return allMilestones, nil
}

func (gr *GitHubRepo) Milestone(title string) (*github.Milestone, error) {
	if title == "" {
		return nil, nil
	}
	if gr.milestone.GetTitle() == title {
		return gr.milestone, nil
	}
	milestones, err := gr.Milestones()
	if err != nil {
		return nil, err
	}
	for _, milestone := range milestones {
		if milestone.GetTitle() == title {
			gr.milestone = milestone
			return milestone, nil
		}
	}
	return nil, fmt.Errorf("you didn't pass a valid milestone title")
}

func (gr *GitHubRepo) TagCommit(name string) (*github.Commit, error) {
	if name == "" {
		return nil, nil
	}
	client := github.NewClient(nil)
	if gr.token != "" {
		ts := oauth2.StaticTokenSource(
			&oauth2.Token{AccessToken: gr.token},
		)
		tc := oauth2.NewClient(context.Background(), ts)
		client = github.NewClient(tc)
	}

	refs, _, err := client.Git.GetRefs(context.Background(), gr.repoOwner, gr.repoName, "tags")
	if err != nil {
		return nil, err
	}

	sha := ""
	var tags []string

	refName := fmt.Sprintf("refs/tags/%s", name)
	for _, ref := range refs {
		if ref.GetRef() == refName {
			sha = ref.Object.GetSHA()
		}
		tag := strings.Split(ref.GetRef(), "/")
		tags = append(tags, tag[len(tag)-1])
	}

	if sha == "" {
		return nil, fmt.Errorf("you didn't pass a valid tag name. the available tags are: %s", tags)
	}

	commit, _, err := client.Git.GetCommit(context.Background(), gr.repoOwner, gr.repoName, sha)
	if err != nil {
		return nil, err
	}

	return commit, err
}

func (gr *GitHubRepo) Repository() *github.Repository {
	return gr.repo
}

func checkrate() error {
	client := github.NewClient(nil)
	rl, _, err := client.RateLimits(context.Background())
	if err != nil {
		return err
	}
	if rl.Core.Remaining == 0 {
		fmt.Println("GitHub API rate limit exceeded!")
		fmt.Printf("GitHub API rate limit resets on %s\n", rl.Core.Reset.Format("2-Jan-2006 15:04:05"))
		os.Exit(1)
	}
	return nil
}
