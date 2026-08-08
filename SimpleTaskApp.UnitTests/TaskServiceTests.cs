using Moq;
using SimpleTaskApp.Core;
using Xunit;

namespace SimpleTaskApp.UnitTests;

// TEST UNITARIO: prueba SOLO la logica de TaskService.
// El repositorio se MOCKEA (Moq) -- no toca SQLite, no toca disco,
// no toca nada externo. Por eso corre en milisegundos.
public class TaskServiceTests
{
    [Fact]
    public void AddTask_ConTituloVacio_LanzaExcepcion()
    {
        var repoMock = new Mock<ITaskRepository>();
        var service = new TaskService(repoMock.Object);

        Assert.Throws<ArgumentException>(() => service.AddTask(""));

        // Verifica que, como fallo la validacion, NUNCA se intento guardar
        repoMock.Verify(r => r.Save(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public void AddTask_ConTituloMuyLargo_LanzaExcepcion()
    {
        var repoMock = new Mock<ITaskRepository>();
        var service = new TaskService(repoMock.Object);
        var tituloLargo = new string('a', 101);

        Assert.Throws<ArgumentException>(() => service.AddTask(tituloLargo));
    }

    [Theory]
    [InlineData("Comprar leche")]
    [InlineData("Revisar TradeSignal")]
    public void AddTask_ConTituloValido_LlamaAlRepositorioUnaVez(string titulo)
    {
        var repoMock = new Mock<ITaskRepository>();
        repoMock.Setup(r => r.Save(It.IsAny<TaskItem>()))
                .Returns((TaskItem t) => t); // simula que guardo y devolvio el mismo item

        var service = new TaskService(repoMock.Object);
        var resultado = service.AddTask(titulo);

        Assert.Equal(titulo, resultado.Title);
        repoMock.Verify(r => r.Save(It.IsAny<TaskItem>()), Times.Once);
    }
}
