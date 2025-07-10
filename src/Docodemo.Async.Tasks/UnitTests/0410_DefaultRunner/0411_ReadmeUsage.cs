using System;
using System.Threading.Tasks;
using Docodemo.Async.Tasks.Extentions; // NOTICE: You need to write it because Visual Studio doesn't add it automatically

namespace Test
{
    public class ReadmeUsage
    {
        async Task DoSomethingAsync(int id)
        {
            await Task.Delay(1000); // Simulate some async work
            Console.WriteLine($"Task {id} completed");
        }

        public void ExampleUsage()
        {
            var funcTasks = new[]
            {
                () => DoSomethingAsync(1),
                () => DoSomethingAsync(2),
            };

            // Call from non-async method in blocking way
            funcTasks.ToAsyncHandler(
                (exceptions) => { }
            ).ShallWeGo();

            // Call from async method in non-blocking way
            funcTasks.ToAsyncHandler(
                (exceptions) => { }
            ).LetThemGo();

        }
    }
}
