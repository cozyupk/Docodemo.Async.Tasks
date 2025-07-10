using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Docodemo.Async.Tasks.Abstractions;

namespace Docodemo.Async.Tasks.DefaultRunner
{
	/// <summary>
	/// Represents the context for an investigation, including cancellation token and semaphore for task completion.
	/// </summary>
	public partial class DefaultContext<TResult> : IContext<TResult>
	{
        public virtual IBuilderContext<TResult> BuilderContext
            => new DefaultBuilderContext(this);

        public virtual IRunnerContext<TResult> RunnerContext
            => new DefaultRunnerContext(this);

        /// <summary>
        /// Field that stores the action to be executed when all tasks are processed.
        /// </summary>
        internal NullableConcurrentWriteOnceField
					<Func<IEnumerable<TResult>, IEnumerable<AggregateException>?, CancellationToken, Task>>
						OnAllTasksProcessedAsyncField { get; } = new();

        /// <summary>
        /// Get TaskContinuationOptions used for Task.ContinueWith() in the runner.
        /// </summary>
        internal NullableConcurrentWriteOnceField<TaskContinuationOptions>
                        TaskContinuationOptionsField { get; } = new();

        /// <summary>
        /// Get TaskScheduler used for Task.ContinueWith() in the runner.
        /// </summary>
        internal NullableConcurrentWriteOnceField<TaskScheduler>
                        TaskSchedulerField { get; } = new();
    }
}
