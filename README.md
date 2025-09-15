# TaskSharp

Console app em C# para gerenciar tarefas. Projeto demonstrativo para portfolio.

## Tech
- .NET 8
- Newtonsoft.Json
- xUnit (tests)

## Run
```bash
dotnet restore
dotnet build
dotnet run --project TaskSharp.csproj
```

## Tests
```bash
dotnet test ./tests/TaskSharp.Tests.csproj
```

## Commits (simulated)
- feat: add initial project structure
- feat: add TodoTask model
- feat: add Json repository and TaskService
- feat: add console UI and import/export
- test: add unit tests for TaskService
- ci: add GitHub Actions CI workflow
