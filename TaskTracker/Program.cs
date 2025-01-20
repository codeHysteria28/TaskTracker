using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class Program
{
    private static TaskList taskList = new TaskList();

    public class TaskList
    {
        public List<Task> Tasks { get; set; } = new List<Task>();
    }

    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: TaskTracker <command> [<args>]");
        }

        string command = args[0];
        string fullPath = @"C:\Users\User\Desktop\cslearning\TaskTracker\TaskTracker\bin\Debug\net8.0\tasks.json";

        switch (command.ToLower())
        {
            case "create":
                string description = args[1];
                DateTime createdAt = DateTime.Now;
                DateTime updatedAt = DateTime.Now;

                int id = GetNextId(fullPath);
                Task task = new Task(id, description, "todo", createdAt, updatedAt);
                CreateTask(task, fullPath);
                break;
            case "update":
                int taskId = int.Parse(args[1]);
                string updatedDescription = args[2];
                UpdateTask(taskId, updatedDescription, fullPath);
                break;
            case "delete":
                int taskIdToDelete = int.Parse(args[1]);
                DeleteTask(taskIdToDelete, fullPath);
                break;
            case "list":
                if(args.Length > 1)
                {
                    string status = args[1];
                    ListAllTasks(fullPath, status);
                    break;
                }
                ListAllTasks(fullPath);
                break;
            case "mark-in-progress":
                int markInProgressTaskId = int.Parse(args[1]);
                ChangeStatus(markInProgressTaskId, fullPath, "in-progress");
                break;
            case "mark-done":
                int markDoneTaskId = int.Parse(args[1]);
                ChangeStatus(markDoneTaskId, fullPath, "done");
                break;

        }
    }

    // create a new task
    public static void CreateTask(Task task, string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();
            }
            else
            {
                taskList = new TaskList();
            }

            taskList.Tasks.Add(task);

            string jsonString = JsonSerializer.Serialize(taskList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, jsonString);

            Console.WriteLine($"File written to: {path}");
            Console.WriteLine(File.ReadAllText(path));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    // update existing task
    public static void UpdateTask(int taskId, string updatedDescription, string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();

                var task = taskList.Tasks.Find(t => t.Id == taskId);

                if (task != null)
                {
                    task.Description = updatedDescription;
                    task.UpdatedAt = DateTime.Now;
                    string jsonString = JsonSerializer.Serialize(taskList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, jsonString);

                    Console.WriteLine($"Task with ID {taskId} updated.");
                }
                else
                {
                    Console.WriteLine($"Task with ID {taskId} not found.");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    // Delete task
    public static void DeleteTask(int taskId, string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();
                var task = taskList.Tasks.Find(t => t.Id == taskId);

                if (task != null)
                {
                    taskList.Tasks.Remove(task);
                    string jsonString = JsonSerializer.Serialize(taskList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, jsonString);
                    Console.WriteLine($"Task with ID {taskId} was deleted.");
                }
                else
                {
                    Console.WriteLine($"Task with ID {taskId} not found.");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }

    // list all tasks
    public static void ListAllTasks(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();

                if (taskList.Tasks.Count == 0)
                {
                    Console.WriteLine("No tasks found.");
                } else
                {
                    foreach (var task in taskList.Tasks)
                    {
                        if(task.Status == "done")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(task.ToString());
                            Console.ResetColor();
                        }
                        else if (task.Status == "in-progress")
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine(task.ToString());
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(task.ToString());
                        }

                    }
                }
            } else
            {
                Console.WriteLine("No tasks found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }

    // List all tasks by status
    public static void ListAllTasks(string path, string status)
    {
        try
        {
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();

                if (taskList.Tasks.Count == 0)
                {
                    Console.WriteLine("No tasks found.");
                }
                else
                {
                    bool tasksFound = false;
                    foreach (var task in taskList.Tasks)
                    {
                        if(task.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                        {
                            tasksFound = true;
                            if(status == "done")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }else if(status == "in-progress")
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            }
                            Console.WriteLine(task.ToString());
                            Console.ResetColor();
                        }
                    }

                    if (!tasksFound)
                    {
                        Console.WriteLine($"No tasks found with status '{status}'");
                    }
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }

    // change tasks status 
    public static void ChangeStatus(int taskId, string path, string status)
    {
        try
        {
            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();

                var task = taskList.Tasks.Find(t => t.Id == taskId);

                if (task != null)
                {
                    task.Status = status;
                    task.UpdatedAt = DateTime.Now;

                    string jsonString = JsonSerializer.Serialize(taskList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, jsonString);

                    Console.WriteLine($"Task with ID {taskId} was marked as {status}.");
                }
                else
                {
                    Console.WriteLine($"Task with ID {taskId} not found.");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static int GetNextId(string path)
    {
        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            taskList = JsonSerializer.Deserialize<TaskList>(existingJson) ?? new TaskList();

            if(taskList.Tasks.Count > 0)
            {
                return taskList.Tasks[^1].Id + 1;
            }
        }

        return 1;
    }
}
