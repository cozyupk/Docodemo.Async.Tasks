using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Docodemo.Async.Tasks.DefaultRunner;
using Xunit;

namespace Docodemo.Async.Tasks.UnitTests.DefaultRunner
{
    public partial class WriteOnceFieldTests
    {
        [Fact]
        public void SetAndGet_OnAllTasksProcessedAsync()
        {
            var ctx = new DefaultContext<string>();

            static Task fn(IEnumerable<string> _, IEnumerable<AggregateException>? __, CancellationToken ___) => Task.CompletedTask;

            ctx.BuilderContext.SetOnAllTasksProcessedAsync(fn);

            Assert.Equal(fn, ctx.RunnerContext.OnAllTasksProcessedAsync);

            // It's not allowed to set it again
            var ex = Assert.Throws<InvalidOperationException>(
                () => ctx.BuilderContext.SetOnAllTasksProcessedAsync(fn)
            );
            Assert.Equal("Value already set.", ex.Message);
        }

        [Fact]
        public void SetOnAllTasksProcessedAsync_ThrowsIfNull()
        {
            var ctx = new DefaultContext<string>();

            var ex = Assert.Throws<ArgumentNullException>(() =>
                ctx.BuilderContext.SetOnAllTasksProcessedAsync(null)
            );

            Assert.Equal("asyncTask", ex.ParamName);
        }

        [Fact]
        public void Get_Default_TaskContinuationOptions_WhenUnset()
        {
            var ctx = new DefaultContext<string>();
            Assert.Equal(
                TaskContinuationOptions.None,
                ctx.RunnerContext.TaskContinuationOptions
            );
        }

        [Fact]
        public void Get_Set_TaskContinuationOptions_WhenSet()
        {
            var ctx = new DefaultContext<string>();
            ctx.TaskContinuationOptionsField.Set(TaskContinuationOptions.LongRunning);
            Assert.Equal(
                TaskContinuationOptions.LongRunning,
                ctx.RunnerContext.TaskContinuationOptions
            );
        }

        [Fact]
        public void Get_Default_TaskScheduler_WhenUnset()
        {
            var ctx = new DefaultContext<string>();
            Assert.Equal(
                TaskScheduler.Default,
                ctx.RunnerContext.TaskScheduler
            );
        }

        [Fact]
        public void Get_Set_TaskScheduler_WhenSet()
        {
            var ctx = new DefaultContext<string>();
            ctx.TaskSchedulerField.Set(TaskScheduler.Default); // 明示的に Default を再設定
            Assert.Equal(
                TaskScheduler.Default,
                ctx.RunnerContext.TaskScheduler
            );
        }

        [Fact]
        public void TaskScheduler_ReturnsDefault_WhenValueIsExplicitlyNull()
        {
            var ctx = new DefaultContext<string>();
            ctx.TaskSchedulerField.Set(null!); // 明示的に null をセット（nullable に注意）

            Assert.Equal(
                TaskScheduler.Default,
                ctx.RunnerContext.TaskScheduler
            );
        }

        [Fact]
        public void Results_And_Exceptions_Are_Queue_Based()
        {
            var ctx = new DefaultContext<string>();
            var queue = ctx.RunnerContext.Results;
            queue.Enqueue("A");

            var aggregator = ctx.RunnerContext.Exceptions;
            var inner = new InvalidOperationException("X");
            aggregator.Enqueue(new AggregateException(inner));

            Assert.True(queue.TryDequeue(out var result));
            Assert.Equal("A", result);

            Assert.True(aggregator.TryDequeue(out var exception));
            Assert.Equal("X", exception.InnerException?.Message);
        }

        [Fact]
        public void Dispose_ThrowsNoExeption()
        {
            var ctx = new DefaultContext<string>();
            ctx.RunnerContext.Dispose();
        }

        [Fact]
        public void CancellationToken_IsStoredCorrectly()
        {
            var ctx = new DefaultContext<string>();

            var token = ctx.RunnerContext.CancellationToken;

            Assert.Equal(default, token);
        }
    }
}
