using GeoAccessControlAPI.Repositories;
using GeoAccessControlAPI.Repositories.Absrtactions;
using GeoAccessControlAPI.Services;
using GeoAccessControlAPI.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IBlockedCountriesRepository, BlockedCountriesRepository>();
builder.Services.AddSingleton<ITempBlockedCountriesRepository, TempBlockedCountriesRepository>();
builder.Services.AddSingleton<IBlockedAttemptsRepository, BlockedAttemptsRepository>();


builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddHostedService<TempBlockCleanupService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
