using Microsoft.EntityFrameworkCore;

using Tasks.Context.TaskDbContext;
using Tasks.Extension.Validate;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
 
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
     
services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString")));
 
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My TaskApi V1");
});
    

// Create a new task and add it to tasks list   
app.MapPost("/createTask", async (TaskDto newTask, TaskDbContext db) =>
{
    Task task = new Task
    {
        Id = Guid.NewGuid().ToString("N"),
        Title = newTask.Title,
        DueDate = newTask.DueDate,
        Description = newTask.Description,
    };
    
    var validationResult = task.Validate();

    if (!validationResult.Equals(StatusCodes.Status200OK))
        return validationResult;

    db.Tasks.Add(task);

    await db.SaveChangesAsync();

    return Results.Created($"/task/{task.Id}", task);
});

// Get a specific task
app.MapGet("/task/{id}", async (string id, TaskDbContext db) =>
{
    if (id.Equals("") || id is null) return Results.BadRequest($"{id} not in the correct format!");
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound($"Task with Id: {id} not found!");
    return Results.Ok(task);
});

// Get all tasks
app.MapGet("/tasks", async (TaskDbContext db) =>
{
    var tasks = await db.Tasks.ToListAsync();
    if (tasks is null) return Results.NotFound($"There are no tasks!");
    return Results.Ok(tasks);
});

 
// Update a task
app.MapPut("/task/{id}", async (string id, TaskDto updatedTask, TaskDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(id))
        return Results.BadRequest("Task ID is required and cannot be empty.");

    var task = await db.Tasks.FindAsync(id);

    if (task is null)
        return Results.NotFound($"Task with Id: {id} not found!");

    // Update the properties
    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.DueDate = updatedTask.DueDate;
    task.Status = updatedTask.Status;

    await db.SaveChangesAsync();

    return Results.Ok(task);
});


// Delete a task
app.MapDelete("/task/{id}", async (string id, TaskDbContext db) =>
{
    if (id.Equals("") || id is null) return Results.BadRequest($"{id} not in the correct format!");

    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound($"Task with Id: {id} not found");

    db.Tasks.Remove(task);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();



