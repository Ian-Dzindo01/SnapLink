using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redis = ConnectionMultiplexer.Connect("localhost:6379");
var db = redis.GetDatabase();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("https://localhost:3001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton<IDatabase>(sp =>
{
    return redis.GetDatabase();
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
