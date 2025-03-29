using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redis = ConnectionMultiplexer.Connect("localhost:6379");
var db = redis.GetDatabase();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton<IDatabase>(sp =>
{
    return redis.GetDatabase();
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
