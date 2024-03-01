namespace MomoAPI.Attitudes;

public class MessageMatch : Attribute
{
    public string MsgType { get; set; }

    public Type ObjType { get; set; }

    public MessageMatch(string type, Type objType)
    {
        MsgType = type;
        ObjType = objType;
    }
}
