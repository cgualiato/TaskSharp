using System;
using System.IO;
using TaskSharp.Services;
using TaskSharp.Utils;

var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TaskSharp");
var dataFile = Path.Combine(dataFolder, "tasks.json");

var repo = new JsonTaskRepository(dataFile);
var service = new TaskService(repo);

void PrintTask(TaskSharp.Models.TodoTask t)
{
    Console.WriteLine($"Id: {t.Id}");
    Console.WriteLine($"Title: {t.Title}");
    Console.WriteLine($"Description: {t.Description}");
    Console.WriteLine($"CreatedAt: {t.CreatedAt:u}");
    Console.WriteLine($"DueDate: {(t.DueDate?.ToString("u") ?? "—")}");
    Console.WriteLine($"Completed: {t.Completed}");
    Console.WriteLine($"Tags: {string.Join(", ", t.Tags)}");
    Console.WriteLine(new string('-', 30));
}

while (true)
{
    Console.Clear();
    Console.WriteLine("TaskSharp — Gerenciador de Tarefas (Console)");
    Console.WriteLine("1) Listar tarefas");
    Console.WriteLine("2) Criar tarefa");
    Console.WriteLine("3) Editar tarefa");
    Console.WriteLine("4) Remover tarefa");
    Console.WriteLine("5) Buscar");
    Console.WriteLine("6) Exportar tarefas (arquivo JSON)");
    Console.WriteLine("7) Importar tarefas (arquivo JSON)");
    Console.WriteLine("0) Sair");
    Console.Write("Escolha: ");
    var choice = Console.ReadLine();

    try
    {
        if (choice == "0") break;
        if (choice == "1")
        {
            var all = service.ListAll().ToList();
            if (!all.Any()) Console.WriteLine("Nenhuma tarefa encontrada.");
            foreach (var t in all) PrintTask(t);
            Console.WriteLine("Enter para voltar...");
            Console.ReadLine();
        }
        else if (choice == "2")
        {
            var title = ConsoleHelper.ReadNonEmpty("Título: ");
            Console.Write("Descrição (opcional): ");
            var desc = Console.ReadLine();
            var due = ConsoleHelper.ReadDate("Due date (YYYY-MM-DD) ou Enter: ");
            var tags = ConsoleHelper.ReadTags("Tags (separadas por vírgula) ou Enter: ");
            var created = service.Create(title, desc, due, tags);
            Console.WriteLine("Criado com Id: " + created.Id);
            Console.ReadLine();
        }
        else if (choice == "3")
        {
            Console.Write("Digite o Id da tarefa: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido"); Console.ReadLine(); continue; }
            var t = service.Get(id);
            if (t == null) { Console.WriteLine("Não encontrada"); Console.ReadLine(); continue; }
            PrintTask(t);
            Console.WriteLine("a) Marcar/Desmarcar concluída");
            Console.WriteLine("b) Editar título/descrição/due/tags");
            Console.Write("Escolha: ");
            var opt = Console.ReadLine();
            if (opt == "a")
            {
                service.Update(id, task => task.Completed = !task.Completed);
                Console.WriteLine("Atualizado.");
            }
            else if (opt == "b")
            {
                Console.Write("Novo título (Enter = manter): ");
                var newTitle = Console.ReadLine();
                Console.Write("Nova descrição (Enter = manter): ");
                var newDesc = Console.ReadLine();
                var newDue = ConsoleHelper.ReadDate("Novo due date (Enter = manter): ");
                Console.Write("Novas tags (vírgula) (Enter = manter): ");
                var tagsLine = Console.ReadLine();
                service.Update(id, task =>
                {
                    if (!string.IsNullOrWhiteSpace(newTitle)) task.Title = newTitle;
                    if (!string.IsNullOrWhiteSpace(newDesc)) task.Description = newDesc;
                    if (newDue.HasValue) task.DueDate = newDue;
                    if (!string.IsNullOrWhiteSpace(tagsLine)) task.Tags = tagsLine.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                });
                Console.WriteLine("Atualizado.");
            }
            Console.ReadLine();
        }
        else if (choice == "4")
        {
            Console.Write("Id para remover: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido"); Console.ReadLine(); continue; }
            if (service.Delete(id)) Console.WriteLine("Removido"); else Console.WriteLine("Não encontrado");
            Console.ReadLine();
        }
        else if (choice == "5")
        {
            Console.Write("Termo de busca: ");
            var q = Console.ReadLine() ?? "";
            var res = service.Search(q).ToList();
            if (!res.Any()) Console.WriteLine("Nenhum resultado.");
            foreach (var t in res) PrintTask(t);
            Console.ReadLine();
        }
        else if (choice == "6")
        {
            Console.Write("Caminho do arquivo para exportar: ");
            var path = Console.ReadLine();
            var all = service.ListAll().ToList();
            System.IO.File.WriteAllText(path ?? "export.json", Newtonsoft.Json.JsonConvert.SerializeObject(all, Newtonsoft.Json.Formatting.Indented));
            Console.WriteLine("Exportado.");
            Console.ReadLine();
        }
        else if (choice == "7")
        {
            Console.Write("Caminho do arquivo JSON para importar: ");
            var path = Console.ReadLine();
            if (!File.Exists(path))
            {
                Console.WriteLine("Arquivo não encontrado.");
                Console.ReadLine();
                continue;
            }
            var text = File.ReadAllText(path);
            var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TaskSharp.Models.TodoTask>>(text) ?? new List<TaskSharp.Models.TodoTask>();
            service.Import(items);
            Console.WriteLine($"Importados: {items.Count}");
            Console.ReadLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro: " + ex.Message);
        Console.ReadLine();
    }
}
