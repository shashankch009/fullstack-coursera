using System;
using System.Collections.Generic;

public class TaskExecutor
{
    private Queue<string> taskQueue = new Queue<string>();
    private const int MaxRetries = 3; // Maximum retry attempts for a task

    public void AddTask(string task)
    {
        if (task == null)
        {
            Console.WriteLine("Cannot add a null task to the queue.");
            return;
        }
        taskQueue.Enqueue(task);
    }

    public void ProcessTasks()
    {
        while (taskQueue.Count > 0)
        {
            string task = taskQueue.Dequeue();
            int retryCount = 0;

            while (retryCount < MaxRetries)
            {
                try
                {
                    Console.WriteLine($"Processing task: {task} (Attempt {retryCount + 1})");
                    ExecuteTask(task);
                    break; // Exit retry loop on success
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"Error processing task '{task}': {ex.Message}");
                    if (retryCount == MaxRetries)
                    {
                        Console.WriteLine($"Task '{task}' failed after {MaxRetries} attempts.");
                    }
                }
            }
        }
    }

    private void ExecuteTask(string task)
    {
        if (task == null)
        {
            throw new Exception("Task is null");
        }
        if (task.Contains("Fail"))
        {
            throw new Exception("Task execution failed");
        }
        // Simulate task execution
        Console.WriteLine($"Task {task} completed successfully.");
    }
}

class Program
{
    static void Main()
    {
        TaskExecutor executor = new TaskExecutor();
        executor.AddTask("Task 1");
        executor.AddTask(null); // This will now be handled gracefully
        executor.AddTask("Fail Task"); // This will now be handled gracefully
        executor.AddTask("Task 2");
        executor.ProcessTasks();
    }
}
