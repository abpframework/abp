### 1. Alternative Article Title Suggestions

1. **Unifying Dev, Architecture, and AI: My Experience Speaking on Conversational SQL at CONVEX 2026**
2. **CONVEX Summit 2026: Notes from the Stage, the Cinema Screens, and the Future of Database-to-Agent Systems**
3. **Beyond the Vibe Coding: Speaking at CONVEX 2026 and Re-Architecting Conversational B2B Systems**

### 2. Selected Title

# Unifying Dev, Architecture, and AI: My Experience Speaking on Conversational SQL at CONVEX 2026

### 3. Meta Description

Join Volosoft Co-Founder Alper Ebiçoğlu as he shares his firsthand experience speaking at CONVEX Summit 2026 in Madrid, exploring natural-language-to-SQL architecture, Model Context Protocol (MCP), and lessons in AI security.

### 4. SEO Slug Suggestion

```
convex-summit-2026-speaks-view-conversational-sql
```

### 5. Full Article

Arriving in Madrid this June for the inaugural CONVEX Summit 2026 felt like witnessing a major shift in how our industry talks about building software. For years, developers, database administrators, and software architects have operated in distinct technical silos. We have watched artificial intelligence disrupt daily operations, development teams rapidly adopt new frameworks, and distributed architectures grow increasingly complex. Yet, these massive shifts have largely occurred in parallel.

To celebrate their twentieth anniversary, the team at Plain Concepts took a bold, necessary step: they unified three of Spain’s flagship tech events—dotNET, the Global Software Architecture Summit (GSAS), and Singularity Tech Day—into a single, cohesive experience.

As I walked into the Kinépolis Ciudad de la Imagen—the largest cinema complex in Madrid—the scale of this integration was immediately apparent. The venue was buzzing with over 1,200 tech professionals representing more than 25 countries. For me, as a co-founder and software architect at Volosoft, this was more than just another conference. It was a unique convergence point where theory met execution, and where I had the privilege of taking the main stage to speak on a topic I’ve been living and breathing: re-architecting how enterprise applications talk to databases.

!(convex_keynote_stage.jpg)

## Speaking at CONVEX: Chat with Your Data

My session, titled **"Chat with Your Data: Turn any database into a conversational reporting engine,"** was scheduled in front of an incredibly engaged audience of developers, CTOs, and systems architects. The primary problem I wanted to tackle is one that almost every B2B application team faces: the endless cycle of custom report building. Traditional enterprise applications are bottlenecked by the constant demand for custom queries, Excel exports, and visual dashboards.

My talk introduced a conversational reporting approach designed to let non-technical stakeholders safely generate complex reports from their database simply by chatting—as if they were messaging a human analyst.

!(alper_ebicoglu_presentation.jpg)

On stage, I detailed the exact architectural pipeline we built using.NET to securely connect natural language prompts to database engines. Letting an LLM generate SQL queries is easy in a demo, but incredibly dangerous in an enterprise environment. If you simply pass user input directly to an LLM and run the resulting string against your database, you are inviting disastrous SQL injections, massive context bloat, and uncontrolled resource exhaustion.

To solve this, we designed a multi-stage validation pipeline that prioritizes security and performance:

```
[User Natural Language Input]
              │
              ▼
   ───► Minimize schema metadata injected into prompt
              │
              ▼
    ──► Parse user intent to avoid context bloat
              │
              ▼
     ──────► Draft query based on precise prompt constraints
              │
              ▼
    ─► AST parsing to block DDL/DML, enforce read-only
              │
              ▼
   ────► Safe execution isolated from production database
              │
              ▼
  [Output Generation Engine] ───► Dynamic formatting into Excel sheets & charts
```

### The Security Mathematics of SQL Validation

The most critical phase of this pipeline is our dynamic SQL parser. Before any generated query touches the database, the.NET application parses the string into an Abstract Syntax Tree (AST). This allows us to run a deterministic evaluation of the query structure.

We can model this strict security boundary mathematically. Let $Q$ be the generated SQL query, and let $T(Q)$ be the set of operation tokens identified in the AST. We define the security function $S(Q)$ as:

