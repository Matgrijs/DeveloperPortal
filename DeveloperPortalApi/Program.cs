using System.Text.Json.Serialization;
using DeveloperPortalApi.Data;
using DeveloperPortalApi.Hubs;
using DeveloperPortalApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionContext"));
});

builder.Services.AddControllers()
    .AddJsonOptions(o => {o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;});
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<PokerService>();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("https://localhost:7059", "http://localhost:5186", "http://192.168.2.28:5186", "http://0.0.0.0:5186")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");
app.MapHub<PokerHub>("/pokerHub");

app.Run();