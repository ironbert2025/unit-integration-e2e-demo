using Moq;
using SimpleTaskApp.Core;
using Xunit;

namespace SimpleTaskApp.UnitTests;

// UNIT TEST: tests ONLY TaskService's logic.
// The repository is MOCKED (Moq) -- it never touches SQLite, disk,
// or anything external. That's why it runs in milliseconds.
public class TaskServiceTests
{
    [Fact]
    public void AddTask_WithEmptyTitle_ThrowsException()
    {
        var repoMock = new Mock<ITaskRepository>();
        var service = new TaskService(repoMock.Object);

        Assert.Throws<ArgumentException>(() => service.AddTask(""));

        // Verifies that, since validation failed, Save was NEVER attempted
        repoMock.Verify(r => r.Save(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public void AddTask_WithTooLongTitle_ThrowsException()
    {
        var repoMock = new Mock<ITaskRepository>();
        var service = new TaskService(repoMock.Object);
        var longTitle = new string('a', 101);

        Assert.Throws<ArgumentException>(() => service.AddTask(longTitle));
    }

    [Theory]
    [InlineData("Buy milk")]
    [InlineData("Review TradeSignal")]
    public void AddTask_WithValidTitle_CallsRepositoryOnce(string title)
    {
        var repoMock = new Mock<ITaskRepository>();
        repoMock.Setup(r => r.Save(It.IsAny<TaskItem>()))
                .Returns((TaskItem t) => t); // simulates saving and returning the same item

        var service = new TaskService(repoMock.Object);
        var result = service.AddTask(title);

        Assert.Equal(title, result.Title);
        repoMock.Verify(r => r.Save(It.IsAny<TaskItem>()), Times.Once);
    }
}
