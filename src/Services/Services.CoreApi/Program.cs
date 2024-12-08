using devdeer.Libraries.Abstractions.Extensions;

using Logic.Interfaces.Logic;
using Logic.Models.Options;
using Logic.OpenAI;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(typeof(Program).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IOpenAILogic, OpenAILogic>();
builder.Services.RegisterOption<OpenAIOptions>("OpenAI");
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