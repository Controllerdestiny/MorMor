using MomoAPI.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Log;

public class TextLog : LogWriter
{
    public TextLog(string file) : base(file)
    {
    }
}