$$S(Q) = \begin{cases}  1 & \text{if } T(Q) \subseteq \{\text{SELECT}\} \land T(Q) \cap \{\text{INSERT}, \text{UPDATE}, \text{DELETE}, \text{DROP}, \text{ALTER}, \text{CREATE}\} = \emptyset \\  0 & \text{otherwise}  \end{cases}$$

If $S(Q) = 0$, the query is immediately rejected at the application boundary, completely mitigating malicious prompt injections or model hallucinations before they can cause damage. We also run these validated queries exclusively against an isolated read-replica, completely separating conversational reporting workloads from our primary transaction database.

During the session, I demonstrated how the pipeline extracts metadata to construct the schema map, routes user intents, and dynamically compiles the resulting database rows into structured Excel files and interactive charts. It was highly rewarding to hear from the community afterward about how this approach solves real-world security concerns while dramatically improving the B2B developer experience.

## What I Learned From the English Sessions

When I wasn't on stage or talking with attendees, I spent my time attending the English-language sessions. Because the conference unified dotNET, GSAS, and Singularity, the technical depth across the tracks was remarkable. Several sessions provided profound, second-order insights into how enterprise engineering teams are actually putting AI and advanced architecture patterns to work at scale.

### AI, Security, and System Exploits

Chema Alonso's Keynote on Day 2, *"Hacking ( with | the ) AI,"* was an eye-opening deep dive into the darker side of generative systems. Alonso, a leading security figure, showcased how malicious actors are actively utilizing AI to accelerate the development of system exploits.

What struck me most was his analysis of semantic vulnerabilities. Traditional firewalls and security protocols are completely blind to threat vectors like jailbreaking, prompt injection, and model exfiltration. Alonso's core thesis resonated deeply with my own presentation: AI is a powerful assistant, but it cannot be treated as a security boundary. If you build an AI feature, you must assume the output generated by the model is untrusted and validate it with rigorous, deterministic code.

### Redefining the Next Digital Frontier with MCP

Another highly practical session was delivered by Manuel Sanchez and Carlos Mendible, titled *"AI, Agents and MCP: Redefining the Next Digital Frontier"*. They introduced the Model Context Protocol (MCP)—an emerging open standard designed to structure how AI agents interact with local applications, databases, and development environments.

Sanchez and Mendible highlighted a common mistake developers make when building agentic integrations: exposing raw CRUD (Create, Read, Update, Delete) database tables to the model's context window. This "context bloat" dramatically increases token costs and degrades the agent's reasoning speed.

Instead, they demonstrated how MCP servers should expose high-level, parameter-driven business tools (e.g., executing a specific calculation or pulling a pre-filtered report). This approach pushes computation back onto the database or backend systems, saving tokens and keeping agents highly performant.

| **Integration Pattern** | **Context Bloat (Tokens)**                          | **Latency**                                    | **Security Control**                        |
| ----------------------- | --------------------------------------------------- | ---------------------------------------------- | ------------------------------------------- |
| **Raw CRUD Exposure**   | Extremely High (Exposes raw schema & raw tables)    | High (Model must process entire dataset)       | Very Poor (Relying on model constraints)    |
| **MCP Business Tools**  | Minimal (Exposes targeted APIs/parameterized tools) | Low (Database/backend handles heavy computing) | Excellent (Enforces strict code boundaries) |

### Re-Evaluating Architectural Trade-offs and Climate Impact

Eoin Woods, one of the leading figures in software design, brought invaluable perspective to the GSAS track with his talk, *"The Key to the Prisoners' Dilemma"*. Woods discussed the constant tug-of-war between business speed and long-term architectural stability, using game theory to prove that proper architecture is actually the primary vehicle for sustainable, ongoing business value.

What made Woods' contribution even more fascinating was his work on green software engineering. He pointed out that the carbon emissions of global computing infrastructure are rising rapidly, with ICT emissions projected to reach 5% by 2030—driven in large part by the extreme computational demands of modern AI models.

Woods introduced the concept of **Demand Shifting**. By utilizing smart, orchestrating software architectures, enterprise teams can dynamically route heavy, non-time-sensitive AI training and query workloads to data centers currently operating on clean, excess renewable energy. This simple architectural decision can reduce operational emissions by up to 40% with virtually zero impact on system performance.

## The Conference Experience

