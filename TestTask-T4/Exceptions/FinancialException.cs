namespace TestTask_T4.Exceptions
{
    public class FinancialException : Exception
    {
        public string Title { get; private set; }

        public Dictionary<string, object>? Extensions { get; private set; }

        public FinancialException(string title, string message, Dictionary<string, object>? extensions = null) : base(message)
        {
            Title = title;
            Extensions = extensions;
        }
    }
}
