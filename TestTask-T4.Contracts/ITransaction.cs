using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_T4.Contracts
{
    public interface ITransaction
    {
        Guid Id { get; }
        Guid ClientId { get; }
        DateTime DateTime { get; }
        decimal Amount { get; }
    }
}
