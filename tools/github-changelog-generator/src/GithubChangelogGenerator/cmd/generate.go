package cmd

import (
	"GithubChangelogGenerator/internal"
	"GithubChangelogGenerator/pkg"
	"fmt"
	"io"
	"os"

	"github.com/google/go-github/github"
	"github.com/spf13/cobra"
	"github.com/spf13/viper"
)

// generateCmd represents the generate command
var generateCmd = &cobra.Command{
	Use:   "generate",
	Short: "Generate changelog",
	Long: `Generates changelog based on a milestone or tag(s)
Edit the config.yaml file to customize the generated output.`,
	Run: func(cmd *cobra.Command, args []string) {
		c := &internal.Config{}
		err := viper.Unmarshal(c)
		if err != nil {
			er(err)
		}

		repo, err := pkg.NewRepo(c.Repo, c.Token)
		if err != nil {
			er(err)
		}

		issuesByMilestone, err := repo.IssuesByMilestone(c.Milestone, c.State)
		if err != nil {
			er(err)
		}
		milestone, err := repo.Milestone(c.Milestone)
		if err != nil {
			er(err)
		}
		sinceTagCommit, err := repo.TagCommit(c.SinceTag)
		if err != nil {
			er(err)
		}
		untilTagCommit, err := repo.TagCommit(c.UntilTag)
		if err != nil {
			er(err)
		}

		var issuesByTag []*github.Issue
		switch {
		case sinceTagCommit != nil:
			issuesByTag, err = repo.IssuesSince(sinceTagCommit.Committer.GetDate())
			if err != nil {
				er(err)
			}
			if c.UntilTag != "" {
				issuesByTag = pkg.FilterUntil(issuesByTag, untilTagCommit.Committer.GetDate())
			}
		case untilTagCommit != nil:
			issuesByTag, err = repo.AllIssues(c.State)
			if err != nil {
				er(err)
			}
			issuesByTag = pkg.FilterUntil(issuesByTag, untilTagCommit.Committer.GetDate())
		}

		groupedIssuesByMilestone := internal.GroupIssues(c.Groups, issuesByMilestone)
		if err != nil {
			er(err)
		}

		groupedIssuesByTag := internal.GroupIssues(c.Groups, issuesByTag)
		if err != nil {
			er(err)
		}

		err = writeChangelog(os.Stdout, &TemplateData{
			Repository:        repo.Repository(),
			IssuesByMilestone: groupedIssuesByMilestone,
			IssuesByTag:       groupedIssuesByTag,
			Milestone:         milestone,
			SinceTag:          c.SinceTag,
			SinceTagCommit:    sinceTagCommit,
			UntilTag:          c.UntilTag,
			UntilTagCommit:    untilTagCommit,
		})
		if err != nil {
			er(err)
		}
	},
}

func init() {
	rootCmd.AddCommand(generateCmd)
	generateCmd.Flags().StringP("repo", "r", "", "repository name to generate the Changelog for, in the form user/repo")
	generateCmd.Flags().StringP("token", "t", "", "personal access token")
	generateCmd.Flags().StringP("milestone", "m", "", "milestone title to get issues and pull requests for")
	generateCmd.Flags().String("since-tag", "", "issues and pull requests since tag")
	generateCmd.Flags().String("until-tag", "", "issues and pull requests until tag")
	generateCmd.Flags().StringP("state", "s", "", "state of the issues and pull requests to get (open,closed or all)")

	err := viper.BindPFlags(generateCmd.Flags())
	if err != nil {
		er(err)
	}
}

type TemplateData struct {
	Repository        *github.Repository
	IssuesByMilestone []*internal.GroupedIssues
	IssuesByTag       []*internal.GroupedIssues
	Milestone         *github.Milestone
	SinceTag          string
	SinceTagCommit    *github.Commit
	UntilTag          string
	UntilTagCommit    *github.Commit
}

func writeChangelog(w io.WriteCloser, td *TemplateData) error {
	template := viper.GetString("template")
	if template == "" {
		template = `{{if .Milestone}}## {{.Milestone.GetTitle}} ({{.Milestone.GetClosedAt.Format "2006-01-02"}}){{end -}}
{{if .IssuesByMilestone}}
{{range .IssuesByMilestone}}
### {{.Title}}
{{range .Issues}}
{{if .IsPullRequest -}}
- PR [\#{{.GetNumber}}]({{.GetHTMLURL}}): {{.GetTitle}} (by [{{.GetUser.GetLogin}}]({{.GetUser.GetHTMLURL}}))
{{- else -}}
- ISSUE [\#{{.GetNumber}}]({{.GetHTMLURL}}): {{.GetTitle}}
{{- end -}}
{{end}}
{{end}}
{{end -}}
{{if .SinceTagCommit}}## {{.SinceTag}}{{if .UntilTagCommit}} - {{.UntilTag}}{{end}}{{end -}}
{{if .IssuesByTag}}
{{range .IssuesByTag}}
### {{.Title}}
{{range .Issues}}
{{if .IsPullRequest -}}
- PR [\#{{.GetNumber}}]({{.GetHTMLURL}}): {{.GetTitle}} (by [{{.GetUser.GetLogin}}]({{.GetUser.GetHTMLURL}}))
{{- else -}}
- ISSUE [\#{{.GetNumber}}]({{.GetHTMLURL}}): {{.GetTitle}}
{{- end -}}
{{end}}
{{end}}
{{- end -}}
`
	}

	result, err := executeTemplate(template, td)
	if err != nil {
		return err
	}
	_, err = fmt.Fprintf(w, "%s", result)
	if err != nil {
		return err
	}

	return w.Close()
}
