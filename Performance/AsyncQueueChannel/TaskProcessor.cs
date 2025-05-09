using System;
using System.Threading.Tasks;

public class TaskProcessor
{
    private readonly TaskQueue _taskQueue;

    public TaskProcessor(TaskQueue taskQueue)
    {
        _taskQueue = taskQueue;
    }

    public async Task StartProcessing()
    {
        while (await _taskQueue.GetReader().WaitToReadAsync())
        {
            var task = await _taskQueue.DequeueTask();
            await task(); // Execute the task
        }
    }
}