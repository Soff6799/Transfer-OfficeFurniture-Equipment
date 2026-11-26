using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OfficeProcurement.Automapper;
using OfficeProcurement.Common;
using OfficeProcurement.Context;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Implementations.ValidateService;
using OfficeProcurement.Infrastructure;
using OfficeProcurement.Infrastructure.Common;
using OfficeProcurement.Infrastructure.Providers;
using OfficeProcurement.Repositories;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services;
using OfficeProcurement.Services.Automapper;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;
using OfficeProcurement.Services.Implementations;
using OfficeProcurement.Services.Validators.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IIdentityProvider, IdentityProvider>();

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ServiceProfile>();
    x.AddProfile<ApiProfile>();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OfficeProcurementContext>(opts =>
    opts.UseNpgsql(connectionString));

builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<OfficeProcurementContext>());

builder.Services.AddScoped<IReader>(x => x.GetRequiredService<OfficeProcurementContext>());
builder.Services.AddScoped<IWriter>(x => x.GetRequiredService<OfficeProcurementContext>());
builder.Services.AddScoped<IDbWriterContext, DbWriterContext>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ITransferActRepository, TransferActRepository>();
builder.Services.AddScoped<ITransferActItemRepository, TransferActItemRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ITransferActItemService, TransferActItemService>();
builder.Services.AddScoped<ITransferActService, TransferActService>();

builder.Services.AddScoped<ITransferActExcelService, TransferActExcelService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddValidatorsFromAssembly(typeof(ClientCreateModelValidator).Assembly);
builder.Services.AddScoped<IValidateService, ValidateService>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Office Procurement API",
        Version = "v1",
        Description = "API для управления офисными закупками"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT-токен, полученный из /api/Auth/login"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Office Procurement API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


