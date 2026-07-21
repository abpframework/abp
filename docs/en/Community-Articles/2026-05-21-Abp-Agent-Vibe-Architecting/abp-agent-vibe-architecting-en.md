# The Antidote to Vibe Architecting: ABP Studio AI Agent

Recent discussions in software engineering have started pointing at a quiet but critical side effect of AI-assisted development. The uncomfortable truth is simple: **AI agents no longer just write code, they make architectural decisions, yet almost no one reviews those decisions as architecture.**

Here is a striking observation: for the *same task*, changing nothing but the wording of your prompt can produce a system whose line count and file count grow several times over. In other words, the words in your prompt shape the architecture of the system. The phenomenon even has a name: **vibe architecting**, architecture that emerges from prompts rather than from deliberate, recorded design.

In this article I will first lay out the problem, and then show why **ABP Studio AI Agent** is designed precisely to mitigate it.

![abp-agent-code-generation](abp-agent-code-generation.gif)

---

## What Is "Vibe Architecting"?

The **vibe coding** popularized by Andrej Karpathy (describing what you want in plain language and letting the AI write the code) has moved far beyond single-line autocomplete. Today's agents spin up entire systems from a single sentence of description. And there is a further step: while writing code, the agent is also **choosing the architecture.**

We can identify five main **mechanisms** through which agents make hidden architectural decisions:

1. **Model selection:** Different LLMs produce structurally different code; switching the model selector is itself an architectural choice.
2. **Task decomposition:** How the agent splits work into subtasks determines the module boundaries of the system.
3. **Default configuration:** Without explicit rules, the agent drifts toward defaults inherited from its training data.
4. **Scaffolding and autonomous generation:** A single prompt folds every framework, database, auth, and deployment choice into one interaction, with no visible rationale.
5. **Integration protocols:** How the system connects to the outside world is chosen by the agent or the tool, not by the team.

Three properties set these decisions apart from human ones:

- **Scale:** Framework, database, authentication, and deployment are selected in a single interaction, bundled together rather than as separately reviewable choices.
- **Speed:** Decisions a team would debate for days happen in **seconds**, faster than any review process can keep up with.
- **Opacity:** The decisions are buried inside the generated code: no ADR, no design document, no recorded rationale.

This has two concrete consequences. The first is the **speed-review gap**: the agent builds a system in minutes, while the team needs hours or days to audit it. The second is **convergence onto narrow stacks**: agent-based tools default to the same stack again and again (e.g. React/TypeScript/Tailwind), which concentrates the security attack surface. In short, these decisions take seconds, arrive bundled, and leave no record behind.

---

## The Bridge: The Risk Is Far Greater in Enterprise .NET

The examples behind these discussions usually revolve around small chatbots. But carry the same mechanisms over to an **enterprise, modular, distributed** solution and the risk multiplies. A hidden architectural decision now looks like this:

- An `ApplicationService` depending directly on `DbContext` (a layering violation),
- Raw data access instead of a repository,
- A hand-written `[Authorize(Roles=…)]` instead of the ABP permission system,
- Hardcoded text instead of localized strings,
- A wrong module dependency, or a flawed event/flow design.

None of these stand out in a small prototype; but in an enterprise system with dozens of modules and many services, they turn into **technical debt that piles up unseen.** And this debt starts accumulating before the code even runs, right at the moment of production.

---

## The Prescription: A Three-Layer Governance Framework

A three-layer framework that maps existing tool mechanisms onto classic software-architecture concepts is a sensible answer to this problem:

- **Layer 1, Constraints:** Defines what the agent may and may not do. Today, instruction files (AGENTS.md, .cursorrules) and MCP configurations play this role informally; in architecture terms, the equivalent is ADLs and Attribute-Driven Design.
- **Layer 2, Conformance:** Checks the generated code against those constraints. Plan-build flows (the agent proposes before acting) and post-generation hooks are the counterpart of *fitness functions* in evolutionary architecture.
- **Layer 3, Knowledge:** Feeds architectural context back to the agent. Today, repository maps and context files; in architecture terms, ADRs and Architectural Knowledge Management.

One caveat worth noting: tools that deliver all three layers together, proactively, are still **largely missing** today. And that is exactly where it gets interesting.

---

## ABP Studio AI Agent: The Prescription, Implemented

ABP Studio AI Agent is not a general-purpose code agent; it is an in-IDE agent that understands ABP solutions *as systems*. Look at its design and you will see it covers all three layers above with surprising clarity.

