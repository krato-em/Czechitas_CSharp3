using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure DI
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>(); // pokud nekdo ze zaregistrovanych kontroleru bude potrebovat IRepository interface, tak mu dej ToDoItemsRepository() tridu
}

var app = builder.Build();
{
    // Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseSwagger(); // rikam, ze chci pouzit swagger
    app.UseSwaggerUI(config => config.SwaggerEndpoint("swagger/v1/swagger.json", "ToDoList API V1")); // rikam, ze chci pouzit UI swaggeru, ktery bude konzumovat definovany json
}

// app.MapGet("/", () => "Hello World!");
// app.MapGet("/test", () => "This is test page!");
// app.MapGet("/czechitas", () => "This is czechitas page");
// app.MapGet("/pozdrav/{jmeno}", (string jmeno) => $"Ahoj {jmeno}");
// app.MapGet("/secti/{a:int}/{b:int}", (int a, int b) => $"Vysledek {a} + {b} je: {a + b}");


app.Run();
