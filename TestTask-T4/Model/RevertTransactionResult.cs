namespace TestTask_T4.Model
{
    public record RevertTransactionResult
    {
        public DateTime RevertDateTime { get; init; }
        public decimal ClientBalance { get; init; }
    }
}
