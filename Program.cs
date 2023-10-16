using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TarefasDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World");

app.MapGet("frases", async () =>
    await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes")
);

app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
    await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound()
);

app.MapGet("/tarefas/concluidas", async (AppDbContext db) => 
    await db.Tarefas.Where(tarefa => tarefa.IsConclued).ToListAsync()
);

app.MapPut("/tarefas/{id}", async (int id, Tarefa inputTarefa, AppDbContext db) =>
{
    Tarefa? tarefa = await db.Tarefas.FindAsync(id);

    if (tarefa is null) return Results.NotFound();
    tarefa.Name = inputTarefa.Name;
    tarefa.Description = inputTarefa.Description;
    tarefa.IsConclued = inputTarefa.IsConclued;

    await db.SaveChangesAsync();
    return Results.NoContent();
}
);

app.MapDelete("/tarefas/{id}", async (int id, AppDbContext db) =>
{
    if(await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.Ok(tarefa);
    }
    return Results.NotFound();
}
);

app.MapPost("tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
}
);



app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsConclued { get; set; }
    public string? Description { get; set; }

}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}