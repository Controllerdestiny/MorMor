using MomoAPI.Log;

namespace MorMor.Log;

public class TextLog : LogWriter
{
    public TextLog(string file) : base(file)
    {
    }
}
