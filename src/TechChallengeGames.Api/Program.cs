using Microsoft.Extensions.Options;
using TechChallengeGames.Api.Configurations;
using TechChallengeGames.Api.Middlewares;
using TechChallengeGames.Application;
using TechChallengeGames.Data;
using TechChallengeGames.Elasticsearch;
using TechChallengeGames.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

#region [Database]
builder.Services.AddSqlContext(builder.Configuration);
builder.Services.AddRepositories();
#endregion

#region [JWT]
builder.Services.AddSecurity(builder.Configuration);
#endregion

#region [Services]
builder.Services.AddServices();
#endregion

#region [Serilog]
builder.AddSerilog();
#endregion

#region [ElasticSearch]
builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection(nameof(ElasticSettings)));
builder.Services.AddSingleton<IElasticSettings>(sp => sp.GetRequiredService<IOptions<ElasticSettings>>().Value);
#endregion

#region [DI]
builder.Services.AddSingleton(typeof(IElasticClient<>), typeof(ElasticClient<>));
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();