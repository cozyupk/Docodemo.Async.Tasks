from pathlib import Path

# Define the markdown content
readme_content = """
👉 [日本語版はこちら (README.ja.md)](./README.ja.md)

> ⚠️ Warning! This project is currently under development and its architecture may change significantly.  
> Use at your own risk (or refrain from using it until it stabilizes).

# Docodemo.Async.Tasks

## ✨ Overview

**Docodemo.Async.Tasks** provides a fluent, composable API for working with `Task`-based asynchronous methods — regardless of whether your own code is `async` or not.

Its purpose is similar to [AsyncBridge](https://tejacques.github.io/AsyncBridge/), but with a modernized, fluent design and a strong emphasis on developer ergonomics.

---

## 📦 Features

- Safely **invoke `async` methods from non-`async` methods** — without risking deadlocks or blocking the UI thread.
- Compose collections of `Task` / `Task<T>` and define post-completion callbacks.
  - Supports `CancellationToken` for graceful cancellation.
- Optionally **block** until all tasks complete, or run in **non-blocking** mode.
- Handle results:
  - Via final callback (sync or async),
  - Or retrieve them programmatically.
- Aggregate exceptions across all tasks for easier error handling.
- Synchronization context isolation by default — your continuations won't accidentally post back to `SynchronizationContext.Current`.

### 🧪 Planned Features (WIP)

- Timeout support for individual or grouped tasks
- Worker thread usage control (e.g., parallelism limits)
- Fine-grained continuation context selection
- Debug/trace mode for inspection of async task flow
- (Maybe...) deadlock detection 😄

---

## 🚀 Usage Example

```csharp
using Docodemo.Async.Tasks.Extentions; // NOTICE: Visual Studio won't add this automatically

Func<Task<int>>[] funcTasks = new[]
{
    () => DoSomethingAsync(1),
    () => DoSomethingAsync(2),
};

// Call from non-async method in blocking way
funcTasks.ToAsyncHandler(
    exceptions =>
    {
        foreach (var ex in exceptions)
            Console.Error.WriteLine(ex);
    }
).ShallWeGo();

// Call from async method in non-blocking way
funcTasks.ToAsyncHandler(
    exceptions =>
    {
        foreach (var ex in exceptions)
            Console.Error.WriteLine(ex);
    }
).LetThemGo();

---

## 💡 Motivation

Since `async` / `await` was introduced in C# 5.0, asynchronous programming has become essential in .NET development—especially for ASP.NET and GUI applications.

Looking ahead, the rise of generative AI and cloud interactions increases the importance of "waiting" well. Generative AI is *orders of magnitude* slower than pure computation, and applications must be designed to wait intelligently to ensure good user experience and performance.

However, the ecosystem of reusable "software parts" for `async`/`await` remains relatively underdeveloped. Bridging between the synchronous and asynchronous worlds—using things like `Task.Wait()` or `GetAwaiter().GetResult()`—can be confusing for beginners and culturally inconsistent across development teams.

Furthermore, as we aim to automate tests and CI/CD pipelines, there's a growing need for tools that treat asynchronous behavior properly—not just superficially.

This library proposes to treat async execution as a "door"—a metaphorical gateway—helping developers transition safely and flexibly between sync and async worlds.  

With a modern touch, and in a way that (hopefully) contributes a bit to human progress.

That is the mission of **Docodemo.Async.Tasks**.

---

## 🗃️ Nupkg

The core features are still under development and no NuGet package has been released yet.  
Please (patiently) stay tuned.

---

## 📜 License

Apache License 2.0