Kinépolis Madrid proved to be an outstanding venue for a tech summit of this scale. Showing complex architecture diagrams, SQL configurations, and C# code on IMAX-sized movie screens was a developer's dream. The audio clarity and amphitheater seating ensured that even the most dense, code-heavy presentations felt incredibly engaging.

!(convex_networking_hall.jpg)

But beyond the high-quality presentation rooms, what really set CONVEX apart was the lack of superficial commercial noise. There were no aggressive sales pitches or standard marketing booths trying to reel you in. Instead, the networking areas were filled with genuine technical conversations.

During the coffee breaks and lunch sessions, I spent hours talking with developer advocates, CTOs, and software architects representing the international.NET and open-source communities. We compared notes on our experiences with Blazor WebAssembly, discussed scaling multi-tenant SaaS structures, and debated the practical limits of "vibe coding". The community-first energy was palpable, showing that the real value of these events is built on the shared experiences and connections made off-stage.

## My Key Takeaways

Reflecting on my conversations, my presentation, and the excellent sessions I attended, several core takeaways stand out for any B2B engineering leader:

- **AI features require absolute data boundaries:** Building conversational database features is a powerful way to eliminate custom report backlogs, but you must validate LLM-generated outputs before they reach your data. Never allow an AI model to write directly to a production database, and always validate queries using AST analysis.
- **The Model Context Protocol (MCP) is the new standard:** Rather than creating custom, ad-hoc integrations for every agent, we must design modular MCP servers that expose clean, business-level APIs to AI models. This reduces context bloat and enforces a cleaner separation of concerns.
- **Green software is an architectural priority:** With AI dramatically increasing energy consumption, we can no longer ignore the environmental impact of our software systems. Implementing patterns like Demand Shifting to run heavy workloads during green energy peaks is becoming a vital non-functional requirement.
- **Developer experience remains a massive competitive advantage:** The success of tools like the ABP Framework and pre-built modular architectures is proof that B2B development teams want to focus on business logic rather than writing repetitive boilerplate code. By automating routine tasks like query generation and report compilation, we can free up engineering teams to focus on core platform value.

## Closing

The inaugural CONVEX Summit 2026 was a resounding success. Plain Concepts did an incredible job of transforming three separate industry dialogues into a unified, high-impact event that reflected the real challenges tech organizations face today.

I want to extend my sincere thanks to the organizers, especially Ivan Suárez Álvarez, for putting together such a high-caliber event. I'm also deeply grateful to all the speakers who shared their hard-earned production lessons, and to every member of the.NET and Volosoft communities who stopped by to talk, share feedback, and celebrate our shared passion for building high-quality software.

I left Madrid with a notepad full of new architectural ideas, a stronger network of global peers, and an even deeper conviction that the intersection of structured software architecture and generative AI is the most exciting place to be building right now. I cannot wait to see where these conversations take us, and I look forward to returning for the next edition!

### 6. Used Photos and Placement Details

1. **Görsel Dosya Adı**: `convex_keynote_stage.jpg`
   - **Yerleştirilen Bölüm**: Kısa giriş (Introduction) bölümünün hemen altı.
   - **Alt Metin (Alt Text)**: The grand keynote stage at Kinépolis Ciudad de la Imagen welcoming over 1,200 international technology leaders to CONVEX 2026.
2. **Görsel Dosya Adı**: `alper_ebicoglu_presentation.jpg`
   - **Yerleştirilen Bölüm**: "Speaking at CONVEX: Chat with Your Data" ana başlığının hemen altı.
   - **Alt Metin (Alt Text)**: Alper Ebiçoğlu presenting 'Chat with Your Data' live on stage, detailing the pipeline that bridges natural language with secure enterprise database queries.
3. **Görsel Dosya Adı**: `alper_slide_ast_validation.jpg`
   - **Yerleştirilen Bölüm**: "Speaking at CONVEX: Chat with Your Data" bölümündeki teknik SQL analizi ve matematiksel formülün hemen altı.
   - **Alt Metin (Alt Text)**: An architecture slide showing the schema discovery, LLM processing, and query validation pipeline of the conversational reporting engine.
