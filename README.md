👉 [日本語版はこちら (README.ja.md)](./README.ja.md)

> ⚠️ Warning! This project is currently under development and its architecture may change significantly.  
> Use at your own risk (or refrain from using it until it stabilizes).

# Docodemo.Async.Tasks

## ✨ Overview

This library provides a **fluent API** to compose, execute, and collect results from `Task`-based methods.  
Its intended functionality is similar to [AsyncBridge](https://tejacques.github.io/AsyncBridge/), though it's still in early development.

---

## 📦 Features

- Compose collections of `Task` (including `Task<T>`) and define completion callbacks.
  - Supports `CancellationToken` for cancellation control.

- Task groups can be executed within a synchronization context.
  - You can choose between blocking or non-blocking execution.

- Results are returned either via callbacks or directly.
  - Exceptions are aggregated and provided as part of the result.
  - Callbacks can be synchronous or asynchronous methods.

- Planned extensions (including ambitious ideas):
  - Task timeout configuration
  - Control over worker thread usage and limits (currently executes on the calling thread)
  - Explicit control of the synchronization context used for continuations (currently isolated from `SynchronizationContext.Current`)
  - Verbose/debugging mode (e.g., logging or task tracing)
  - And perhaps... deadlock detection? 😄

---

## 🚀 Usage Example

```csharp
// Coming soon...
```

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