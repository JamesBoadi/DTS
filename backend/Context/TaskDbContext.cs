namespace Tasks.Context.TaskDbContext;
using Microsoft.EntityFrameworkCore;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<Task> Tasks => Set<Task>();
}
