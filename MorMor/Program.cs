namespace MorMor;
class Program
{
    public static async Task Main()
    {
        await MorMorAPI.Star();
        Console.Read();
    }
}