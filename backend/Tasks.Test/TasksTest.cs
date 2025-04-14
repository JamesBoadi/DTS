namespace Tasks.Test;

[TestFixture]
public class TaskApiTests
{
    private TaskDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(databaseName: $"TaskDb_{Guid.NewGuid()}")
            .Options;

        _context = new TaskDbContext(options);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task CreateTask_ValidTask_ReturnsCreated()
    {
        var newTask = new TaskDto
        {
            Title = "Test Task",
            Description = "Test Desc",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = TaskStatus.Pending,
        };

        var task = new Task
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = newTask.Title,
            Description = newTask.Description,
            DueDate = newTask.DueDate,
            Status = newTask.Status,
        };

        var result = task.Validate();
        Assert.AreEqual(200, result);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var saved = await _context.Tasks.FindAsync(task.Id);
        Assert.NotNull(saved);
        Assert.AreEqual(task.Title, saved.Title);
    }

    [Test]
    public async Task GetAllTasks_WhenTasksExist_ReturnsTasks()
    {
        _context.Tasks.Add(new Task
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "Seed Task",
            Description = "Test",
            DueDate = DateTime.UtcNow,
            Status = TaskStatus.Pending,
        });

        await _context.SaveChangesAsync();

        var tasks = await _context.Tasks.ToListAsync();
        Assert.IsNotEmpty(tasks);
    }

    [Test]
    public async Task GetTaskById_ValidId_ReturnsTask()
    {
        var task = new Task
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "Find Me",
            Description = "Test",
            DueDate = DateTime.UtcNow,
            Status = TaskStatus.Pending
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var result = await _context.Tasks.FindAsync(task.Id);
        Assert.NotNull(result);
        Assert.AreEqual(task.Title, result.Title);
    }

    [Test]
    public async Task UpdateTask_ValidData_UpdatesSuccessfully()
    {
        var task = new Task
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "Old Title",
            Description = "Old Desc",
            DueDate = DateTime.UtcNow,
            Status = TaskStatus.Pending
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Simulate update
        var existing = await _context.Tasks.FindAsync(task.Id);
        existing.Title = "Updated Title";
        await _context.SaveChangesAsync();

        var updated = await _context.Tasks.FindAsync(task.Id);
        Assert.AreEqual("Updated Title", updated.Title);
    }

    [Test]
    public async Task DeleteTask_ValidId_DeletesTask()
    {
        var task = new Task
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "Delete Me",
            Description = "Delete Desc",
            DueDate = DateTime.UtcNow,
            Status = TaskStatus.Pending
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        var result = await _context.Tasks.FindAsync(task.Id);
        Assert.IsNull(result);
    }
}