### Layer 1, Constraints: ABP-Aware by Default

The first layer calls for "a constraint layer that tells the agent what it may do." ABP Agent brings this as default behavior. By instruction, the agent prefers: **ABP base classes** over plain POCOs, **repositories** over direct `DbContext`, **`ApplicationService`** over plain services, the **ABP permission system** over `[Authorize(Roles=…)]`, **localized strings** over hardcoded text, **`BusinessException`/`UserFriendlyException`** over plain `Exception`, and the **distributed cache** abstraction over raw in-memory cache. When it is unsure about an ABP feature, it consults the **official ABP documentation** as the authoritative source, not random blog posts.

On top of that, **Custom Workflows** define the deterministic steps that must run before and after every agent turn, and they can be shared with the team. So the constraints don't live in one developer's head; they live inside the solution.

### Layer 2, Conformance: Plan Mode + ABP-Aware Review

![abp-agent-ai-review](abp-agent-ai-review.gif)

The second layer calls for "plan-build flows and fitness-function-like checks." ABP Agent has two concrete answers to this:

- **Plan mode:** The agent first inspects the solution in read-only mode, consults the ABP docs, and produces a structured implementation plan (Problem, Solution, Workflow Diagram, Files Affected, Expected Result). Once you approve it, the plan turns into implementation with a single click. This is exactly the "propose before acting" flow the framework asks for.
- **ABP-aware AI code review:** This is not a generic review; it catches **ABP-specific pattern violations**: POCOs in the wrong place, direct `DbContext` injection, hardcoded strings, plain exceptions, role-based authorization, and so on. When unsure, it checks the official ABP docs. This is the concrete form of a **fitness function** that audits generated code against the constraints.

### Layer 3, Knowledge: Analyze Engine + Lessons

![abp-agent-analyze-engine](abp-agent-analyze-engine.png)

The third layer calls for "a knowledge layer that feeds architectural context back to the agent": repository maps, ADRs. ABP Agent's **Analyze engine** is a higher-level version of this: the moment you open a solution, it scans every package and produces a **typed, ABP-role-aware** structural map. It knows what each type actually is: an aggregate root, a repository, an application service, a DTO, an ETO, a permission provider. The agent receives this map at the start of every session; it doesn't look at folders and guess, it **knows** the structure of the solution.

Add to that **lessons**: when the agent makes a mistake and gets corrected, it records the correction as a short, verified note and carries it into future sessions. This is a living counterpart to the ADR/AKM idea of persisting decisions and their rationale.

---

## The Problem → ABP Agent's Answer

| The problem being raised | ABP Studio AI Agent's answer |
| --- | --- |
| **Opacity:** decisions buried in code, no rationale | The Analyze engine makes the structure visible; Plan mode turns a decision into a written plan first |
| **Speed-review gap:** agent fast, review slow | Deterministic workflows + ABP-aware review close the loop inside the IDE |
| **Convergence onto narrow stacks / concentrated risk** | ABP already provides a consistent, secure, enterprise stack and conventions |
| **Governance / ADR gap** | Lessons + shared custom workflows put decisions on the record |
| **Implicit coupling:** the prompt dictates the infrastructure | Solution-awareness means infrastructure is determined by the real structure, not by guesswork |

The pattern is consistent: everything the governance framework says "should exist" is part of ABP Agent's design.

---

## An Honest Boundary

Let's be clear: the governance framework above is a general, ABP-independent discussion; it wasn't built to promote ABP. Nor are we claiming that "academia recommends ABP." Our claim is more modest and more solid: ABP Studio AI Agent's design **overlaps remarkably** with these **principles**. Vibe architecting is a real risk, and no tool reduces it to zero; but in enterprise .NET development, ABP Agent is built to reduce that risk meaningfully.

---

## Conclusion

The real question is not whether AI agents make architectural decisions; they do, and that is now an irreversible reality. The real question is this: are those decisions **visible, governed, and reviewable**, or do they get quietly buried in the code and turn into technical debt?

ABP Studio AI Agent is designed to answer "yes, visible and governed." It surfaces decisions instead of hiding them; it knows the structure of the solution instead of guessing; it learns and keeps a record instead of starting from scratch every time. If you build enterprise software in the age of vibe architecting, that is exactly where the difference lies.

- **ABP Studio AI Agent:** https://abp.io/studio/ai-agent
- **Live demo (community talk):** https://www.youtube.com/watch?v=GYVFn2lRuWw
