using System;
using System.Collections.Generic;
using TaskSharp.Models;

namespace TaskSharp.Services
{
    public interface ITaskRepository
    {
        IEnumerable<TodoTask> GetAll();
        TodoTask? Get(Guid id);
        void Add(TodoTask task);
        void Update(TodoTask task);
        void Remove(Guid id);
        void SaveChanges();
        void Import(IEnumerable<TodoTask> tasks);
    }
}
