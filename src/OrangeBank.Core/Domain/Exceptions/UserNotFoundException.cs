namespace OrangeBank.Core.Domain.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message)
            : base(message)
        {
        }

    }
}

