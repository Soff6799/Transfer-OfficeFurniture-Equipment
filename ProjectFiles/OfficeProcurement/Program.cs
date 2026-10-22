using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Common;
using OfficeProcurement.Context;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Implementations.ValidateService;
using OfficeProcurement.Repositories;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services;
using OfficeProcurement.Services.Automapper;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Validators.Client;
using FluentValidation;
using OfficeProcurement.Implementations;
using OfficeProcurement.Services.Validators;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ServiceProfile>();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OfficeProcurementContext>(opts =>
    opts.UseNpgsql(connectionString));

builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<OfficeProcurementContext>());

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

builder.Services.AddValidatorsFromAssemblyContaining<ClientCreateModelValidator>();
builder.Services.AddScoped<IValidateService, ValidateService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


