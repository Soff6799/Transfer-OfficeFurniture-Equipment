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

        CreateMap<Contract, ContractModel>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.SignedAt.Date));
        CreateMap<ContractModel, Contract>()
            .ForMember(dest => dest.SignedAt, opt => opt.MapFrom(src => new DateTimeOffset(src.Date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)));
        CreateMap<ContractCreateModel, Contract>()
            .ForMember(dest => dest.SignedAt, opt => opt.MapFrom(src => new DateTimeOffset(src.Date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)));

        CreateMap<Product, ProductModel>()
    .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));
        CreateMap<ProductCreateModel, Product>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));
        CreateMap<ProductModel, Product>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));

        CreateMap<Supplier, SupplierModel>().ReverseMap();
        CreateMap<SupplierCreateModel, Supplier>();

        CreateMap<TransferAct, TransferActModel>().ReverseMap();
        CreateMap<TransferActCreateModel, TransferAct>();

        CreateMap<TransferActItem, TransferActItemModel>().ReverseMap();
        CreateMap<TransferActItemCreateModel, TransferActItem>();
    }
}
