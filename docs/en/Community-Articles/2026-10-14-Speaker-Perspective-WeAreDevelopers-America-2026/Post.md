# A Speaker’s Perspective: WeAreDevelopers World Congress North America 2026 with Lots Of AI

I’ve been to quite a few developer conferences over the years but this one was a little different: **WeAreDevelopers World Congress had arrived in North America for the first time.** I traveled from Istanbul to San Francisco which took 14 hours of flying. It was exhausting but it worth. Conference was 2 days plus 1 day pre-conf. The dates are September 23–25, 2026. Host venue was San Jose McEnery Convention Center.

The location was San José, in the heart of Silicon Valley and the scale was impressive. As I learnt the first day 9000 developers registered and took their badges. AI builders, architects and technology leaders came together for three days of talks, workshops, networking and probably more technical conversations than anyone could keep track of.

I was there as one of the speakers, so here are some impressions from the conference, this time from both sides of the stage 😊

![image-20261009193336745](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009193336745.png)

## San José, Developers Everywhere

One thing WeAreDevelopers does very well is scale. This isn’t the kind of conference where you walk into a single hall, watch ten talks, drink a few coffees and go home. Almost 10.000 attendees came to the conf. More than 600 speakers... 8 stages were running in parallel alongside workshops, expo areas, networking spaces and even an outdoor stage and lounge.
And, of course, in 2026, you can probably guess the main topic:

> **AI... Lots of AI 🙂**

As I see, the conversations are going beyond “**look what an LLM can do**.” now. We're not surprising anymore about LLMs capabilities. 
The next important questions are:

* **How do we actually put AI into production software?**
* **How much does it cost?**
* **Is it secure?**
* **Who's responsible for the AI made results?**
* **How much effort do humans and AI expend?**
* **When will human take part if AI is doing everything?**
* **While AI agents are writing code, how can we make more deterministic apps?**
* **Are sandboxes getting more more important?**



![image-20261009195239178](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009195239178.png)



>  *We've reached a point where none of us truly understands the internals of our own software anymore!*

Sessions covered AI agents, AI-generated software, security for autonomous systems, testing non-deterministic behavior, developer infrastructure, data and software architecture.

That shift feels important. The industry is no longer only experimenting with AI. Developers are trying to figure out how to make it 

* **reliable**
* **secure**
* **maintainable**
* **genuinely useful.**

## My Talk: Chat with Your Data

My session was on Stage 6 on Friday 09:40. It was the first time the audience listened to me via headphones while I was speaking on stage. It was a somewhat interesting experience. :) And I made this talk before in different countries but the USA audience was very interested in my topic. They asked many questions after the session. The conversations continued even after the talk and I made some new friends there.

----> MY PHOTOS <-----



### Being a speaker creates a funny problem at conferences

 Before your session, a small part of your brain is always thinking about your slides, your demo and all the things that could theoretically go wrong. After the talk, suddenly the conference becomes much more relaxing. 🙂

And there was a lot to choose from.

The speaker lineup included people such as Thomas Dohmke (former GitHub CEO), Mark Cavage (President of Docker), Erran Berger (CTO of LinkedIn), Amjad Masad (Replit CEO), Kelsey Hightower (Google Distinguished Engineer),  Justin Boitano (VP of NVIDIA), Prashanth Chandrasekar (CEO Stack Overflow), Eric Simons (Founder Bolt.New), Neel Sundaresan (General Manager AI Products IBM)

![image-20261009195303929](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009195303929.png)

## The Best Part Is Still the People

Technology conferences are supposedly about technology.

But after attending enough of them, I think the real value is often somewhere **between the sessions**.

You meet somebody after your talk.

A five-minute conversation becomes thirty minutes.

Someone tells you how they solved a problem completely differently.

Someone has been using a project you’ve worked on for years.

Or you finally meet someone you’ve known online for a long time.

With more than 10,000 people in the same place, there were plenty of opportunities for those accidental conversations.

And those are usually the moments I remember much longer than individual slides or keynotes.

![image-20261009195315021](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009195315021.png)

## What I Learnt From the Talks

