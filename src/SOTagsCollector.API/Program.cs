using Microsoft.EntityFrameworkCore;
using Serilog;
using SOTagsCollector.API;
using SOTagsCollector.API.Clients;
using SOTagsCollector.API.Installers;
using SOTagsCollector.API.Persistence;
using SOTagsCollector.API.Repositories;
using SOTagsCollector.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddOptions<StackExchangeConfig>()
    .Bind(builder.Configuration.GetSection(StackExchangeConfig.SectionName))
    .ValidateOnStart();
builder.Services.AddDbContext<TagsDb>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("tagsdb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();
builder
    .ConfigureLogging()
    .AddHttpClients()
    .AddMessaging()
    .AddEndpoints();

var app = builder.Build();

app.SeedTags();
app.UseSwagger();
app.UseSwaggerUI();
app.MapTags();
app.Run();

public partial class Program{}