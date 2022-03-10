using Newtonsoft.Json.Linq;
using WebAPI.JSON.CRUD.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string fileName = "users.json";
string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
var dbUsers = Database(path);

app.MapGet("/", () => "Hello World! 🔥" + path);

app.MapGet("/users", () =>
{
    return dbUsers.Select("users").ToJsonString();
})
.WithName("GetUsers");

app.MapGet("/users/{id}", (int id) =>
{
    return dbUsers.Select("users").Where("id", id).ToJsonString();
})
.WithName("GetUserById");

app.MapPost("/users", (User user) =>
{
    dbUsers.Add("users", user);
})
.WithName("AddUser");

app.MapDelete("/users/{id}", (int id) =>
{
    dbUsers.Delete("users", Convert.ToString(id), 1);
})
.WithName("RemoveUser");

app.MapPut("/users/{id}", (int id, User user) =>
{
    dbUsers.Update("users", Convert.ToString(id), 3, user);
})
.WithName("EditUser");

app.Run();

JObject Database(string filePath)
{
    return JsonManager.Load(filePath);
}

internal record User()
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public bool Verified { get; set; }
}