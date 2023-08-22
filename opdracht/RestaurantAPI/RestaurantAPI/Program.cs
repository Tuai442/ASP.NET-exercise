using RestaurantBL.Interfaces;
using RestaurantBL.Services;
using RestaurantDL.Repositories;

var builder = WebApplication.CreateBuilder(args);
string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Tuur\Documents\Gegevens\BACK_UP_LAPTOP\hogent\Web 4\opdracht\RestaurantAPI\Database\Database.mdf"";Integrated Security=True";

// Add services to the container.
builder.Services.AddControllers();



builder.Services.AddSingleton<IGebruikerRepository>(r => new GebruikerRepositoryADO(connectionString));
builder.Services.AddSingleton<IRestaurantRepository>(r => new RestaurantRepositoryADO(connectionString));
builder.Services.AddSingleton<IReservatieRepository>(r => new ReservatieRepositoryADO(connectionString));

builder.Services.AddSingleton<GebruikerService>();
builder.Services.AddSingleton<RestaurantService>();
builder.Services.AddSingleton<ReservatieService>();

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

app.Run();
