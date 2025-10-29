using ToDoList.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure DI
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
}

var app = builder.Build();
{
    // Configure Middleware (HTTP request pipeline)
    app.MapControllers();
}

// app.MapGet("/", () => "Hello World!");
// app.MapGet("/test", () => "This is test page!");
// app.MapGet("/czechitas", () => "This is czechitas page");
// app.MapGet("/pozdrav/{jmeno}", (string jmeno) => $"Ahoj {jmeno}");
// app.MapGet("/secti/{a:int}/{b:int}", (int a, int b) => $"Vysledek {a} + {b} je: {a + b}");


app.Run();
