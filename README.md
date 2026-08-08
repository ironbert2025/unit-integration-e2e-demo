# SimpleTaskApp — Ejemplo de los 3 tipos de test

Proyecto de práctica (WinForms + SQLite) armado para ver, en un caso
chiquito, cómo se ven Unit / Integration / E2E tests en una app de
escritorio real, antes de aplicarlo a TradeSignal.

## Estructura

```
SimpleTaskApp.sln
├── SimpleTaskApp.Core/              Lógica pura: TaskItem, ITaskRepository, TaskService
├── SimpleTaskApp.Data/              SqliteTaskRepository (implementación real con SQLite)
├── SimpleTaskApp.App/               WinForms UI (MainForm con txtTitle, btnAdd, lstTasks)
│
├── SimpleTaskApp.UnitTests/         xUnit + Moq -> prueba TaskService con repo MOCKEADO
├── SimpleTaskApp.IntegrationTests/  xUnit + SQLite real en memoria -> prueba Service+Repo juntos
└── SimpleTaskApp.E2ETests/          xUnit + FlaUI -> abre el .exe real y simula clics
```

## Cómo correr cada tipo (en tu máquina, con Visual Studio o `dotnet` CLI)

Este entorno de chat no tiene acceso a NuGet ni al SDK de Windows, así
que el código no se compiló aquí — está listo para abrir en Visual
Studio o correr con `dotnet` en tu PC.

### 1. Unitarios (rápidos, sin dependencias externas)
```
cd SimpleTaskApp.UnitTests
dotnet test
```
No necesita nada más. Corre en milisegundos porque `ITaskRepository`
está mockeado con Moq.

### 2. Integración (SQLite real, en memoria)
```
cd SimpleTaskApp.IntegrationTests
dotnet test
```
Usa `Microsoft.Data.Sqlite` con una base en memoria por test — SQL real,
tabla real, sin necesidad de instalar SQL Server ni Docker para este
ejemplo chico.

### 3. End-to-End (abre la app real)
Primero compila la app:
```
cd SimpleTaskApp.App
dotnet build
```
Luego corre los tests E2E (ajusta `RutaExe` en `MainFormE2ETests.cs`
si tu ruta de salida es distinta):
```
cd SimpleTaskApp.E2ETests
dotnet test
```
Esto literalmente abre `SimpleTaskApp.App.exe`, escribe en el textbox,
hace clic en el botón, y verifica lo que aparece en pantalla — igual
que lo harías tú a mano.

## Qué mirar en cada proyecto

- **`TaskService.cs`** — la regla de negocio (título no vacío, máx 100
  caracteres) es lo que prueban los tests unitarios, sin BD de por medio.
- **`SqliteTaskRepository.cs`** — el SQL real; solo los tests de
  integración lo ejercitan de verdad.
- **`MainForm.Designer.cs`** — nota los `AccessibleName` en cada control
  (`txtTitle`, `btnAdd`, `lstTasks`, `lblError`). Eso es lo que FlaUI usa
  como `AutomationId` para encontrar los controles — sin eso, el E2E no
  puede "ver" la UI.

## Siguiente paso

Una vez que corras los 3 tipos aquí y se sientan naturales, aplicamos
exactamente el mismo patrón a TradeSignal:
- Unit → `ZoneAnalyzer`, `TrailingStopV3`
- Integration → `AnalysisEngine` + SQL Server/MinIO (tu setup Docker)
- E2E → `MainForm.cs` de TradeSignal.App con FlaUI