> A model reviewing its own work tends to overlook the same mistakes. 
> [Jeff An](https://www.linkedin.com/in/jeff-an/) Co-founder of Momentic.ai says benchmarks showed that a leading coding agent detected only 15% of the bugs it had introduced.
> So, we should avoid assigning the same agent to both write and review the code.

[Julia Furst Morgado](https://www.linkedin.com/in/juliafmorgado/): "Your Agents Need Observability Before They Need Better Models" 

**Evolution of Software Development** : Procedural Programming → Object-Oriented Programming → Frameworks & Design Patterns → Microservices → Cloud-Native & Serverless → Agentic Software Engineering

In his session “Who Tests the AI?”, [Himraj Singh](https://www.linkedin.com/in/himraj-singh-5048a266/) emphasized that the same inputs can now produce different outputs, making traditional testing necessary but far from sufficient.

## Manufacturing Trust: Speed and Safety in the Age of Agents 🧑 Mark Cavage, President of Docker 

![image-20261008194727172](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261008194727172.png)

AI agents can dramatically accelerate software development but they need deterministic security boundaries, explicit permissions and reproducible environments to operate safely at scale.

![bf6c9bb8-5869-46ca-9d58-5096e9de3a93](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\bf6c9bb8-5869-46ca-9d58-5096e9de3a93.png)

#### Takeaways

- Sandboxing should be the default. Docker advocates microVM-based isolation so agents can operate freely inside their environments without gaining access to sensitive host systems.
- Permissions must be deterministic. Remove dangerous capabilities rather than repeatedly asking the model to behave responsibly.
- Successful task completion isn't enough. Agents must achieve their goals while staying within their intended authorization boundaries.
- Agent teams need engineering processes. Multiple collaborating agents require clearly defined roles, responsibilities, handoffs, reviews and approval gates.
- Portability and reproducibility are essential. Agent environments should work consistently across developers' laptops, teams and cloud infrastructure.

**The four requirements of an AI agent factory**

![image-20261008193826119](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261008193826119.png)

The following images are retrieved from https://www.docker.com/blog/coding-agent-horror-stories-the-29-million-secret-problem/

![img](https://www.docker.com/app/uploads/2026/07/image2-1.png)

![img](https://www.docker.com/app/uploads/2026/07/image1-1.png)

 

**Docker's Products:** 

- **Docker SBX**: Free local microVM-based sandboxing for autonomous agents
- **Docker Sandbox Kits**: Reproducible specifications for agent environments, models, MCP tools, networks and permissions
- **Docker Cloud Sandboxes**: Cloud-hosted sandboxes using the same configuration and security approach as local environments

**Actionable Insights**

1. **Audit your current AI agent permissions:** Identify which agents can access repositories, production systems, customer data, credentials and external APIs. Eliminate unnecessary access.

2. **Introduce sandboxing for coding agents: ** Test isolated environments for Claude Code, Codex, or other coding agents. Avoid mounting sensitive host resources into agent environments.

3. **Design a structured multi-agent development workflow:** Define separate responsibilities for planning, implementation, testing, security review and final human approval.

4. **Standardize environments across the team: ** Store agent configurations, policies, tools and dependencies in version-controlled files to make workflows reproducible.

5. **Measure trust alongside productivity: ** Track not only completed tasks and development speed but also permission violations, human interventions, security incidents, token costs and escaped defects.

6. **Capture organizational knowledge through agents: ** Experiment with turning successful development workflows into reusable skills, while requiring approval before sharing sensitive organizational knowledge.

![image-20261008195346170](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261008195346170.png)

![image-20261008195527905](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261008195527905.png)

![image-20261008200008969](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261008200008969.png)



**Nvidia Unveils AI Agent Safety Platform With Hardware-Based Watchdog**

| Component | Type                              | Function                                                     |
| --------- | --------------------------------- | ------------------------------------------------------------ |
| OpenShell | Open-source software              | Isolates AI agents and controls their permissions and API access. https://github.com/NVIDIA/OpenShell |
| Sentry    | Hardware-backed security software | Runs on NVIDIA BlueField-4 DPU to provide independent security monitoring and enforcement. |

![image-20261014003620685](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014003620685.png)

---



## AI Decision Observability: Enabling Transparency and Trust in Intelligent Systems 

🧑**Speakers:** Amjad Shaikh (VP Platform ServiceNow)

![image-20261009150813966](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009150813966.png)

###  Key takeaways

- AI decisions require different levels of control. Automatically routing a support ticket is fundamentally different from rejecting a loan application. Autonomy should depend on risk, impact and reversibility.
- Traditional logs are insufficient. Knowing what an AI system did is not the same as understanding why it made a particular decision.
- Organizations must know which information, model versions, configurations and policies influenced an AI decision.
- AI performance must be measured against business outcomes. A technically correct prediction can still produce a poor business result.
- As the world changes, artificial intelligence models degrade. Continuous monitoring for data and concept drift is necessary after deployment.
- Accountability remains with the organization. Delegating a decision to AI does not transfer legal or operational responsibility to the model.

![image-20261009152831092](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009152831092.png)

### AI Decisions Factors

![image-20261009151732552](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009151732552.png)



### Three Layers of AI Decision Observability

![image-20261009152024726](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009152024726.png)

1. **Capture**: Record decisions, inputs, data lineage, model versions, policies and configurations at decision time.

2. **Interpret**: Analyze feature influence, decision robustness, model drift and potential causal relationships.

3. **Govern**: Preserve tamper-evident evidence, enforce accountability and support audits and investigations.

### Practical Implementation Checklist 

✔ Figure out what decisions your AI is making and how risky they actually are. Some mistakes are easy to fix, others can cause serious problems.
✔ Be clear about what AI can handle on its own and what still needs a human to sign off.
✔ Log what goes into each AI decision, what comes out, which model was used and what information it relied on. You'll thank yourself later.
✔ Don't just track AI accuracy. Look at whether it's actually improving things like resolution times, customer satisfaction, or costs.
✔ Keep an eye on how your AI behaves in production. Models can get worse over time, especially when the data changes.
✔ Test how AI decisions would change under different scenarios and check for potential bias when those decisions affect people.
✔ Keep sensitive logs and decision records secure. Make sure you know who can access them and how long they're stored.
✔ Decide which information sources your AI should trust first. You don't want it treating some random outdated document as the ultimate source of truth.

---

## Building the Agentic Software Factory

🧑**Speakers:**  Thomas Dohmke (Founder of [Entire](https://entire.io/)) and Matan Grinberg (Cofounder of [Factory](https://factory.com/)) 

**Agentic Software Factory:** AI-powered software development system where autonomous AI agents collaborate to design, build, test, deploy and maintain software applications with minimal human intervention.

AI coding agents are cool but the real revolution isn't getting Claude or Codex to write more code. It's building an entire software development system that can plan, code, test, review, deploy and improve products with minimal human intervention.



![image-20261009153157480](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009153157480.png)

[Entire](https://entire.io/) is created by Thomas Dohmke the former CEO of GitHub and it focuses on AI Agents + Human collaboration in software development.

![image-20261009152937844](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009152937844.png)



## Key takeaways

- We're moving from AI coding assistants to autonomous software factories. Instead of developers prompting agents for individual tasks, the goal is a continuous loop that turns customer feedback, incidents and product requirements into deployed improvements.

- Developers will increasingly build the factory, not just the software. Engineering shifts toward designing workflows, orchestrating agents, establishing quality controls and deciding what deserves human attention.

- Code review is the biggest bottleneck. Agents can generate hundreds of pull requests but humans can't realistically review them all. More generated code doesn't automatically mean more delivered value.

- Full autonomy is technically possible today but business readiness is another story. New projects can be designed for autonomous development. Legacy enterprise systems are much harder because of complexity, security, compliance and organizational processes.

- Token economics matter. Spending $5,000 on AI tokens to complete a task a developer could finish in five seconds is obviously ridiculous. Successful factories will optimize for business outcomes, not agent activity.

- Human oversight isn't disappearing. The goal is to automate routine checks and low-risk changes while keeping engineers involved in critical architectural, security and product decisions.

- Experience alone is becoming less of a differentiator. Curiosity, problem-solving, initiative and adaptability may matter more than years spent mastering a particular programming language

**A software factory is more than a bunch of agents**

![image-20261009154003347](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009154003347.png)



Don't automate every PR equally. Automate predictable, low-risk changes first and concentrate human expertise where mistakes could be expensive.



🩸**AI-generated PR spam is a real problem**

Agents can create hundreds or thousands of PRs, potentially overwhelming maintainers.

**🩸Developers aren't necessarily going away but their jobs are changing**

The panel's optimistic argument is that software engineers are fundamentally problem solvers, not just people who know programming syntax.AI removes some implementation barriers, letting engineers work on bigger problems. Some developers still genuinely enjoy writing code. The panel acknowledges that the craft doesn't have to disappear just because machines can automate much of it.

**🩸Agent infrastructure needs to evolve**

Thomas explained that traditional Git workflows were designed for human collaboration. Autonomous agents introduce much higher activity levels and new interaction patterns:

- Human-to-agent collaboration
- Agent-to-agent coordination
- Shared memory across agent sessions
- Traceability between agent actions and commits

### Key Takeaways

- Start with one repetitive workflow. Pick something like dependency updates, test generation, bug triage or documentation. Don't try to automate your entire engineering department overnight.

- Build an agent workflow, not just a prompt library. Define the inputs, expected outputs, tools, tests, review criteria and conditions for escalation to a human.

- Introduce **risk-based auto-merging**. Allow well-tested, low-risk changes to merge automatically in a controlled pilot. Require human approval for security-sensitive, architectural and business-critical changes.

- Measure ROI (Return on Investment) instead of token consumption. Track engineering hours saved, cost per accepted PR, deployment frequency, defect rates and review time.

- Connect agents to real customer signals. Support tickets, telemetry, bug reports and feature requests should feed a prioritized engineering pipeline, not just sit in separate dashboards.

- Teach developers agent orchestration. The emerging skill isn't just prompt engineering. It's designing systems where agents can operate for extended periods without constant intervention.

- Make failures improve the factory. When an agent makes a mistake, add a regression test, architectural rule or automated check so the same mistake is less likely to recur.

---

## I Don't Trust AI Agents (And Neither Should You): Building Production-Ready Architectures

🧑**Speakers:**  Darko Mesaros (Distinguished Developer Advocate AWS)

![image-20261009155220391](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009155220391.png)

The problem isn't that AI agents make mistakes. It's that developers give them too much freedom without building proper controls around them.

The industry is spending a ridiculous amount of energy making agents smarter, while a lot of production problems are really about controlling what agents are allowed to do.

The most useful idea in this talk isn't another fancy agent framework. It's treating the LLM as an untrusted component inside an otherwise well-engineered system.

And I especially liked the point about not giving one giant agent 150 tools and hoping a massive system prompt will keep everything under control.

- Don't trust agents to behave correctly just because you've written good system prompts.
- Build multiple security layers around agents: guardrails, steering, tool authorization, deterministic policies and observability.
- Never let an LLM decide who has access to sensitive data. Authorization belongs in your backend.
- High-risk operations should be blocked or require human approval.
- Avoid building one giant agent with 150 tools. Use specialized subagents with granular permissions.
- Trust the architecture surrounding the agent, not the agent itself.

![image-20261009182127593](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009182127593.png)



### 1. Prompt engineering is not security

You can tell an agent "Never refund more than $500" but that doesn't guarantee it will obey.

The speaker identifies three failure modes:

- Says something wrong: Hallucinates information or promises something it cannot deliver.
- Does something wrong: Refunds money, deletes data, or accesses the wrong customer's account.
- Gets manipulated: Prompt injection causes unintended behavior.

The solution is defense in depth: several independent protection layers rather than trusting the LLM.

### 2. Steering agents act as reviewers

A steering agent is a second LLM that evaluates the main agent's response before it reaches the customer.

For example, if the agent claims to have created a shortened return-label URL that it cannot actually generate, the steering agent rejects the response and provides corrective guidance.

Interesting takeaway: You don't necessarily need a smarter main model. Sometimes adding an independent reviewer is more effective.

One caveat: a steering agent is still probabilistic, so it shouldn't replace deterministic security controls.

### 3. Never trust LLM-generated tool parameters

This is probably one of the most important technical points.

Imagine a customer asks:

> Show me order #67890.

The LLM should not be trusted to determine whether that customer owns the order.

Instead:

- Authenticate the customer outside the LLM.
- Inject the authenticated customer ID into MCP calls using an interceptor.
- Enforce authorization deterministically in the backend.
- Return only data that customer is permitted to access.

The speaker demonstrates this with Amazon Bedrock AgentCore Gateway MCP interceptors.

### 4. Put policy enforcement in front of tools

The speaker demonstrates the Dogwood policy language to enforce business rules before MCP tools execute.

Examples:

| Rule                     | Enforcement                           |
| ------------------------ | ------------------------------------- |
| Refund/return above $500 | Requires human review                 |
| Return label generation  | Eligibility check must run first      |
| Return eligibility       | Must satisfy the 30-day return window |
| Customer order lookup    | Must respect customer identity        |

The particularly interesting part is temporal policies.

For example, the `generate_return_label` tool cannot execute unless `check_return_eligibility` has successfully run within the previous hour.

That's much stronger than asking an LLM to remember the correct workflow.

### 5. Observability is essential

Production agents need more than basic application logs.

The speaker recommends tracking:

- Agent decisions and tool calls
- Tool selection accuracy
- Parameter selection accuracy
- Task success rates
- Harmful or incorrect outputs

He also recommends periodically evaluating historical conversations, especially after changing prompts, models, or tools.

The key point: An agent's behavior can change even when your business logic hasn't.

### 6. One giant agent is usually a bad architecture

In the Q&A, Darko highlights a common mistake: building one agent with a huge prompt and access to 150 tools.

Instead, use specialized agents with limited responsibilities.

For example:

- Returns Agent
- Customer Account Agent
- Order Status Agent

Each agent should receive only the tools and permissions it needs.

This makes agents easier to secure, evaluate and debug.

![image-20261009182139355](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009182139355.png)

**Security Layers Architecture** 



![image-20261009182212869](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009182212869.png)

![image-20261009182259531](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009182259531.png)



![image-20261009182348881](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009182348881.png)

## Most interesting

- The MCP Gateway becomes a security boundary. It's not just a convenient integration layer. It can enforce identity, tool access and policies independently of the model.
- Temporal policies are underrated. Enforcing what must happen before an action is a powerful way to control agent workflows.
- The speaker is essentially advocating traditional software engineering. Authentication, authorization, least privilege, logging and separation of responsibilities still matter. LLMs don't eliminate those requirements.
- Subagents are not just for better reasoning. They're also a way to isolate permissions and limit damage.



---



## Securing AI Agent Infrastructure: Identity, Attestation and Trust at Scale

🧑**Speaker:**  Abdel Fane (Founder OpenA2A)

![image-20261014004734473](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014004734473.png)

AI agents should be secured through infrastructure, not system prompts, because LLMs are unpredictable and vulnerable to prompt injection. The speaker recommends three essential security layers: 

1. Identity (every agent needs a verifiable identity tied to its owner)

2. Capabilities (strict permissions defining exactly what tools, data and actions an agent can access)

3. Audit Trails (tracking every action, including blocked attempts). 

   

He demonstrates how a malicious website can trick an AI agent into leaking sensitive information, while deterministic tool-level permissions can prevent it. 

> Treat agents like users with their own identities, enforce least-privilege access outside the LLM and log every action. Never rely on prompts alone to protect production systems.

---

## Zero-Trust Architecture for Agentic AI: Securing Multi-User Access and Third-Party

🧑**Speakers:**  Borko Djurkovic (Member of Technical Staff Cohere)

![image-20261014005347668](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014005347668.png)

AI agents must operate under Zero-Trust security principles, especially in multi-user enterprise environments. The speaker highlights three critical areas:

1. **User authorization:** Combine RBAC (Role-Based Access Control) with ReBAC (Relationship-Based Access Control, using tools like OpenFGA) to control exactly which agents, models and tools each user can access. 
2. **Secure third-party integrations:** Use OAuth, delegated credentials and controlled MCP tool access so agents never exceed the permissions of the users they represent. 
3. **Secure RAG:** Remove sensitive information before indexing documents and enforce document- or chunk-level permissions during retrieval. The main takeaway for ABP Studio AI Agent and Volobox: An agent must inherit the user's permissions, not bypass them. Every tool call and retrieved document should be independently authorized, regardless of what the LLM decides to do.



---

## Give the Agent a Budget, Not a Token

🧑**Speakers:** Sachin Malhotra (Platform Engineer Anthropic)

![image-20261014010315592](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014010315592.png)

AI agents shouldn't receive unlimited permissions, even when performing legitimate tasks. Instead, give them controlled budgets for what they can do, how often and with what level of risk. Sachin Malhotra from Anthropic shares a real incident where Claude accidentally deleted 200 workloads belonging to 20 engineers in just 90 seconds. He recommends rate limits (restrict destructive actions per hour), asymmetric permissions (allow low-risk actions but require approval for dangerous ones), tripwires (monitor unusual behavior) and the Undo Test (if an action cannot be safely reversed, require human approval). Crucially, permissions must be enforced by infrastructure using independently verified agent identities, not prompts or agent-controlled credentials. The takeaway for ABP Studio AI Agent and Volobox: Give every agent its own identity, enforce action limits, monitor behavior and require human approval for high-risk operations. The goal isn't to eliminate every mistake but to limit the damage an agent can cause while preserving its autonomy.



---

## From Model Selection to Smart Routing

🧑**Speakers:** Viktoria Semaan (Principal Technical Evangelist at Databricks)

![image-20261014010740705](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014010740705.png)

Using the most powerful LLM for every task is expensive and often unnecessary. Instead, AI systems should automatically select the most cost-effective model based on task complexity, quality, speed and cost. Victoria Ziman from Databricks recommends three approaches: 

1. **Model Evaluation:** Benchmark different models against real tasks, measuring accuracy, latency and cost per task. 

2. **AI Gateway:** Centralize model access, track spending, enforce budgets and support model switching and fallbacks. 

3. **Smart Routing:** Automatically assign simpler tasks to cheaper models and complex reasoning tasks to more powerful models. 

   

She also demonstrates [Omnigen](https://omnigent.ai/), a meta-harness that coordinates coding agents, selects models and controls execution budgets and retry limits.

![image-20261014011228086](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014011228086.png)

> Implement intelligent model routing, per-agent budgets and continuous evaluation to reduce AI costs without significantly compromising quality. The goal isn't to use the best model everywhere, but the best model for each task.



---

## Building AI Products vs. Building With AI

🧑**Speakers:** Aparna Dhinakaran (Founder Arize), Rukmini Reddy (VP of Engineering PagerDuty), Tamar Bercovici (VP of Engineering Box)

![image-20261014011432422](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014011432422.png)

AI is transforming software engineering from writing code to orchestrating AI agents, evaluating their outputs and ensuring production reliability. As code generation becomes cheaper and faster, engineering teams must rethink their workflows, responsibilities and infrastructure. 
The panel highlights four major shifts:

1. **AI Software Factory:** Engineering teams are moving beyond individual coding assistants toward shared platforms where agents generate code, investigate bugs, analyze logs, create PRs and automate development workflows.
2. **Agent Harness & Evaluation:** Building reliable AI products requires adaptable agent harnesses, continuous evaluations, shared context, reusable skills and intelligent model selection. Teams must measure output quality rather than relying solely on traditional unit tests.
3. **AI-Powered Observability:** As AI-generated code increases, human code reviews alone cannot scale. AI agents can review other agents' activities, analyze traces, detect anomalies and identify production issues. Guardrails and controls to limit the impact of failures remain essential.
4. **Build vs. Buy:** AI makes custom software easier to build, but maintaining production-grade reliability, security, compliance and observability remains challenging. Companies should focus engineering resources on differentiating capabilities and consider buying specialized infrastructure.

The speakers also emphasize that engineers should treat agents like new team members: when an agent produces poor results, improve its context, tools, instructions and skills rather than simply abandoning the task.

> Move beyond AI-powered code generation toward an AI Software Factory with shared agent skills, reusable context, continuous evaluations, automated code reviews and production observability. The real competitive advantage is not generating more code but building a reliable system where humans and AI agents can continuously develop, test, deploy and maintain software together.



---

## Making (and Breaking) Agents by Adding 1,000 MCP Tools

🧑 Speaker: Guillaume Lebedev (Co-founder & CTO, StackOne)

![image-20261014013431823](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014013431823.png)

Adding more MCP tools doesn't necessarily make AI agents more capable. It can actually break them by overwhelming their context windows, reducing tool-selection accuracy and increasing security risks. Guillaume Lebedev shows what happens when an agent is connected to 1,000 MCP tools and recommends four approaches:

1. **Tool Search & Lazy Loading:** Instead of loading every MCP tool definition into the LLM's context, dynamically discover and load only relevant tools. His demo shows that loading hundreds of tools can consume approximately 440K tokens before the agent even starts working.
2. **Semantic Tool Discovery:** Traditional keyword-based search (BM25) can miss relevant tools or their prerequisites. Vector embeddings, semantic search and dependency graphs improve tool discovery accuracy and help agents find the correct sequence of operations.
3. **Context Optimization:** Use techniques such as Code Mode and Recursive Language Models (RLM) to process large tool responses without filling the agent's context window. This helps agents work efficiently with large amounts of external data.
4. **Prompt Injection Defense:** Every MCP tool response should be treated as potentially untrusted. Malicious instructions hidden in emails, CRM notes, meeting transcripts, or support tickets can hijack agents. Combining specialized classifiers with LLM-based detection improves protection, but no detection method is perfect.

He also demonstrates that tool discovery and prompt injection detection are classification problems, where specialized models can outperform simple keyword matching. However, the strongest protection remains deterministic access policies that prevent agents from calling unauthorized tools or accessing sensitive data.

> Build a scalable MCP tool architecture with lazy tool loading, semantic tool discovery and dependency-aware tool selection. Optimize large tool responses to reduce context consumption and token costs. Most importantly, treat all external tool results as untrusted and enforce tool-level permissions outside the LLM. The goal isn't to give agents access to more tools, but to help them discover and safely use the right tools at the right time.

---



## Context Engineering: How Machines Remember and Forget

🧑 Speaker: Emre (Solutions Architect at OpenAI)

![image-20261014013748342](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014013748342.png)

Context engineering is about providing AI agents with the right information at the right time, rather than filling their context windows with everything available. The speaker explains that long-running agents accumulate unnecessary tokens, outdated information and conflicting context, which can reduce accuracy and increase costs. He introduces three core strategies:

1. **Reshape & Fit:** Optimize short-term context using trimming, compaction and summarization. Preserve important decisions and task progress while removing irrelevant conversation history and excessive tool outputs.
2. **Isolate & Route:** Use separate contexts and subagents to prevent information overload. Delegate specific tasks to agents with focused context rather than forcing one agent to process everything.
3. **Extract & Retrieve:** Build long-term memory that works across sessions. Extract meaningful information, store it persistently and retrieve only relevant memories when needed.

He also emphasizes that memory should evolve rather than grow indefinitely. Agents must consolidate conflicting memories, forget outdated preferences and decide whether information should be loaded automatically or retrieved on demand. Evaluations (evals) are essential for measuring whether memory and context optimization actually improve performance, including testing agents with memory enabled versus disabled.

> Build a dedicated Context & Memory Management layer that automatically summarizes long-running sessions, isolates subagent contexts and maintains persistent memory across sessions. Implement intelligent memory extraction, retrieval, consolidation and forgetting. Continuously evaluate context quality, token consumption and task success. The goal isn't to make agents remember everything, but to make them remember what matters and forget what doesn't.

---

## Responsible AI Architecture with Zero Trust Agents

**🧑 Speaker:** Ashok Prakash (Staff ML Engineer Apple)

![image-20261014014342669](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014014342669.png)

Building an AI agent is easy, but making it safe, controllable, observable and production-ready requires a complete infrastructure architecture. The speaker identifies four major production risks: black-box decisions, silent model drift, unsafe actions and uncontrolled costs. He proposes an open-source, Zero-Trust reference architecture with five essential principles:

1. Agent Controller & Execution Budgets: Run agents through controlled workflows (e.g., Temporal or Celery) with checkpoints, execution limits, token budgets and the ability to stop runaway agents. Prevent infinite reasoning loops and uncontrolled subagent execution.
2. Zero-Trust Tool Access: Treat tools and MCP integrations as contracted services with scoped identities, least-privilege permissions and just-in-time access. Agents must never access another user's data or retain permissions beyond their authorized scope.
3. Policy-Based Governance & Human Approval: Use deterministic policy engines such as Open Policy Agent (OPA) instead of relying on natural-language prompts. Classify actions into autonomous, human-assisted and human-only decisions, with structured approval requests for sensitive operations.
4. Responsible Data & Memory Management: Enforce consent tracking, PII redaction, data retention policies and user-scoped access. Isolate conversational memory from knowledge bases and maintain source traceability to detect and recover from poisoned data.
5. Observability, Evaluation & Safe Deployment: Use OpenTelemetry to track agent decisions, costs and failures. Continuously monitor model drift, version models and RAG datasets and deploy through evaluation gates, shadow testing, canary releases and gradual rollouts.

The speaker also highlights KServe and vLLM for model serving, caching, autoscaling and cost optimization. His central architectural principle is that safety must be enforced throughout the infrastructure, not added as a final layer.

> Build a Zero-Trust Agent Runtime with centralized execution control, scoped tool permissions, deterministic policies, human approval workflows and end-to-end observability. Add execution budgets, versioned agent configurations, continuous evaluations and safe deployment mechanisms. The goal is not simply to create autonomous agents, but to make their behavior predictable, auditable, recoverable and safe enough for enterprise production.

---

## Rethinking Developer Tools for the Agent Era

🧑 Speakers: Ben Helle (Chief Product Officer, Webflow), Han Wang (Co-founder, Mintlify), Sam Bhagwat (Founder & CEO, Mastra)

![image-20261014014755824](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014014755824.png)

AI agents are becoming primary users of developer tools, forcing companies to rethink product design, documentation, APIs and developer experience. Webflow reports that the majority of its MCP-driven content development activity now involves agents, while Mintlify reports that agent traffic increased from approximately 2–3% in early 2025 to 67%. The speakers highlight four major shifts:

1. **Developer Experience (DX) → Agent Experience (AX):** Developer tools must increasingly serve AI agents alongside humans. Agent-friendly APIs, MCP servers, CLI interfaces, skills files and documentation are becoming important product capabilities.
2. **Agent Discovery & Adoption:** Agents increasingly discover, evaluate and recommend developer tools. Clear documentation, accurate technical information and well-designed SDKs influence whether agents successfully adopt a product. However, great human developer experience often translates into great agent experience.
3. **Task-Oriented MCP & Shared Skills:** Simply exposing REST APIs through MCP isn't enough. Tools should support meaningful workflows and human goals. Teams also need centralized, reusable agent skills to produce consistent results across multiple agents.
4. **Humans Become Agent Managers:** Developers are shifting from writing every line of code to coordinating teams of agents. The most valuable human skills become product judgment, customer empathy, problem identification and the ability to design effective agent workflows.

The panel also discusses how AI dramatically reduces the cost of building software but doesn't automatically make products successful. Finding product-market fit, understanding customers and building something people genuinely love remain difficult.

> Treat AI agents as first-class users of your products. For ABP, invest in agent-friendly documentation, MCP integrations, CLI capabilities, reusable skills and task-oriented workflows. Focus on human-agent collaboration, shared skills and managing teams of agents. The opportunity is no longer just helping developers write code faster, but enabling humans to manage agents that build software for them.



---

## Your Next Customer Won’t Be Human

🧑 Speaker: Ben Smith (Staff Developer Advocate at Stripe)

 ![image-20261014015141785](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014015141785.png)

AI agents are becoming economic actors: they will not only discover and purchase products on behalf of humans, but also autonomously pay for APIs, tools, compute and services to complete tasks. Traditional websites and checkout flows are designed for humans, while agents need structured data, discoverable APIs, standardized protocols and secure payment mechanisms. Ben Smith from Stripe highlights four major developments:

1. **Agentic Commerce & Agent Experience:** Businesses must make their products and services discoverable and purchasable by AI agents. Structured product catalogs, clear constraints, APIs and machine-readable information become essential.
2. **Universal Commerce Protocol (UCP):** A standardized protocol that allows agents to discover merchants, browse products, create checkouts and complete purchases without building custom integrations for every merchant. Merchants expose their capabilities through `/.well-known/ucp`.
3. **Secure Agent Payments:** Stripe's Shared Payment Tokens (SPT) allow agents to initiate purchases without exposing raw payment credentials to merchants. Tokens can be restricted by merchant, spending amount, expiration and usage.
4. **Machine Payments Protocol (MPP):** Enables machine-to-machine payments using HTTP `402 Payment Required`. Agents can automatically pay for APIs, MCP tools, storage, compute and other services on demand, without requiring users to manually create accounts or manage API keys.

The speaker demonstrates an AI shopping assistant that discovers products, recommends options, calculates shipping and taxes and completes a purchase. He also shows how changing an agent's instructions can turn a helpful shopping assistant into a misleading salesperson, emphasizing the importance of responsible agent behavior.

> Prepare for an agent-to-agent economy where agents can discover, purchase and consume external capabilities autonomously. Consider per-agent wallets, spending limits, payment approvals and usage tracking to support agents purchasing APIs, MCP tools and services. For ABP, explore agent-friendly service discovery and standardized commerce integrations. The next generation of software won't just serve human users; it will also serve AI agents that can independently discover, access and pay for capabilities.

---

## The Browser Is Becoming an AI Runtime

🧑 Speakers: Ajit Varma (Head of Firefox, Mozilla), Will Burke (Founder & CEO, Exa), Julia Love (Technology Reporter, Bloomberg , Moderator)

![image-20261014015602329](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014015602329.png)

Browsers are evolving from tools humans use to navigate websites into AI runtimes where agents search, understand and interact with the internet on our behalf. Mozilla and Exa envision an open, agent-friendly web where users can choose their AI models, preserve their browsing context and avoid dependence on a single AI provider. The speakers highlight four major shifts:

1\. **Browser as an AI Runtime:** Browsers are becoming execution environments for agents. Instead of simply displaying websites, they enable AI to navigate pages, interpret content, make decisions and perform actions. Some agent systems already use cloud-hosted browsers running inside virtual machines.

2\. **AI Choice & Model Independence:** Mozilla's Firefox Smart Windows approach allows users to choose between AI providers, including open and privacy-focused models, while maintaining their browsing context. This reduces vendor lock-in and supports AI sovereignty.

3\. **Search Built for Agents:** Exa is developing search infrastructure specifically for AI agents rather than humans. Will Burke predicts that AI-generated searches could surpass human searches by early 2027, creating a fundamentally different market for information retrieval.

4\. **A New Economy for Web Content:** Traditional search relies heavily on advertising and human clicks. AI agents increasingly consume information without visiting websites, threatening publishers' existing business models. Exa proposes a model where agents pay data providers directly for high-quality information, including through its Exa Connect offering.

The panel also emphasizes open web standards, publisher consent, source attribution and user control. Websites are still primarily designed for humans, but agents increasingly interact with them through browser automation, visual recognition and emerging protocols such as MCP.

> Treat browser automation as a core agent capability, alongside APIs and MCP tools. Consider isolated browser sessions where agents can navigate websites, perform tasks and request human approval for sensitive actions. For ABP, make applications accessible to both humans and agents through structured interfaces, APIs and agent-friendly workflows. The future browser won't just display applications; it will become an execution environment where AI agents operate applications on behalf of users.

---



## Going Beyond the Code Through the GitHub Copilot App: How to Unlock You and Your Team’s Superpower

🧑 Speaker: Jenny Huang (Director of User Research, GitHub)

![image-20261014020250963](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014020250963.png)

AI coding agents have removed the bottleneck of writing code, but the bottleneck has shifted to coordination, context switching and deciding what to build. Jenny Huang explains that software productivity is no longer just about generating code faster. It's about connecting information, reducing fragmented workflows and helping people move from ideas to valuable outcomes. She demonstrates three practical applications of the GitHub Copilot app:

1. Product Feedback & Prioritization: Aggregate thousands of feedback signals from GitHub Issues, customer surveys, telemetry and other sources. Use AI to identify patterns, understand customer needs and prioritize product development based on evidence rather than scattered discussions.
2. Customer Context & Sales Readiness: Connect information from multiple business systems to create a unified customer overview. Sales teams can prepare for meetings, understand customer relationships and identify opportunities without manually searching across different applications.
3. Research & Content Creation: Transform raw data, transcripts and external research into actionable insights and working applications. The VS Code team demonstrates using Copilot to research podcast topics and build an analytics dashboard from podcast engagement data.

She emphasizes that AI-powered workflows should extend beyond developers to product managers, researchers, sales teams and other business users. The objective is to preserve human creativity and judgment while automating repetitive information gathering and coordination.

> Think beyond AI-assisted coding and build a unified human-agent workspace that connects development, product management, customer feedback and business operations. Prioritize cross-application context sharing, collaborative workflows and agents that turn scattered information into actionable tasks. For ABP Studio AI Agent, consider integrating product requirements, documentation and user feedback directly into development workflows. The next productivity breakthrough won't come from generating more code, but from reducing the coordination overhead between humans, agents and tools.

---

## Why Buy Software When AI Can Build It?

🧑**Speakers:** Lee Edwards (General Partner at Root Ventures), Mang-Git Ng (CEO of Anvil), Richard Robinson (Chief Evangelist at UnqorkAI)

![image-20261009182548395](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009182548395.png)

AI is making software cheaper to build but not necessarily cheaper to operate, secure, scale and maintain. That's where software companies can still win.

- Building an MVP is becoming trivial. Getting software production-ready is still difficult.
- Traditional SaaS is under pressure. Customers are starting to build tools themselves rather than paying subscriptions.
- Developer experience is changing. A great UI may no longer be enough. AI-friendly APIs, CLIs, MCP integrations and documentation are becoming competitive advantages.
- AI agents are becoming software buyers and decision-makers. They increasingly recommend which tools, databases and platforms developers should use.
- Software companies need stronger differentiation. Security, compliance, scalability, reliability and domain expertise matter more than basic features.
- The future may involve fewer dashboards. Users could interact with personal agents rather than traditional SaaS interfaces.

## Main arguments

### 1. The economics of Buy vs. Build have fundamentally changed

Historically, buying software was often cheaper than building and maintaining it. AI is shifting that calculation.

This is particularly concerning for developer tools that primarily monetize convenience, hosting, or ease of setup.

However, the panel agrees that building something and operating it reliably are very different problems.

### 2. MVPs are no longer a competitive advantage

Getting a prototype working is easy. Getting it into production and supporting thousands of customers is still hard.

AI is good at fixing problems once they are identified but panelists argue it remains less reliable at anticipating scaling problems before they occur.

The competitive advantage is shifting:

OLD DIFFERENTIATION

Build software faster

- Rapid development
- Easy UI
- More features
- Quick prototypes

NEW DIFFERENTIATION

Ship reliable software faster

- Production readiness
- Security and compliance
- Scalability
- Ongoing maintenance

### 3. Your next customer might be an AI agent

Lee Edwards describes how Claude recommended Railway and Neon for his projects. The developer didn't independently discover these products. The AI selected them based on the project's requirements.

Anvil's CEO also explains that their strong documentation helps AI agents recommend Anvil over competing PDF and e-signature solutions, generating inbound interest.

As a result:

- Documentation becomes a marketing channel.
- MCP servers and CLIs become product discovery and adoption channels.
- Being recommended by AI assistants may become as important as ranking on Google.

This doesn't mean traditional SEO disappears but software companies increasingly need to optimize for both human and AI-driven discovery.

### 4. SaaS might disappear from the UI, not the infrastructure

Lee Edwards suggests a future where people stop opening multiple SaaS dashboards.

Instead of logging into Salesforce to review sales activity, someone might ask their personal AI agent for an update.

This creates two distinct risks for SaaS companies:

- Customers build their own replacement software.
- Customers stop using traditional applications directly because agents handle the interactions.

He also suggests that systems of record and foundational infrastructure may retain stronger competitive positions.

### 5. Companies should buy what doesn't differentiate them

Anvil's CEO makes a practical argument: Why spend months building PDF generation when that functionality has already been solved, tested and scaled by a specialist provider?

The same logic applies to security, infrastructure and other standardized capabilities.

Build the functionality that makes your business unique. Buy or reuse the capabilities that don't.

AI reduces implementation cost but it doesn't eliminate the value of mature, specialized software.

## 2. What Can We Do for ABP?



| Priority | Opportunity                       | Recommended action                                           |
| -------- | --------------------------------- | ------------------------------------------------------------ |
| High     | AI-driven discovery               | Make ABP documentation easier for AI agents to discover, interpret and recommend |
| High     | Production readiness              | Emphasize secure, maintainable, production-ready applications rather than just faster development |
| High     | Agent-native developer experience | Make ABP Studio capabilities accessible through agent-friendly tools and interfaces |
| Medium   | Competitive positioning           | Explain why AI-generated code still benefits from ABP architecture and modules |
| Medium   | Marketing                         | Publish AI coding workflows demonstrating the value of ABP alongside coding agents |



The below message shifts the value proposition from code generation to the engineering foundations required to deliver reliable applications.

## AI can write your code.  ABP helps you ship production-ready applications.

>  **The whole "AI will kill SaaS" argument is way too simplistic.**

Sure, Claude can build a CRM prototype in an afternoon. But who's going to maintain it, secure it, handle migrations, deal with compliance and keep it running when the business grows?

That's the actual business opportunity.

>  **AI agents becoming the ones choosing software.**

Imagine spending years optimizing your website for developers, only to discover that the developer now asks Claude which framework to use.

Suddenly, documentation, APIs, CLI and MCP integrations become part of your marketing strategy.

> **Don't compete with AI on how quickly it can generate code.** Compete on everything required to turn that code into dependable software.

*Can we make ABP the preferred foundation for AI-generated enterprise applications, rather than positioning it as an alternative to AI coding?* 👊 **YES**



## NETWORKING

![image-20261009184812166](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009184812166.png)



## ROUNDTABLE: Running AI Agents at Scale

What's actually breaking in production agent deployments, from visibility and governance to cost control at scale and what a real governance layer for agentic AI needs to look like. We gathered to talk the topic: "Running AI Agents at Scale".  We were 15 senior engineering and technical leaders facing the same problems. It was hosted by James Everingham, CEO and Co-founder of Guild.ai. I couldn't remember everyone's name in the meeting but as I remember there were Ankit Patel from NVIDIA, Yifei Liu from Vectra AI, Phani Pendurthi from Mastercard, from Oracle and Devin AI.

![image-20261009185237161](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009185237161.png)

### Roughly We Talked About

* There should be lots of alerts in an agentic system. For budget, security... For example if an agent runs too long or it retrieves too much data, be alerted.
* There should be an **AI Kill Switch** toggle somewhere. We don't know when we'll hack, abuse or unwanted agent runs...
* Permissions are crucial! Agents must be allowed with scoped access to the tools, data, APIs.
* Agents should have role-based authentication.
* AI firewalls are another security layer which protect how AI agents interact with users, tools, data and other systems. For example [Cloudflare AI Security](https://www.cloudflare.com/solutions/ai-security/) What an AI firewall does:
  * Prompt Injection: Block malicious instructions hidden in documents, emails, or web pages.
  * Data Leakage: Prevent agents from exposing API keys, credentials, or sensitive customer information.
  * Tool Call Validation: Block unauthorized or suspicious MCP tool calls.
  * Content Filtering: Detect harmful or policy-violating inputs and outputs.
  * Anomaly Detection: Flag unusual agent behavior, such as excessive API calls or bulk data exports.
* For MCP tools, we should apply least-privilege access, allowing agents to discover and execute only the tools they need for their specific tasks. 
* Who's accountable for the things that AI does? Simply humans. An agent must be under control of someone (who created or who initiates) and that person must be accountable for any issues.
* Auditing is more important now! If don't trace the agent's work, we cannot find the accountable person, measure the outputs, costs, running durations, data breaches etc...
* Using sandbox is a best practise.
* AI agents are not for free! To manage your budget, you need to set upper limits per your agent, project, team, task.
* Human in the loop but when?! If an AI agent asks for human approval at every step, automation loses its value. Everything slows down and humans become the bottleneck.
  * **Impact**: How significant are the consequences of the action?
  * **Reversibility**: Can the action be easily undone?
  * **Confidence**: How confident is the AI in its decision?
  * **Risk**: What are the potential financial, security, legal, health or reputational risks?
  * **Authority**: Is the action within the AI agent's permitted scope?

> The higher the impact, the lower the reversibility and the greater the uncertainty, the more important human oversight becomes.

For example, an AI agent can draft an email without approval but sending an email to 10,000 customers might require human confirmation.

### Another topic we discussed: Agent Access Control

AI agents shouldn't have permanent access to everything. Instead, they should receive only the permissions they need, exactly when they need them and only for as long as necessary. This is called Just-in-Time (JIT) Access. For example, an AI agent processing a refund might temporarily get permission to issue that refund, but lose it immediately afterward. This minimizes security risks, limits the damage from compromised agents and allows organizations to scale AI safely without giving agents excessive privileges.

**Just-in-Time Access:** AI agents shouldn't have permanent access to sensitive systems. Instead, they request permissions when needed and a centralized access management system grants temporary access based on predefined security policies. Humans define the rules and the system enforces them automatically. This reduces security risks, limits potential damage and allows AI agents to operate at scale without requiring human approval for every action.

For example, a system administrator defines the following rules:

- An AI agent can issue refunds up to $100.
- Refunds are allowed only for customers assigned to the agent.
- Access is granted temporarily for a single transaction.
- Refunds exceeding $100 require human approval.

When the agent requests access, the system evaluates these predefined policies and grants temporary permissions only if the conditions are met.

---

## Whole Summary

- Move beyond AI coding assistants toward AI Software Factories, where agents collaborate to build, test, review and deploy software.
- Run AI agents in isolated sandboxes with dedicated identities, limited permissions and controlled budgets.
- Apply Zero-Trust security with deterministic policies, audit trails and human approval for high-risk actions.
- Choose AI models based on cost per successful task, using smart routing and continuous evaluations.
- Invest in context engineering and long-term memory to improve agent performance and reduce costs.
- Make observability and automated evaluations essential parts of production AI systems.
- Use specialized agents with focused responsibilities instead of giving one agent access to hundreds of tools.
- Design documentation, APIs, CLIs and MCP tools for AI agents, not just human developers.
- Prepare for an agent-driven economy where AI agents discover, recommend, use and purchase software.
- Focus on production readiness, security, scalability and maintainability as AI makes code generation increasingly accessible.
- Shift developers' focus from writing code to orchestrating agents, solving problems and making architectural decisions.
- Keep investing in junior developers and human expertise, because judgment, creativity and understanding what to build remain essential.



---

## Volobox

I also have chance to explain our new product [Volobox.io](https://volobox.io) to some attendees. Everyone likes the idea and implementation details. 

>  Context is the new king! 

AI models are getting smarter. But the real game-changer is context. That's why Volobox  brings chats, meetings, tasks, files and AI agents into one shared context. No need to copy-paste from different apps, environments. Just AI agents that understand your work; Get things done...

![image-20261009192927320](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261009192927320.png)



## About My Trip in San Francisco & Los Angeles

Some pictures from my trip.

![usa-trip](usa-trip-moments.png)

![image-20261014022128876](C:\Users\alper\Desktop\WeAreDevelopers-USA-2026\blog-post\image-20261014022128876.png)