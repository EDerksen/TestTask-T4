using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_T4.Contracts
{
    public record DebitTransactionRequest : ITransaction
    {
        public Guid Id { get; init; }

        public Guid ClientId { get; init; }

        public DateTime DateTime { get; init; }

        public decimal Amount { get; init; }
    }
}
