On the same tasks, with the same model, ABP Studio's AI agent sent 33–78% fewer input tokens than Claude Code and opencode, and needed fewer model calls to finish. It did not do less work: all three agents delivered the same feature across the backend and four client apps. The rest of this article explains where the difference comes from.

## The results

We ran each agent headless on a real ABP solution, a social app with a mobile API, a portal, a Blazor admin and a public website. All three used the same model and reasoning effort, and each started from a fresh clone of the same commit.

**Task 1: add a feature (Claude Opus 5.5).** Add "People you may know" friend suggestions to the backend and every client app.

| Agent | Model calls | Input tokens | Files changed |
| --- | --: | --: | --: |
| ABP Studio | **61** | **6.84M** | 45 |
| Claude Code | 74 | 10.13M | 40 |
| opencode | 85 | 14.22M | 37 |

**Task 2: explain existing code (Claude Sonnet 5.5).** Answer "how does the friendship system work?" without changing anything.

| Agent | Model calls | Input tokens |
| --- | --: | --: |
| ABP Studio | **4** | **95K** |
| Claude Code | 7 | 425K |
| opencode | 8 | 354K |

In Task 1 all three agents reached the same end state: the feature in the backend and all four apps, the host projects building, and the same pre-existing test-config gap reported.

## Where the tokens go

An agent resends the whole conversation on every model call: the system prompt, the tool definitions and every tool result so far. Total input is therefore the context size summed over all calls, or roughly calls × average context.

| Agent (Task 1) | Model calls | Average context per call | Total input |
| --- | --: | --: | --: |
| ABP Studio | 61 | 112K | 6.84M |
| Claude Code | 74 | 137K | 10.13M |
| opencode | 85 | 167K | 14.22M |

That gives an agent two levers, and ABP Studio pulls both about equally. Against Claude Code, roughly half of the gap comes from fewer calls and half from a smaller average context. Against opencode the split is 45% and 55%.

![](images\image1.png)

The calls near the end are the heaviest, because each carries everything gathered before it. Every extra call there sends 150–240K tokens, so finishing in fewer late calls matters more than trimming the first ones.

## 1. It knows the solution before the first call

ABP Studio's first model call already asked for specific domain types by package. The other agents spent their first calls finding out what the solution contains.

| Agent | First calls in Task 1 | Output |
| --- | --- | --: |
| ABP Studio | `get_outlines` for `Friendship`, `FriendRequest`, `FriendshipAppService`, `UserBlock` and `UserProfile` | 10.6K chars |
| Claude Code | `ls`, `git ls-files`, `cat Volo.DailyTalking.slnx`, `git ls-files \| grep -i friend` | 12.9K chars |
| opencode | `git ls-files src \| Select-String Friend` plus file listings of the admin projects, in one command | 13.4K chars |

In Task 2, opencode's second call tried `rg`, which was not installed. Its third listed every file mentioning "friend" and overflowed its output limit.

Studio can skip this step because it analyzes the solution before the agent starts:

- **It reads the compiled code.** A small analyzer loads the solution's assemblies and inspects them with reflection. ABP-aware contributors pick out aggregate roots, entities, application services, DbContexts, permissions, events and mappings. The result is stored per package and only refreshed when the package changes.
- **It gives the model a map of the solution.** Every module and package is listed with its role, its path and the main types it contains.

This map is not free. Studio's first call carried 25.8K tokens, the same as Claude Code's and more than opencode's 11.0K. Studio pays that up front so that later calls go straight to the code that matters.

## 2. It reads summaries first, then only the lines it needs

`get_outlines` gives the model a type's shape without its code. One call can cover many types, and each comes back as a short, structured summary. This is part of what Studio returned for the app service in its first call:

```text
## FriendshipAppService (ApplicationService) - Volo.DailyTalking.Mobile.Application
File: src/Volo.DailyTalking.Mobile.Application/Friendships/FriendshipAppService.cs
Declaration: FriendshipAppService : DailyTalkingMobileAppServiceBase, IFriendshipAppService
Attributes: [Authorize]

Methods:
- GetStatusAsync(userId:Guid) -> FriendshipStatusDto  [L:50-95]
- SendRequestAsync(input:SendFriendRequestInput) -> SendFriendRequestResultDto  [L:97-188]
- AcceptRequestAsync(requestId:Guid) -> FriendDto  [L:210-262]
- GetFriendsAsync(input:GetFriendsInput) -> FriendsListResultDto  [L:302-346]
...
```

