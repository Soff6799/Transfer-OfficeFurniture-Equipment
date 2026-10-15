using OfficeProcurement.Services.Automapper;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

using OfficeProcurement.Services;
using OfficeProcurement.Services.Contracts.Interface;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ServiceProfile>();
});

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ITransferActItemService, TransferActItemService>();
builder.Services.AddScoped<ITransferActService, TransferActService>();


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


