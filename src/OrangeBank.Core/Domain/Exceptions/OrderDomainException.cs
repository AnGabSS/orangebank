namespace OrangeBank.Core.Domain.Exceptions
{
    public class OrderDomainException : DomainException
    {
        public OrderDomainException(string message)
            : base(message)
        {
        }

    }
}

