using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TaskSharp.Models;
using TaskSharp.Services;
using Xunit;

public class TaskServiceTests
{
    [Fact]
    public void Create_AddsTask()
    {
        var repoMock = new Mock<ITaskRepository>();
        var created = new List<TodoTask>();
        repoMock.Setup(r => r.Add(It.IsAny<TodoTask>())).Callback<TodoTask>(t => created.Add(t));
        var svc = new TaskService(repoMock.Object);

        var t = svc.Create("Title", "Desc", null, new []{"test"});
        Assert.NotNull(t);
        Assert.Equal("Title", t.Title);
        Assert.Single(created);
    }

    [Fact]
    public void Search_ReturnsMatching()
    {
        var list = new List<TodoTask>
        {
            new TodoTask{ Title="Comprar leite", Description="Ir ao mercado", Tags = new List<string>{"compras"} },
            new TodoTask{ Title="Ler livro", Description="Capítulo 1" }
        };
        var repoMock = new Mock<ITaskRepository>();
        repoMock.Setup(r=>r.GetAll()).Returns(list);
        var svc = new TaskService(repoMock.Object);

        var res = svc.Search("leite").ToList();
        Assert.Single(res);
    }
}
