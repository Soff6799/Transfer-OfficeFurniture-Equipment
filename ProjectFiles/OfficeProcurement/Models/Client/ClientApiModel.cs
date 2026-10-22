namespace OfficeProcurement.Models.Client
{
    /// <summary>
    /// Модель клиента для API
    /// </summary>
    public class ClientApiModel : ClientCreateApiModel
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public Guid Id { get; set; }
    }
}
