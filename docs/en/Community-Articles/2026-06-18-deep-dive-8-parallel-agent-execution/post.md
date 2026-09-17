# Deep Dive on ABP AI Agent #8: Parallel Agent Execution

Real work rarely lines up one task at a time.

While the agent is busy adding a feature, a question comes up about a different module. A review is waiting. A small fix would take two minutes, but the agent is in the middle of something else. With a single session, you wait. You watch one task finish before you can start the next, even when the two have nothing to do with each other.

ABP Studio does not make you wait. It can run several agent sessions at the same time, each on its own task.

## Parallel Agent Sessions

A session is one conversation with the agent. It has its own history, its own mode, its own model, its own scope, and its own workflow. ABP Studio keeps multiple sessions per solution, and they can run in parallel.

There is a limit, and it is there on purpose. By default you can run up to **3 sessions at once**, and you can set that anywhere from **1 to 5**. When you send more prompts than there are open slots, the extra ones are queued and start as soon as a slot frees up.

So "parallel" here is not a trick of switching back and forth quickly. The sessions actually run at the same time, up to the limit you choose.

## Parallel Session Use Cases

The point is to stop letting one task block another. A few ways it plays out:

* One session implements a feature while another answers questions about a different part of the solution.
* Two unrelated modules get worked on at once, each in its own session.
* One session writes the code while another reviews a diff or does research.

Because each session is separate, you can even point them at different parts of the solution and give them different jobs:

```text
Session 1 (Agent): Add a Category screen to the Catalog module.
Session 2 (Ask): Explain how permissions flow from the API to the UI.
```

The first one edits files. The second one only reads and answers. They do not interfere, because they are different sessions with different settings.

## Per-Session Settings Isolation

This is the part that makes parallel work predictable instead of chaotic.

When a session sends its first message, two things happen at different levels. The session permanently locks its **scope** and **workflow**—these stay fixed for the entire session lifetime and cannot be changed once the first message is sent. On the other hand, the **model**, **mode**, and **active tools** are snapshotted fresh at each run (each turn the agent takes), so they reflect whatever is configured at the moment the run starts but remain stable for its duration.

That matters the moment you have more than one session open. Say one session is running in the background, scoped to the `Catalog` module. You switch the foreground to a different scope to start a second task. The background session does not notice. It keeps the scope and workflow it was locked to, and each of its runs uses whatever model and tools were configured at the moment that run began.

Without this, parallel sessions would quietly corrupt each other every time you changed a setting. With it, each session is a sealed unit of work.

## The Prompt Queue

You do not have to wait for a session to be idle to line up its next step.

While a session is running, you can queue more prompts on it. They attach to the same session and are sent one after another, each after the current turn finishes. The queue keeps the session's settings, so a queued prompt runs with the same mode, scope, model, workflow, and active plan as the rest of that session.

This works together with the concurrency limit. Prompts that cannot start right away, because every slot is busy, simply wait their turn instead of failing.

## Keeping Parallel Work Safe

Running several sessions at once is powerful, and it also gives you a new way to get in your own way: two sessions editing the same files.

ABP Studio helps here, but it does not pretend the problem away. It tracks file changes, so if one session edited a file after another session read it, the second is told to read it again before overwriting. It also serializes operations that cannot safely overlap, such as adding a migration while a build is running, so two sessions do not run conflicting `dotnet` commands at the same time. And because each session tracks its own pending questions, one session waiting on your input does not freeze the others.

Still, the simplest rule is the best one: keep parallel Agent-mode sessions on separate parts of the solution. This is exactly where scopes earn their place. Give each session its own scope, and they stay in their own lanes by design instead of by luck.

## A Second Kind Of Parallel: Subagents

There is also a smaller, quieter form of parallel work that happens inside a single session.

When the agent needs to look something up, it can fan out multiple **subagents** in a single turn: read-only helpers that research in parallel and return their results before the main agent continues. There are a few kinds, each with a narrow job:

* one that researches your solution and code,
* one that searches the web,
* one that searches and reads the official ABP documentation.

These research-style subagents (code research, web search, documentation search) are read-only—they cannot modify files or solution state. They only read, gather, and hand back a short summary. That keeps the main session's context clean, because the digging happens elsewhere and only the answer comes back. (Note that the browser subagent is an exception: it is stateful and can mutate the shared browser session.)

ABP Studio can use a separate model for research subagents, configured apart from your main one. A fast, cheap model is the right choice for this kind of lookup work.

![Model Settings: the research model used by subagents is set separately from the main model](model-settings.png)

So there are two scales of parallel here. Sessions run independent tasks side by side. Subagents run read-only research inside one task. Both keep the slow parts from blocking the useful parts.

## ABP Studio Parallel Execution Model

Plenty of tools let you open more than one chat, and that is genuinely useful. So the idea of running agents in parallel is not, by itself, an ABP feature.

The difference is that each session here carries the context of an ABP solution and stays inside it. A session locks its scope and workflow permanently, and snapshots its model and tools per run, so background work does not drift when you change the foreground. Studio coordinates the operations that would otherwise collide, like builds and migrations, across all the running sessions. And scopes give each session a clear boundary, so parallel does not turn into a pile of agents editing the same files.

In short, the parallelism is not just several chat windows. It is several controlled, solution-aware sessions that know how to stay out of each other's way.

## Conclusion

Parallel execution is about respecting how work actually arrives: more than one thing at a time, often unrelated.

ABP Studio lets you run several agent sessions together, each sealed to its own settings, with a queue for what comes next and guardrails for the places where parallel work could collide. Inside each session, subagents add a second layer of parallel research without cluttering the main task.
