using MomoAPI.Entities;

namespace MorMor.Extensions;

public static class MessageBodyExt
{
    public static MessageBody MarkdownImage(this MessageBody body, string content)
    {
        try
        {
            var stream = Utils.Utility.Markdown(content).Result;
            return body.Image(stream);
        }
        catch (Exception ex)
        {
            return body.Text(ex.Message);
        }
    }
}
