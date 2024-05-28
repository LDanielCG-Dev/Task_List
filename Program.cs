using System;
using System.Collections.Generic;
using System.IO;

class Program
{
	static List<Task> tasks = new List<Task>();
	static string solutionDirectory = GetSolutionDirectory();
	static string filePath = Path.Combine(solutionDirectory, "tasks.txt");

	static void Main(string[] args)
	{
		Console.WriteLine($"File path: {filePath}");  // Mostrar la ruta completa
		LoadTasks();
		bool exit = false;

		while (!exit)
		{
			Console.Clear();
			Console.WriteLine("To-Do List:");
			Console.WriteLine("1. Agregar tarea");
			Console.WriteLine("2. Ver tareas");
			Console.WriteLine("3. Marcar tarea como completada");
			Console.WriteLine("4. Eliminar tarea");
			Console.WriteLine("5. Salir");
			Console.Write("Seleccione una opción: ");

			switch (Console.ReadLine())
			{
				case "1":
					AddTask();
					break;
				case "2":
					ViewTasks();
					break;
				case "3":
					CompleteTask();
					break;
				case "4":
					DeleteTask();
					break;
				case "5":
					exit = true;
					break;
				default:
					Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
					Console.ReadKey();
					break;
			}
		}

		SaveTasks();
	}

	static void AddTask()
	{
		Console.Clear();
		Console.Write("Descripción de la nueva tarea: ");
		string description = Console.ReadLine();
		tasks.Add(new Task(description));
		Console.WriteLine("Tarea agregada. Presione una tecla para continuar...");
		Console.ReadKey();
	}

	static void ViewTasks()
	{
		Console.Clear();
		if (tasks.Count == 0)
		{
			Console.WriteLine("No hay tareas. Presione una tecla para continuar...");
		}
		else
		{
			for (int i = 0; i < tasks.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {tasks[i]}");
			}
		}
		Console.ReadKey();
	}

	static void CompleteTask()
	{
		Console.Clear();
		ViewTasks();
		Console.Write("Número de la tarea a completar: ");
		if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
		{
			tasks[taskNumber - 1].IsCompleted = true;
			Console.WriteLine("Tarea marcada como completada. Presione una tecla para continuar...");
		}
		else
		{
			Console.WriteLine("Número de tarea no válido. Presione una tecla para continuar...");
		}
		Console.ReadKey();
	}

	static void DeleteTask()
	{
		Console.Clear();
		ViewTasks();
		Console.Write("Número de la tarea a eliminar: ");
		if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
		{
			tasks.RemoveAt(taskNumber - 1);
			Console.WriteLine("Tarea eliminada. Presione una tecla para continuar...");
		}
		else
		{
			Console.WriteLine("Número de tarea no válido. Presione una tecla para continuar...");
		}
		Console.ReadKey();
	}

	static void LoadTasks()
	{
		if (File.Exists(filePath))
		{
			var lines = File.ReadAllLines(filePath);
			foreach (var line in lines)
			{
				var parts = line.Split('|');
				if (parts.Length == 2)
				{
					var task = new Task(parts[0]) { IsCompleted = bool.Parse(parts[1]) };
					tasks.Add(task);
				}
			}
		}
	}

	static void SaveTasks()
	{
		var lines = new List<string>();
		foreach (var task in tasks)
		{
			lines.Add($"{task.Description}|{task.IsCompleted}");
		}
		File.WriteAllLines(filePath, lines);
		Console.WriteLine($"Tasks saved to {filePath}");  // Mostrar la ruta completa
	}

	static string GetSolutionDirectory()
	{
		string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
		DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);

		while (directoryInfo != null && !File.Exists(Path.Combine(directoryInfo.FullName, "Task_List.sln")))
		{
			directoryInfo = directoryInfo.Parent;
		}

		if (directoryInfo == null)
		{
			throw new Exception("No se pudo encontrar el archivo de la solución (.sln) en el árbol de directorios.");
		}

		return directoryInfo.FullName;
	}
}
