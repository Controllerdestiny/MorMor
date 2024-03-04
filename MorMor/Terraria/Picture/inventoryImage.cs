using MorMor.Model.Terraria;
using MorMor.Terraria.Server.ApResultArgs;
using SkiaSharp;

namespace MorMor.Picture;

public class InventoryImage
{
    private readonly SKBitmap Bitmap;

    private readonly SKBitmap _Bitmap;

    private readonly SKCanvas _canvas;

    private readonly SKCanvas canvas;

    private readonly SKPaint paint;

    private readonly SKPaint _paint;

    private readonly SKFont Font;

    private readonly int w = 7000;

    private readonly int h = 5000;

    public InventoryImage()
    {
        Bitmap = new SKBitmap(w, h);
        var p = new SKPaint()
        {
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0), new SKPoint(w, h), 
                new SKColor[] 
                { 
                    SKColor.Parse("fbd3e9"), 
                    SKColor.Parse("eef2f3"), 
                    SKColor.Parse("ffdde1")
                }, SKShaderTileMode.Repeat)
        };
        _Bitmap = new SKBitmap(210, 210);
        _canvas = new SKCanvas(_Bitmap);
        canvas = new SKCanvas(Bitmap);
        canvas.DrawRect(0, 0, w, h, p);
        p.Dispose();

        Font = new()
        {
            Size = 40f,
            Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
        };

        paint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true, // 抗锯齿
            Style = SKPaintStyle.Fill,
        };

        _paint = new SKPaint()
        {
            Color = SKColors.Wheat,
            IsAntialias = true, // 抗锯齿
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 20,
        };
        _canvas.DrawColor(SKColors.White);
        _canvas.DrawRect(0, 0, 210, 210, _paint);
    }
    /// <summary>
    /// 画背包
    /// </summary>
    /// <param name="data">物品数据</param>
    /// <param name="SlotCount">画多少个</param>
    /// <param name="MaxCount">每行或没列最大计数</param>
    /// <param name="x">起始x</param>
    /// <param name="y">起始y</param>
    /// <param name="vertical">是否竖列</param>
    /// <returns></returns>
    private void DrawSlotItem(List<ItemInfo> data, int SlotCount, int MaxCount, int x, int y, bool vertical = false)
    {
        int sourceX = x;
        int sourceY = y;
        int interval = 220;
        for (int i = 0; i < SlotCount; i++)
        {
            //画背包格子
            canvas.DrawImage(SKImage.FromBitmap(_Bitmap), x, y);
            var itemID = data[i].NetID;
            var num = data[i].Stack;
            if (itemID > 0)
            {
                var itemImage = SKBitmap.Decode((byte[])Properties.Resources.ResourceManager.GetObject(itemID.ToString())!);
                SKBitmap zoom;
                if (itemImage.Width > itemImage.Height)
                    zoom = Zoom(itemImage, 120, 0);
                else
                    zoom = Zoom(itemImage, 0, 120);
                canvas.DrawBitmap(zoom, ((210 - zoom.Width) / 2) + x, ((210 - zoom.Height) / 2) + y);
                canvas.DrawText(num.ToString(), x + _Bitmap.Width - 70, _Bitmap.Height - 15 + y, SKTextAlign.Center, Font, paint);
            }

            if ((i + 1) % MaxCount == 0)
            {
                if (vertical)
                {
                    x += interval;
                    y = sourceY;
                }
                else
                {
                    y += interval;
                    x = sourceX;
                }
            }
            else
            {
                if (vertical)
                {
                    y += interval;
                }
                else
                {
                    x += interval;
                }
            }
        }
    }

    private List<ItemInfo> GetItems(List<ItemInfo> data, int start)
    {
        var arr = new List<ItemInfo>();
        for (int i = 0; i < data.Count; i++)
        {
            if (i >= start)
            {
                arr.Add(data[i]);
            }
        }
        return arr;
    }
    private void DrawLoadout(LoadoutInfo data, List<ItemInfo> miscEquip, List<ItemInfo> miscDye, int x, int y)
    {
        DrawSlotItem(data.Armors, 3, 3, x, y, vertical: true);
        DrawSlotItem(GetItems(data.Armors, 10), 3, 3, x - (220 * 1), y, true);
        DrawSlotItem(data.Dyes, 3, 3, x - (220 * 2), y, vertical: true);
        DrawSlotItem(GetItems(data.Armors, 3), 7, 7, x - (220 * 3), y, true);
        DrawSlotItem(GetItems(data.Armors, 13), 7, 7, x - (220 * 4), y, true);
        DrawSlotItem(GetItems(data.Dyes, 3), 7, 7, x - (220 * 5), y, true);
        DrawSlotItem(miscEquip, 5, 5, x - (220 * 6), y, vertical: true);
        DrawSlotItem(miscDye, 5, 5, x - (220 * 7), y, vertical: true);
    }

    public Stream DrawImg(PlayerinventoryInfo data, string name, string server)
    {
        //背包
        DrawSlotItem(data.Inventory, 50, 10, 300, 1200);
        //猪猪
        DrawSlotItem(data.Piggiy, 40, 10, 300, 2600);
        //保险箱
        DrawSlotItem(data.Safe, 40, 10, 300, 3800);
        //虚空宝库
        DrawSlotItem(data.VoidVault, 40, 10, 2600, 2600);
        //护卫熔炉
        DrawSlotItem(data.Forge, 40, 10, 2600, 3800);
        //装备1
        DrawLoadout(data.Loadout[0], data.MiscEquip, data.MiscDye, 4500, 780);
        //装备2
        DrawLoadout(data.Loadout[1], data.MiscEquip, data.MiscDye, 6680, 780);
        //装备3
        DrawLoadout(data.Loadout[2], data.MiscEquip, data.MiscDye, 6680, 3000);

        //金币弹药
        DrawSlotItem(GetItems(data.Inventory, 50), 8, 8, 70, 900, true);
        DrawSlotItem(data.TrashItem, 1, 1, 2280, 2300, true);
        Font.Size = 200f;
        canvas.DrawText($"{server}服务器{name}背包", 3500, 300, SKTextAlign.Center, Font, paint);
        Font.Size = 140f;
        canvas.DrawText($"背包物品", 1350, 1100, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"猪猪存钱罐", 1350, 2500, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"保险箱", 1350, 3700, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"虚空宝库", 3650, 2500, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"护卫熔炉", 3650, 3700, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"套装一", 3750, 680, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"套装二", 5930, 680, SKTextAlign.Center, Font, paint);
        canvas.DrawText($"套装三", 5930, 2900, SKTextAlign.Center, Font, paint);
        return SKImage.FromBitmap(Zoom(Bitmap, 2000, 0)).Encode(SKEncodedImageFormat.Jpeg, 100).AsStream();

    }


    /// <summary>
    /// 等比例缩放
    /// </summary>
    /// <param name="bmp"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static SKBitmap Zoom(SKBitmap bmp, int width, int height)
    {
        if (width == 0 && height == 0)
        {
            width = bmp.Width;
            height = bmp.Height;
        }
        else
        {
            if (width == 0)
            {
                width = height * bmp.Width / bmp.Height;
            }
            if (height == 0)
            {
                height = width * bmp.Height / bmp.Width;
            }
        }

        return bmp.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
    }

}
