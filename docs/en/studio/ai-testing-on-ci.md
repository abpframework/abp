```json
//[doc-seo]
{
    "Description": "Run the ABP Studio UI/UX Test and Security Test from the ABP CLI with abp test, and set them up on GitHub Actions with abp test init-ci."
}
```

# ABP Studio: Running the AI Tests from the CLI and on CI

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Security Test",
    "Path": "studio/security-test"
  },
  "Next": {
    "Name": "Concepts",
    "Path": "studio/concepts"
  }
}
````

The [UI/UX Test](ui-ux-test.md) and the [Security Test](security-test.md) also run without the Studio UI, with the `abp test` command of the [ABP CLI](../cli/index.md). They read the same [inputs files](ai-testing.md#the-inputs-file) as Studio, write their reports to a folder, and end with an exit code, so they can run on a developer machine or on CI.

## Requirements

- The ABP CLI and the ABP Studio agent server, which runs the tests:

  ```bash
  dotnet tool install -g Volo.Abp.Studio.Cli
  dotnet tool install -g Volo.Abp.Studio.AgentServer
  ```

- A signed-in ABP account with AI access. On a developer machine, sign in with `abp login`. On CI, set the `ABP_ACCESS_TOKEN` environment variable (see [Setting Up GitHub Actions](#setting-up-github-actions)); for an account without two-factor authentication, `ABP_USERNAME` and `ABP_PASSWORD` (and `ABP_ORGANIZATION`) also work.
- For the runtime part, the applications running, or `--start` to start them.
- The test accounts' passwords in the environment variables the inputs file names (`passwordEnv`).

## Running a Test

```bash
abp test ui-ux --solution MyCompany.MyProject.abpsln --mode both --start
abp test security --solution MyCompany.MyProject.abpsln --mode both --start
```

| Option | Description |
| --- | --- |
| `--solution` | The `.abpsln` file. Default: the one in the current folder. |
| `--mode` | `codebase` (reviews the code), `runtime` (tests the running applications) or `both`. Default: the inputs file's mode, else `both`. |
| `--url Name=<url>[,Name=<url>]` | The running applications to test. Default: the `targets` of the inputs file. |
| `--start` | Starts the solution's default run profile before the test, and stops what it started afterwards. |
| `--profile` | The run profile `--start` uses, instead of the default one. |
| `--changed-since` | Reviews only the code changed since a git ref (`origin/main`), a date (`2026-11-01`) or a time span (`7d`, `2w`, `12h`, `3mo`). |
| `--model` | The model to test with. Default: the inputs file's model, else the default model. |
| `--output` | The report folder. Default: `TestResults/ui-ux` or `TestResults/security`. |
| `--fail-on` | Which findings fail the test (exit code `1`). See below. |
| `--verbose` | Writes detailed logs. |

`--fail-on` values:

| Test | Values | Default |
| --- | --- | --- |
| `ui-ux` | `defect` (any defect; improvements never fail the test), `confirmed` (confirmed defects only), `high` (High and Critical defects), `none` | `defect` |
| `security` | `high` (High and Critical findings), `any`, `confirmed` (confirmed findings only), `none` | `high` |

### Starting the applications

With `--start`, the test does what the [Solution Runner](running-applications.md) does on a clean machine: it starts the run profile's Docker containers, runs the tasks that prepare the solution (such as the database migration), builds the solution once, then starts the applications in order. If a container, a task, the build or an application fails, the test stops before testing (exit code `2`). Everything it started is stopped when the test ends.

Free the applications' ports first: an application already holding one is stopped.

### Testing only the changes

`--changed-since` limits the code review to the files changed since a point in the history, including uncommitted and untracked files. Findings in other files are not reported. The running applications are still tested in full, since a changed file does not tell which pages it changes.

| Value | Reviews the files changed |
| --- | --- |
| A branch, tag or commit, such as `origin/main` | since the branch point with that ref, as a pull request shows them |
| A date, such as `2026-11-01` | since the last commit before that date |
| A time span, such as `7d`, `2w`, `12h` or `3mo` | since the last commit before that time (days, weeks, hours, months) |

A date and a time span are resolved from the git history, so they also work on CI, where the checkout has the full history (`fetch-depth: 0`). The test prints the commit it counted from.

### Stopping a test

Ctrl+C stops the test after the step it is on, as *Stop Test* does in Studio: the reports are written with what was found until then, and the exit code is `3`. Press Ctrl+C again to end it at once, without reports. Canceling a CI job stops the test the same way.

The reports of a stopped test say so: a line under the title of the Markdown report, `"stoppedEarly": true` and `stopReason` in the JSON report, and the same two fields in the `properties` of the SARIF run. A stopped test cannot be continued from the CLI; the next run starts from the beginning.

## Reports

| Test | Files |
| --- | --- |
| `ui-ux` | `ui-ux-test.json` and `ui-ux-test.md`, with the screenshots beside them |
| `security` | `security-test.json`, `security-test.md` and `security-test.sarif` |

## Exit Codes

| Code | Meaning |
| --- | --- |
| `0` | Passed. |
| `1` | Findings past `--fail-on`. |
| `2` | Error: the test could not run or finish (for example, the applications did not start, the sign-in failed, or the code review failed). |
| `3` | The test stopped before the end: the AI stopped it (for example, no credits left), or it was stopped with Ctrl+C. The reports hold what was found until then. |
| `4` | The AI cannot be used: sign in, or check the license. |

## Setting Up GitHub Actions

`abp test init-ci` sets a GitHub repository up to run both tests:

```bash
abp test init-ci --solution MyCompany.MyProject.abpsln
```

It:

1. Creates a token for CI through your browser (two-factor authentication included). Your own sign-in on the machine stays as it is. The command prints the date the token expires; run it again before then.
2. Sets the token as the repository's `ABP_ACCESS_TOKEN` secret with the [GitHub CLI](https://cli.github.com/) (`gh`).
3. Writes `.github/workflows/abp-tests.yml`. Commit it.

| Option | Description |
| --- | --- |
| `--solution` | The `.abpsln` file. Default: the one in the current folder. |
| `--repo <owner/name>` | The repository to set the secret on. Default: the repository of the solution folder. |
| `--mode` | `both` (default): the workflow starts the applications and tests them with the code. `codebase`: it reviews the code only. |
| `--force` | Replaces an existing workflow file. Without it, an existing file is not changed. |
| `--show-token` | When `gh` is not installed, prints the token so that you can add the secret yourself. |

The workflow runs the UI/UX Test and the Security Test as two jobs:

- **On a pull request**, the code review covers only the files the pull request changes (`--changed-since origin/<base branch>`). The started applications are tested in full.
- **Weekly** (Monday 03:00 UTC) and **on demand**, the whole code is reviewed.
- The reports are uploaded as build artifacts, and the Security Test's findings are sent to GitHub code scanning, where they show in the pull request and the *Security* tab. Private repositories need GitHub Advanced Security for that.
- Pull requests from forks and from Dependabot are skipped, because GitHub does not give them the repository's secrets.

Each run uses AI credits of the account behind `ABP_ACCESS_TOKEN`. Remove the `schedule` trigger to run the tests on demand and on pull requests only.

> With `--mode both`, the workflow builds and starts the applications of the solution's default run profile on the runner. If they need a database or another service the runner does not have, add it to the workflow, or run `abp test init-ci --mode codebase --force` to review the code only.

To test with test accounts on CI, add each account's password as a repository secret, and set the environment variable the inputs file names in the test step:

```yaml
- name: UI/UX test
  env:
    ABP_ACCESS_TOKEN: ${%{{{ secrets.ABP_ACCESS_TOKEN }}}%}
    ABP_TEST_ADMIN_PASSWORD: ${%{{{ secrets.ABP_TEST_ADMIN_PASSWORD }}}%}
```

## See Also

- [AI Testing](ai-testing.md)
- [UI/UX Test](ui-ux-test.md)
- [Security Test](security-test.md)
- [ABP CLI](../cli/index.md#test)