For entities it adds the primary key, base types, and properties with their doc comments. The facts come from the analysis model, and the line ranges from a syntax-only Roslyn parse of the one file. Method bodies are left out.

The line ranges keep the next step small. When Studio later needed the friend-request rules, it read lines 97–190 of that 577-line file, not the whole file. Of the 25 file views in Task 1, 12 were explicit line ranges.

There is a safety net for when the model forgets. `read_files` returns only the first 100 and last 20 lines of a file over 500 lines when no range is given, with a note on how to read the rest. That happened twice in Task 1.

The other agents learned the same types by reading source files whole. Claude Code's first reads were `cat` over whole folders: 19.4K, 26.5K and 13.2K characters in three calls. Studio's outline of five types was 10.6K characters in one call.

## 3. It researches first, then writes in batches

ABP Studio read about as often as the others: 42 read-only calls, against 48 and 51. The difference is when. It read first, and then wrote with few interruptions. After its first edit it finished in 29 model calls. Claude Code needed 60 and opencode 65, because they kept going back to read while writing.

![](images\image2.png)

This is how Studio's agent is designed to work: gather what it needs first, send independent edits together in one response, and trust an edit's success reply instead of re-reading the file.

The batching shows in the numbers. Studio made 1.85 tool calls per model call, against 1.26 for Claude Code and 1.56 for opencode. The backend's 11 translation files went out in one model call, and the mobile app's 11 language files in another.

Batching saves model calls, not time. Studio runs the tool calls of one response one after another, and its wall-clock time matched Claude Code's: 12m 20s against 12m 17s.

## 4. Its tools return less text

Every character a tool returns stays in the context for the rest of the run. Studio's tools returned the least, so its context grew the slowest.

| Task 1 | ABP Studio | Claude Code | opencode |
| --- | --: | --: | --: |
| Tool output, whole run | 218K chars | 265K chars | 273K chars |
| Context on the last call | 178K tokens | 198K tokens | 237K tokens |

Besides outlines and windowed reads, three tool designs keep the output short:

- **Search with limits.** `search_code` shows 5 lines of context and at most 10 matches per file by default, with hard caps of 80 matches and 30,000 characters. It skips build output and `node_modules`, and leaves out files the agent has already read in full. Studio's 33 searches in Task 1 averaged 1.7K characters.
- **A build tool that reports only what matters.** A successful `dotnet_build` returns one line. A failed one keeps the error lines and drops the rest. Studio's whole-solution build, which failed on the missing test config file, came back in 584 characters. Claude Code and opencode ran `dotnet build` in a shell and had to filter its output themselves with `grep` or `Select-String`.
- **Edits that answer in one line.** `edit_file_batch` applies up to 20 edits to a file and replies "Success: Applied N/N edits". Studio's agent trusts that reply and does not re-read the file. The other agents' edit tools are just as terse, so the saving comes from not re-reading, not from the reply itself.

## What does not explain the difference

Several plausible explanations turn out not to apply to these runs:

- **A smaller system prompt.** Studio's starting prompt is not small. At 25.8K tokens it matched Claude Code's and was more than twice opencode's.
- **Prompt caching.** In Task 1, all three agents served 97–98% of their input from the provider's prompt cache. Caching works the same way for every agent and does not change how many tokens are sent.
- **Context compression.** Studio can shorten old tool results in long sessions, but only near 90% of the context window. Its context peaked at 178K tokens, so compression never ran.
- **Subagents.** Studio can hand research to parallel subagents. It did not use them in either task.

The gains also depend on what kind of solution the agent is working in. The workspace map and `get_outlines` are built from Studio's analysis of ABP solutions. In a plain .NET solution the per-package summaries are skipped, and that part of the advantage shrinks. ABP Studio is efficient here because it understands how an ABP application is put together.

## How we measured

Both tasks ran on our agent benchmark harness. It drives each agent headless and records its raw event stream.

- **Setup.** Each agent worked in its own fresh clone of the same commit. All three used the same model, medium reasoning effort, a 30-minute timeout and the same permission mode.
- **Tokens.** Input tokens are the full prompt of every call, including tokens read from or written to the prompt cache. They come from each agent's own usage reports.
- **Model calls.** We counted model calls directly from the event streams. Claude Code's own `num_turns` counts more than its model calls (94 against 74 in Task 1), so we did not use it.

These results come from one run of each task per agent, on one ABP solution. Agent runs vary from one run to the next, so the exact percentages will move. The mechanisms behind them come from how Studio's agent and tools are designed, and showed up the same way in both tasks.
