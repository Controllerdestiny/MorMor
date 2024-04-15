namespace MorMor.Music.Music_QQ;

internal struct Br_Type
{
    public string size_type;

    public int size;

    public string type;

    public string suffix;

    public Br_Type(string size_type, int size, string type, string suffix)
    {
        this.size_type = size_type;
        this.size = size;
        this.type = type;
        this.suffix = suffix;
    }
}
