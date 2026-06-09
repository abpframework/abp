# Simple Scenario Draft: Debugging With Tool Access

This is a draft demo script for the article section that will be written later.

The goal is to show that ABP AI Coding Agent does not only reason from files. When ABP Studio tools are enabled, it can use runtime information from the active solution, such as exceptions, logs, requests, application state, tasks, and build results. When a tool is disabled, the agent should stay inside that boundary.

## Demo Idea

Use a small ABP application and deliberately introduce a runtime exception in a page, controller, application service, or endpoint that is easy to trigger from the browser.

Example failure:

- Add a temporary `throw new Exception("Sample exception for demonstrating integrated tools!");` line in a request path.
- Start the application from ABP Studio.
- Trigger the request from the browser so ABP Studio Monitoring captures the exception.

The exact code location can be adjusted depending on the demo solution. The important part is that the exception is captured by ABP Studio Monitoring and can be retrieved through the monitoring tools.

## Before The Demo

Action:

1. Start the application from ABP Studio.
2. Open the page or endpoint that contains the deliberate exception.
3. Confirm that the application shows an error.

This setup is not the main demo step. It only prepares the same runtime failure so the difference between disabled and enabled tool access is easier to see.

## Step 1: Ask Without Runtime Tool Access

Tool setup:

- Keep the monitoring tool that retrieves exceptions disabled. //TODO: add image of disabling it!

Prompt:

```text
Can you get the latest exception from ABP Studio Monitoring and explain what failed?
```

Expected result:

The agent should explain that it cannot directly retrieve the exception details because the required monitoring tool is not enabled. It may still suggest checking the code or enabling the relevant tool.

This is the first important teaching moment: tool access is explicit. If the agent does not have the tool, it should not pretend that it has runtime information.

## Step 2: Enable The Exception Tool

Tool setup:

- Enable the monitoring tool that can retrieve exceptions, such as `get_exceptions`. //TODO: add image of enabling it!!!

Prompt:

```text
Now the get_exceptions tool is enabled. Please get the latest exception from ABP Studio Monitoring, identify the failing code path, and suggest the smallest fix.
```

Expected result:

The agent should retrieve the latest exception details, connect the exception message and stack trace to the related code, and explain the fix.

If Agent mode is being used, the follow-up prompt can allow the agent to apply the fix:

```text
Apply the smallest safe fix for this exception, then verify the application again.
```

Expected result:

The agent should edit the broken code, use available build or application tools to validate the change, and report what was fixed.

## Step 3: Add Logs Or Requests For More Context

Optional prompt:

```text
Use the available monitoring tools to check the related logs and recent requests for this failure. Tell me whether they confirm the same root cause.
```

Expected result:

The agent can correlate the exception with logs or request information, if those tools are enabled. This shows how multiple ABP Studio tools can work together in one troubleshooting flow.

## Step 4: Validate The Fix

Optional prompt:

```text
Run the available build or application validation tools and confirm that the problem is fixed.
```

Expected result:

The agent should use the enabled build or application tools, summarize the result, and mention any remaining risk if validation is incomplete.

## Final Demo Message

The final article section can turn this draft into a short story:

- First, the agent is asked for runtime information without the tool and respects the boundary.
- Then, the tool is enabled and the agent can retrieve the exception.
- Finally, the agent uses the runtime signal to make a focused fix and validate it.

This demonstrates the main value of integrated ABP Studio tools: the AI Coding Agent can move from source code to real solution behavior without forcing the developer to manually copy exception details, logs, requests, or build output into the chat.