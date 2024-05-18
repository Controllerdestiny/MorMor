using MorMor.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MorMor.Terraria.Picture;

public class ProgressImage
{
    public static MemoryStream Start(Dictionary<string, bool> parameters, string serverName)
    {
        var rand = new Random();
        var id = rand.Next(1, 30);
        using Image image = Image.Load((byte[])Properties.Resources.ResourceManager.GetObject($"bg{id}")!);
        image.Mutate(x => x.Resize(4000, 3500));
        using Image slot = Image.Load((byte[])Properties.Resources.ResourceManager.GetObject("Slot")!);
        ImageUtils.DrawProgresst(image, slot, parameters, 500, 400, maxLineCount: 7, darwCount: 28);
        ImageUtils.DrawText(image, $"{serverName}进度", image.Width / 2 - 300, 100, 150, Color.White);
        ImageUtils.ResetSize(image, 1500);
        MemoryStream ms = new();
        image.SaveAsync(ms, new JpegEncoder());
        return ms;
    }

}
