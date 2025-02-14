using Riok.Mapperly.Abstractions;
using TestTask_T4.Contracts;
using TestTask_T4.Model;

namespace TestTask_T4.Mappers
{
    [Mapper]
    public static partial class ClientsMapper
    {
        public static partial ClientBalanceResponse ToResponse(this ClientBalance clientBalance);
    }
}
