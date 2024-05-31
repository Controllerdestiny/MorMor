using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaMap;

public class MapInfo
{
    public string Name { get; set; }

    public byte[] Buffer { get; set; }

    public MapInfo(string name, byte[] buffer)
    {
        Name = name;
        Buffer = buffer;
    }
}
