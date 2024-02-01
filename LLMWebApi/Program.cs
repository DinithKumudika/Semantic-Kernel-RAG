using LLMWebApi;
using LLMWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bot intialization
BotService.Init(builder);

// OpenAI embedding initialization
EmbeddingService.Init(BotService.KernelBuilder, builder);
// Qdrant Memory initialization
VectorDbService.Init(builder, BotService.BotKernel);

var db = new ChatHistoryContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();