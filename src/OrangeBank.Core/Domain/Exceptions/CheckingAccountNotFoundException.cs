namespace OrangeBank.Core.Domain.Exceptions
{
    public class CheckingAccountNotFoundException : NotFoundException
    {
        public CheckingAccountNotFoundException(string message)
            : base(message)
        {
        }

    }
}

