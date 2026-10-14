
using OfficeProcurement.Infrastructure.Common;

namespace OfficeProcurement.Infrastructure.Providers
{
    /// <summary>
    /// Поставщик информации о текущем пользователе
    /// </summary>
    public class IdentityProvider : IIdentityProvider
    {
        /// <summary>
        /// Имя текущего пользователя
        /// </summary>
        public string Name => "Admin";
    }
}
