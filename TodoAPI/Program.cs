using Microsoft.EntityFrameworkCore;
using TodoAPI;

var builder = WebApplication.CreateBuilder(args);

// add DI - AddService
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

// configure pipeline - UseMethod
app.MapGet("/todoitems",async (TodoDb db) => await db.Todos.ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) => await db.Todos.FindAsync(id));

app.MapPost("/todoitems", async (TodoItem todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, TodoItem inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null)
    {
        return Results.NotFound();
    }

    todo.Name = inputTodo.Name;
    todo.IsCompleted = inputTodo.IsCompleted;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();