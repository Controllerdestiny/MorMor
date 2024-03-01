using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Exceptions;

public class CommandBanException : Exception
{
    public CommandBanException(string messge) : base(messge)
    {
        
    }
}
