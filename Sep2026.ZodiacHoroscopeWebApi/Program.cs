using Microsoft.EntityFrameworkCore;
using Sep2026.ConsoleApp4.Database.ZodiacDbContextModels;
using Sep2026.ZodiacHoroscopeWebApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command",LogLevel.None);
var app = builder.Build();

// JSON Import
var scope = app.Services.CreateScope();
{
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    //string filePath = Path.Combine(
    //    app.Environment.ContentRootPath,
    //    "DataFiles",
    //    "ZodiacAndHoroscopeData.json"
    //);

    string jsonContent = File.ReadAllText("C:\\Users\\HP\\Downloads\\ZodiacAndHoroscopeData.json");

    ZodiacAndHoroscopeJson.ResponseModel responseModel =
        JsonSerializer.Deserialize<ZodiacAndHoroscopeJson.ResponseModel>(jsonContent)!;

    if (!db.ZodiacSigns.Any())
    {
        for (int i = 0; i < responseModel.ZodiacData.Length; i++)
        {
            var item = responseModel.ZodiacData[i];

            ZodiacSign zodiac = new ZodiacSign()
            {
                ZodiacName = item.zodiacName,
                Symbol = item.symbol,
                Element = item.element,
                DateRange = item.dateRange,
                Description = item.description
            };

            db.ZodiacSigns.Add(zodiac);
        }
    }

    if (!db.Horoscopes.Any())
    {
        for (int i = 0; i < responseModel.HoroscopeData.Length; i++)
        {
            var item = responseModel.HoroscopeData[i];

            Horoscope horoscope = new Horoscope()
            {
                ZodiacId = item.zodiacId,
                HoroscopeDate = DateOnly.Parse(item.horoscopeDate),
                Overall = item.overall,
                Love = item.love,
                Career = item.career,
                Health = item.health,
                Finance = item.finance,
                LuckyColor = item.luckyColor,
                LuckyNumber = item.luckyNumber
            };

            db.Horoscopes.Add(horoscope);
        }
    }

    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
