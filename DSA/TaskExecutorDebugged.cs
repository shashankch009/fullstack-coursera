using System;
using System.Collections.Generic;

public class TaskExecutor
{
    private Queue<string> taskQueue = new Queue<string>();

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
            try
            {
                Console.WriteLine($"Processing task: {task}");
                ExecuteTask(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing task '{task}': {ex.Message}");
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
