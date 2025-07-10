namespace Docodemo.Async.Tasks.Abstractions
{
    /// <summary>
    /// Represents the context for async door context builders and runners.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IContext<TResult>
    {
        /// <summary>
        /// Represents the context for async door context builders.
        /// </summary>
        IBuilderContext<TResult> BuilderContext { get; }

        /// <summary>
        /// Represents the context for async door runners.
        /// </summary>
        IRunnerContext<TResult> RunnerContext { get; }
    }
}
