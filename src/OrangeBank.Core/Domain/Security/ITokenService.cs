using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBank.Core.Domain.Security
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
