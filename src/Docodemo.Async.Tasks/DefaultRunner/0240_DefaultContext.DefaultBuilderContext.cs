using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Docodemo.Async.Tasks.Abstractions;

namespace Docodemo.Async.Tasks.DefaultRunner
{
    /// <summary>
    /// Represents the context for an investigation, including cancellation token and semaphore for task completion.
    /// In this partial file, we implement the <see cref="IBuilderContext{TResult}"/> interface.
    /// </summary>
    partial class DefaultContext<TResult>
    {
        /// <summary>
        /// A builder context that provides methods to set actions for task processing.
        /// </summary>
        private class DefaultBuilderContext : IBuilderContext<TResult>
        {
            /// <summary>
            /// Field that stores the instance of parent <see cref="DefaultContext{TResult}"/> for this builder context.
            /// </summary>
            private DefaultContext<TResult> Context { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="BuilderContext"/> class with the specified context.
            /// </summary>
            public DefaultBuilderContext(DefaultContext<TResult> context)
            {
                Context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Sets the action to be executed when all tasks are processed.
            /// </summary>
            public void SetOnAllTasksProcessedAsync(
                Func<IEnumerable<TResult>, IEnumerable<AggregateException>?, CancellationToken, Task>? asyncTask
            )
            {
                Context
                  .OnAllTasksProcessedAsyncField
                    .Set(
                        asyncTask ?? throw new ArgumentNullException(nameof(asyncTask))
                    );
            }
        }
    }
}
