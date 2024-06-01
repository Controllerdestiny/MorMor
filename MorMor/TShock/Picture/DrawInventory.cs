using MorMor.Model.Socket.Internet;
using MorMor.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace MorMor.TShock.Picture;

internal class DrawInventory
{
    public static MemoryStream Start(PlayerData data, string name, string server)
    {
        var rand = new Random();
        var id = rand.Next(1, 30);
        using Image image = Image.Load((byte[])Properties.Resources.ResourceManager.GetObject($"bg{id}")!);
        using Image slot = Image.Load((byte[])Properties.Resources.ResourceManager.GetObject("Slot")!);
        var obj = new
        {
            //背包卡槽位置
            inventoryX = 150,
            inventoryY = 300,
            //卡槽间隔
            slotinterval = 10,
            //卡槽大小
            slotSize = 100,
            //背包卡槽与弹药卡槽间隔
            inventoryAmmo = 50,
            //装备栏卡槽与背包卡槽X间隔
            interval = 200
        };
        ImageUtils.DrawText(image, $"{server} {name} 的背包", image.Width / 3 + 200, 100, 150, Color.Wheat); ;
        ImageUtils.DrawSlot(image, slot, data.trashItem, obj.inventoryX + obj.slotSize * 9 + obj.slotinterval * 9, obj.inventoryY + 5 * obj.slotinterval + 5 * obj.slotSize, obj.slotSize, darwCount: 1);
        //绘制背包
        ImageUtils.DrawSlot(image, slot, data.inventory, obj.inventoryX, obj.inventoryY, obj.slotSize);
        ImageUtils.DrawText(image, "背包物品", (obj.slotSize * 10 + obj.slotinterval * 9) / 2, obj.inventoryY - obj.slotSize - 50, 100, Color.Wheat);
        ImageUtils.DrawSlot(image, slot, data.inventory.Skip(50).ToArray(), obj.inventoryX + obj.slotSize * 10 + obj.slotinterval * 9 + obj.inventoryAmmo, obj.inventoryY, darwCount: 8, maxLineCount: 4, erect: true);
        ImageUtils.DrawText(image, "猪猪储钱罐", image.Width - obj.inventoryX - obj.slotSize * 10 - obj.slotinterval * 9 + (obj.inventoryX + obj.slotSize * 10 + obj.slotinterval * 9) / 4, obj.inventoryY - obj.slotSize - 50, 100, Color.Wheat);
        ImageUtils.DrawSlot(image, slot, data.Piggiy, image.Width - obj.inventoryX - obj.slotSize * 10 - obj.slotinterval * 9, obj.inventoryY, darwCount: 40);
        ImageUtils.DrawText(image, "保险箱", image.Width - obj.inventoryX - obj.slotSize * 10 - obj.slotinterval * 9 + (obj.inventoryX + obj.slotSize * 10 + obj.slotinterval * 9) / 4, obj.inventoryY + 4 * obj.slotSize + 4 * obj.slotinterval + obj.interval - obj.slotSize - 50, 100, Color.Wheat);
        ImageUtils.DrawSlot(image, slot, data.safe, image.Width - obj.inventoryX - obj.slotSize * 10 - obj.slotinterval * 9, obj.inventoryY + 4 * obj.slotSize + 4 * obj.slotinterval + obj.interval, darwCount: 40);
        ImageUtils.DrawText(image, "虚空宝库", image.Width - obj.inventoryX - obj.slotSize * 10 - obj.slotinterval * 9 + (obj.inventoryX + obj.slotSize * 10 + obj.slotinterval * 9) / 4, obj.inventoryY + 2 * (4 * obj.slotSize + 4 * obj.slotinterval + obj.interval) - obj.slotSize - 50, 100, Color.Wheat);
        ImageUtils.DrawSlot(image, slot, data.VoidVault, image.Width - obj.inventoryX - obj.slotSize * 10 - obj.slotinterval * 9, obj.inventoryY + 2 * (4 * obj.slotSize + 4 * obj.slotinterval + obj.interval), darwCount: 40);
        ImageUtils.DrawText(image, "护卫熔炉", obj.inventoryX + obj.slotSize * 12 + obj.slotinterval * 11 + obj.inventoryAmmo + 90 + 400, obj.inventoryY + 2 * (4 * obj.slotSize + 4 * obj.slotinterval + obj.interval) - obj.slotSize - 50, 100, Color.Wheat);
        ImageUtils.DrawSlot(image, slot, data.Forge, obj.inventoryX + obj.slotSize * 12 + obj.slotinterval * 11 + obj.inventoryAmmo + 90, obj.inventoryY + 2 * (4 * obj.slotSize + 4 * obj.slotinterval + obj.interval), darwCount: 40);
        int[] n = [0, 2, 4];
        for (int i = 0; i < 3; i++)
        {
            ImageUtils.DrawText(image, $"套装{i + 1}", obj.inventoryX + i * (obj.slotSize + obj.slotinterval) + n[i] * obj.interval + 80, 900, 80, Color.Wheat);
            ImageUtils.DrawSlot(image, slot, data.miscEquip.Concat(data.miscDye).ToArray(), obj.inventoryX + i * (obj.slotSize + obj.slotinterval) + n[i] * obj.interval, 1000, darwCount: 10, maxLineCount: 10, erect: true);
            ImageUtils.DrawSlot(image, slot, data.Loadout[i].armor.Skip(10).ToArray(), obj.inventoryX + (i + 1) * (obj.slotSize + obj.slotinterval) + n[i] * obj.interval, 1000, darwCount: 10, maxLineCount: 10, erect: true);
            ImageUtils.DrawSlot(image, slot, data.Loadout[i].armor, obj.inventoryX + (i + 2) * (obj.slotSize + obj.slotinterval) + n[i] * obj.interval, 1000, darwCount: 10, maxLineCount: 10, erect: true);
        }
        //ImageUtils.ResetSize(image, 1500);
        MemoryStream ms = new();
        image.SaveAsync(ms, new JpegEncoder());
        return ms;
    }
}



