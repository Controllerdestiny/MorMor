using System.ComponentModel;


namespace MomoAPI.Enumeration.ApiType;

public enum RecordType
{
    [Description("mp3")]
    Mp3,

    [Description("amr")]
    Amr,

    [Description("wma")]
    Wma,

    [Description("m4a")]
    M4a,

    [Description("spx")]
    Spx,

    [Description("ogg")]
    Ogg,

    [Description("wav")]
    Wav,

    [Description("Flac")]
    Flac
}