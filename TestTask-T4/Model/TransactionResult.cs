﻿namespace TestTask_T4.Model
{
    public record TransactionResult
    {
        public DateTime InsertDateTime { get; init; }
        public decimal ClientBalance { get; init; }
    }
}
