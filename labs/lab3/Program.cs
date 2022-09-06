using Hangfire;
using Hangfire.MemoryStorage;
using lab3;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection("app").Get<Settings>();

builder.Services.AddTransient<CurrencyService>();
builder.Services.AddSingleton(_ =>
    new GoogleAnalyticsApi(settings.FirebaseAppId, settings.ApiSecret, settings.InstanceId));

builder.Services.AddHangfire(x => x.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    BackgroundJob.Schedule<CurrencyService>(x => x.SendToGaAsync(), TimeSpan.FromSeconds(5));
});

app.Run();

