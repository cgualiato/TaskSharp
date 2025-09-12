using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TaskSharp.Models;

namespace TaskSharp.Services
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;
        private List<TodoTask> _tasks;

        public JsonTaskRepository(string filePath)
        {
            _filePath = filePath;
            _tasks = Load();
        }

        private List<TodoTask> Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? ".");
                    File.WriteAllText(_filePath, "[]");
                }

                var text = File.ReadAllText(_filePath);
                var items = JsonConvert.DeserializeObject<List<TodoTask>>(text) ?? new List<TodoTask>();
                return items;
            }
            catch
            {
                return new List<TodoTask>();
            }
        }

        public IEnumerable<TodoTask> GetAll() => _tasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue);

        public TodoTask? Get(Guid id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void Add(TodoTask task)
        {
            _tasks.Add(task);
            SaveChanges();
        }

        public void Update(TodoTask task)
        {
            var idx = _tasks.FindIndex(t => t.Id == task.Id);
            if (idx >= 0) _tasks[idx] = task;
            SaveChanges();
        }

        public void Remove(Guid id)
        {
            _tasks.RemoveAll(t => t.Id == id);
            SaveChanges();
        }

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void Import(IEnumerable<TodoTask> tasks)
        {
            _tasks.AddRange(tasks);
            SaveChanges();
        }
    }
}
