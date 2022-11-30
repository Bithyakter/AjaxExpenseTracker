using API.LogginMiddleWare;
using Infrastructure.Contracts;
using Infrastructure.Repositories;
using Infrastructure.Sql;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
{
   o.AddPolicy("AllowAll", new CorsPolicyBuilder()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed(origin => true)
                   .AllowCredentials()
                   .Build());
});

// ConnectionString Added
builder.Services.AddDbContext<DataContext>(opsions => opsions.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpLogging(logging =>
{
   logging.LoggingFields = HttpLoggingFields.All;
   logging.RequestHeaders.Add(HeaderNames.Accept);
   logging.RequestHeaders.Add(HeaderNames.ContentType);
   logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
   logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
   logging.RequestHeaders.Add(HeaderNames.ContentLength);

   logging.MediaTypeOptions.AddText("application/json");
   logging.MediaTypeOptions.AddText("multipart/form-data");

   logging.RequestBodyLogLimit = 4096;
   logging.ResponseBodyLogLimit = 4096;
});

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration).Enrich.FromLogContext().WriteTo.Console()
);
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

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest);

app.UseCors("AllowAll");

app.Run();
