//Entity Framework core is an ORM(object relational mapping) that makes all the management of data or cryd operations simple


//These were for the dependency injection logging stuff
//using Serilog;
//using Magic_Villa_VillaAPI.Logging;

using Magic_Villa_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

/*
 * using Serilog was related to this
 * Installed Serilog.AspNetCore and Serilog.Sinks.File nuget packages to make this work
//this is configuring the logger configuration using Serilog
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog(); //tells it to use Serilog logging instead of console logging

*/
builder.Services.AddControllers(option =>{
    //option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();//addxml allows you the api to support xml 
//adding the option lambda makes sure that only json data is accepted, if other type you get the error message
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ILogging, LoggingV2>();
    //check addsingleton, scope and transient

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
