using AutoMapper;
using OfficeProcurement.Entities;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Automapper;

public class ServiceProfile : Profile
{
    /// <summary>
    /// Инициализирует профиль маппинга и настраивает соответствия
    /// между сущностями и моделями сервисов
    /// </summary>
    public ServiceProfile()
    {
        CreateMap<Client, ClientModel>().ReverseMap();
        CreateMap<ClientCreateModel, Client>();

        CreateMap<Contract, ContractModel>().ReverseMap();
        CreateMap<ContractCreateModel, Contract>();

        CreateMap<Product, ProductModel>().ReverseMap();
        CreateMap<ProductCreateModel, Product>();

        CreateMap<Supplier, SupplierModel>().ReverseMap();
        CreateMap<SupplierCreateModel, Supplier>();

        CreateMap<TransferAct, TransferActModel>().ReverseMap();
        CreateMap<TransferActCreateModel, TransferAct>();

        CreateMap<TransferActItem, TransferActItemModel>().ReverseMap();
        CreateMap<TransferActItemCreateModel, TransferActItem>();
    }
}
