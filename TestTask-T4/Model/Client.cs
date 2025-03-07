﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using TestTask_T4.Data.Extensions;

namespace TestTask_T4.Model
{
    public record Client : IEntityCreatedAt, IEntityModifiedAt
    {
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
