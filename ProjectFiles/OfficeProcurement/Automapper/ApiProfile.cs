using AutoMapper;
using OfficeProcurement.Models.Client;
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
        }

    }
}
