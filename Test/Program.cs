using Newtonsoft.Json;
using Test;

var result = JsonConvert.SerializeObject(new CTest(),Formatting.Indented);
Console.WriteLine(result);
Console.ReadLine();
class CTest
{
    [JsonConverter(typeof(CommentConvert),"年龄")]
    public int Age { get; set; }

    [JsonConverter(typeof(CommentConvert), "名字")]
    public string Name { get; set; }

    public CTest()
    {
        Age = 1;
        Name = "张三";
    }
}

