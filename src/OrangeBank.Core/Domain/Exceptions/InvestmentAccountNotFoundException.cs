using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBank.Core.Domain.Exceptions
{
    public class InvestmentAccountNotFoundException: NotFoundException
    {
        public InvestmentAccountNotFoundException(string message)
            : base(message)
        {
        }
    }
}
