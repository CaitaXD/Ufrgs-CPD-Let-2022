using System.Threading.Tasks.Dataflow;

namespace CPD_Lab_01;

public static class ParalallelAsyncEnumerableExtentions
{
    public static async Task AsyncParallelForEach<A>(this IAsyncEnumerable<A> source, Func<A, Task> body, int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler? scheduler = null)
    {
        var options = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        if (scheduler != null)
            options.TaskScheduler = scheduler;

        var block = new ActionBlock<A>(body, options);

        await foreach (var item in source)
            block.Post(item);

        block.Complete();
        await block.Completion;
    }
}
