using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRedisService
    {
        BalanceEntity? GetCurrentBalance();
        void SaveCurrentBalanceRedis(BalanceEntity balance);
    }
}
