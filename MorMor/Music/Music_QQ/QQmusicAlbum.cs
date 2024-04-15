using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Music.Music_QQ;

public class QQmusicAlbum
{

    public string Media_mid { get; set; }

    public string Mid { get; set; }

    public int Type { get; set; }

    public string Name { get; set; }

    public IEnumerable<string> Singers { get; set; }

    public QQmusicAlbum(string mid, string media_mid,int type, string name, IEnumerable<string> singers )
    {
        Mid = mid;
        Media_mid = media_mid;
        Type = type;
        Name = name;
        Singers = singers;
    }
}
