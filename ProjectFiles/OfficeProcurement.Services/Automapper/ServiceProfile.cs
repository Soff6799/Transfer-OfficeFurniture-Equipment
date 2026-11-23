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
        CreateMap<Client, ClientModel>();
        CreateMap<ClientModel, Client>(MemberList.Source);
        CreateMap<ClientCreateModel, Client>(MemberList.Source);

        CreateMap<Contract, ContractModel>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.SignedAt.DateTime)))
            .ForMember(dest => dest.Sum, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore());
        CreateMap<ContractModel, Contract>(MemberList.Source)
            .ForMember(dest => dest.SignedAt, opt => opt.MapFrom(src => new DateTimeOffset(src.Date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)))
            .ForSourceMember(src => src.Date, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Sum, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Description, opt => opt.DoNotValidate());
        CreateMap<ContractCreateModel, Contract>(MemberList.Source)
            .ForMember(dest => dest.SignedAt, opt => opt.MapFrom(src => new DateTimeOffset(src.Date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)))
            .ForSourceMember(src => src.Date, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Sum, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Description, opt => opt.DoNotValidate());

        CreateMap<Product, ProductModel>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));
        CreateMap<ProductCreateModel, Product>(MemberList.Source)
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
            .ForSourceMember(src => src.UnitPrice, opt => opt.DoNotValidate());
        CreateMap<ProductModel, Product>(MemberList.Source)
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
            .ForSourceMember(src => src.UnitPrice, opt => opt.DoNotValidate());

        CreateMap<Supplier, SupplierModel>()
            .ForMember(dest => dest.SupplierId, opt => opt.Ignore());
        CreateMap<SupplierModel, Supplier>(MemberList.Source)
            .ForSourceMember(src => src.SupplierId, opt => opt.DoNotValidate());
        CreateMap<SupplierCreateModel, Supplier>(MemberList.Source)
            .ForSourceMember(src => src.SupplierId, opt => opt.DoNotValidate());

        CreateMap<TransferAct, TransferActModel>();
        CreateMap<TransferActModel, TransferAct>(MemberList.Source);
        CreateMap<TransferActCreateModel, TransferAct>(MemberList.Source);

        CreateMap<TransferActItem, TransferActItemModel>();
        CreateMap<TransferActItemModel, TransferActItem>(MemberList.Source);
        CreateMap<TransferActItemCreateModel, TransferActItem>(MemberList.Source);
    }
}
