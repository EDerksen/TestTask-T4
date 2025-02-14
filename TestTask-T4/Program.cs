using Microsoft.EntityFrameworkCore;
using TestTask_T4.Data;
using TestTask_T4.Middleware;
using TestTask_T4.Services.Clients;
using TestTask_T4.Services.Finance;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinanceDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("FinanceDB")));

builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddTransient<ITransactionValidator, TransactionValidator>();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    db.Database.Migrate();
}

app.Run();
