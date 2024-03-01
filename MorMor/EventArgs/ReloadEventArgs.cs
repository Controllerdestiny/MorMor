using MomoAPI.Entities;
using MomoAPI.Entities.Segment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.EventArgs;

public class ReloadEventArgs : System.EventArgs
{
    public MessageBody Message { get; }

    public ReloadEventArgs()
    {
        Message = new();
    }
}
