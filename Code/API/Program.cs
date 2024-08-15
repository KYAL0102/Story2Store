using System.Data.SQLite;
using ClassLibrary;
using ClassLibrary.Converter;
using Core.Managers;

var builder = WebApplication.CreateBuilder(args);

var connectionStringBuilder = new SQLiteConnectionStringBuilder
{
    DataSource = "data.db",
    ReadOnly = false
};

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SqLiteManager>( _ => new SqLiteManager(connectionStringBuilder.ConnectionString));

builder.WebHost.UseKestrel(options =>
{
    options.ListenLocalhost(GlobalConstants.BackendPort);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsPolicy", policyBuilder =>
    {
        policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins($"http://localhost:{GlobalConstants.FrontendPort}");
    });
});
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TextComponentConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsPolicy");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();