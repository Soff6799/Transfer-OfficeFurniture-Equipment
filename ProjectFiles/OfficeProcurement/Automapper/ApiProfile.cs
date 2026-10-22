using AutoMapper;
using OfficeProcurement.Models.Client;
using OfficeProcurement.Models.Contract;
using OfficeProcurement.Models.Product;
using OfficeProcurement.Models.Supplier;
using OfficeProcurement.Models.TransferAct;
using OfficeProcurement.Models.TransferActItem;
using OfficeProcurement.Services.Contracts.Models;


namespace OfficeProcurement.Automapper
{
    /// <summary>
    /// Профиль маппинга между API-моделями и сервисными моделями
    /// </summary>
    public class ApiProfile : Profile
    {
        /// <summary>
        /// Инициализирует профиль маппинга и настраивает соответствия
        /// </summary>
        public ApiProfile()
        {
            CreateMap<ClientModel, ClientApiModel>();
            CreateMap<ClientCreateApiModel, ClientCreateModel>();
            CreateMap<ClientCreateApiModel, ClientModel>();

            CreateMap<SupplierModel, SupplierApiModel>();
            CreateMap<SupplierCreateApiModel, SupplierCreateModel>();
            CreateMap<SupplierCreateApiModel, SupplierModel>();

            CreateMap<ContractModel, ContractApiModel>();
            CreateMap<ContractCreateApiModel, ContractCreateModel>();
            CreateMap<ContractCreateApiModel, ContractModel>();

            CreateMap<ProductModel, ProductApiModel>();
            CreateMap<ProductCreateApiModel, ProductCreateModel>();
            CreateMap<ProductCreateModel, ProductModel>();

            CreateMap<TransferActModel, TransferActApiModel>();
            CreateMap<TransferActCreateApiModel, TransferActCreateModel>();
            CreateMap<TransferActCreateApiModel, TransferActModel>();

            CreateMap<TransferActItemModel, TransferActItemApiModel>();
            CreateMap<TransferActItemCreateApiModel, TransferActItemCreateModel>();
            CreateMap<TransferActItemCreateApiModel, TransferActItemModel>();
        }

    }
}
