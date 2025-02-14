namespace TestTask_T4.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Title { get; private set; }

        public NotFoundException(string title, string message) : base(message)
        {
            Title = title;
        }
    }
}