4. **Görsel Dosya Adı**: `convex_networking_hall.jpg`
   - **Yerleştirilen Bölüm**: "The Conference Experience" bölümünün hemen altı.
   - **Alt Metin (Alt Text)**: Attendees engaging in technical discussions and B2B networking during the breaks in the exhibition hall of Kinépolis Madrid.

### 7. LinkedIn Post Suggestions

#### Post 1: Short & B2B Professional (General Event Review)

> Unifying dotNET, GSAS, and Singularity Tech Day, CONVEX Summit 2026 in Madrid brought together over 1,200 international tech leaders to answer a single question: How do we turn technological potential into real-world software impact?
>
> I was thrilled to take the stage to talk about conversational database architectures. Read my complete B2B conference review for technical highlights on AI security, green computing, and agentic workflows: [Link] #CONVEX2026 #SoftwareArchitecture #EnterpriseAI #DotNet

#### Post 2: Short & Technical (NL-to-SQL Pipeline)

> Letting an LLM generate SQL queries is easy in a demo, but incredibly risky in production. At CONVEX 2026, I shared our.NET-based pipeline for "Chat with Your Data," demonstrating how to use Abstract Syntax Tree (AST) parsing to enforce strict read-only queries at the application boundary.
>
> Curious about dynamic schema discovery, context injection, and preventing context bloat? Check out my latest technical write-up from the Madrid stage: [Link] #ConversationalSQL #SoftwareEngineering #PostgreSQL #B2BTech

#### Post 3: Medium & Analytical (Architectural Focus)

> AI-Guards alone will not save your B2B application. At CONVEX 2026, experts like Chema Alonso reminded us that generative systems are not security boundaries.
>
> As software architects, we must assume LLM outputs are untrusted. In my latest article, I evaluate key architectural takeaways from the Madrid summit—exploring why we must transition to specialized Model Context Protocol (MCP) servers, why we should adopt "Demand Shifting" to lower the carbon footprint of heavy AI workloads, and how to safely design natural-language-to-SQL engines.
>
> Read the full technical breakdown: [Link] #EnterpriseAI #CyberSecurity #SystemDesign #MCP

#### Post 4: Medium & Developer Productivity (SaaS & Frameworks)

> Developer experience remains the ultimate competitive edge. As creators of the open-source ABP Framework, we at Volosoft are always looking for ways to cut out boilerplate code and accelerate feature delivery.
>
> At CONVEX 2026, the discussion shifted from "vibe coding" back to spec-driven architecture. By building conversational reporting tools that handle the data translations while our C# code handles validation and dynamic Excel generation, we can eliminate reporting bottlenecks forever.
>
> Here are my reflections on how modern development, architecture, and AI are finally merging into a single, high-impact narrative: [Link] #DX #DotNet #SaaS #ABPFramework

#### Post 5: Personal & Story-Driven (My Speaker Journey)

> What an incredible week in Madrid! Speaking at the inaugural CONVEX Summit 2026 was an absolute highlight of my year. Sharing the stage at the stunning Kinépolis cinema venue to present our "Chat with Your Data" conversational reporting pipeline was a fantastic experience.
>
> Beyond presenting, what made this trip truly special was the community. Meeting with fellow software architects at the speaker dinner, exploring historical tech challenges, and exchanging notes on modern.NET configurations over coffee made for some unforgettable conversations.
>
> I want to extend a huge thank you to Plain Concepts and Ivan Suárez Álvarez for organizing a stellar event. I've gathered my favorite technical sessions, personal notes, and major architecture takeaways in my latest article. I hope it sparks some great ideas for your team! [Link] #CONVEXSummit #MySpeakerJourney #Volosoft #TechCommunity

### 8. SEO Keyword Suggestions

1. `CONVEX Summit 2026`
2. `Natural language to SQL pipeline`
3. `Alper Ebiçoğlu speaker`
4. `Model Context Protocol MCP`
5. `Abstract Syntax Tree SQL validation`
6. `Dynamic database schema discovery`
7. `Green software demand shifting`
8. `Plain Concepts Madrid`
9. `B2B software architecture AI`
10. `ABP Framework database reporting`

### 9. Social Media Hashtag Suggestions

- `#CONVEX2026`
- `#SoftwareArchitecture`
- `#DotNet`
- `#ConversationalData`
- `#EnterpriseAI`