using BlockedCountriesApi.BackgroundServices;
using BlockedCountriesApi.Repositories;
using BlockedCountriesApi.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<BlockedCountryRepository>();


builder.Services.AddSingleton<CountryService>();
builder.Services.AddSingleton<LoggingService>();
builder.Services.AddHttpClient<IpService>();  


 builder.Services.AddHostedService<TemporalBlockCleanerService>();


var app = builder.Build();

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 

app.UseAuthorization();

app.MapControllers();

app.Run();
