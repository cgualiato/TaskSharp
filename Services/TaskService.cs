using System;
using System.Collections.Generic;
using System.Linq;
using TaskSharp.Models;

namespace TaskSharp.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo) => _repo = repo;

        public IEnumerable<TodoTask> ListAll() => _repo.GetAll();

        public TodoTask? Get(Guid id) => _repo.Get(id);

        public TodoTask Create(string title, string? description, DateTime? dueDate, IEnumerable<string>? tags)
        {
            var t = new TodoTask
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Tags = tags?.Select(s => s.Trim()).Where(s => s.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>()
            };
            _repo.Add(t);
            return t;
        }

        public bool Update(Guid id, Action<TodoTask> updater)
        {
            var t = _repo.Get(id);
            if (t == null) return false;
            updater(t);
            _repo.Update(t);
            return true;
        }

        public bool Delete(Guid id)
        {
            var t = _repo.Get(id);
            if (t == null) return false;
            _repo.Remove(id);
            return true;
        }

        public IEnumerable<TodoTask> Search(string q)
        {
            q = q?.ToLowerInvariant() ?? "";
            return _repo.GetAll().Where(t =>
                (t.Title?.ToLowerInvariant().Contains(q) ?? false) ||
                (t.Description?.ToLowerInvariant().Contains(q) ?? false) ||
                t.Tags.Any(tag => tag.ToLowerInvariant().Contains(q))
            );
        }

        public void Import(IEnumerable<TodoTask> tasks) => _repo.Import(tasks);
    }
}
