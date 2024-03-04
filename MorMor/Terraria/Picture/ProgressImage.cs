
using SkiaSharp;

namespace MorMor.Picture;

public class ProgressImage
{
    private const int Weight = 6000;

    private const int Height = 4000;

    public static Stream DrawImg(Dictionary<string, bool> data, string name)
    {
        var Stream = new MemoryStream();
        var Bitmap = new SKBitmap(Weight, Height);
        var p = new SKPaint()
        {
            Shader = SKShader.CreateLinearGradient(new SKPoint(0, 0),
            new SKPoint(Weight, Height), 
            //渐变色
            new SKColor[] 
            { 
                SKColor.Parse("fbd3e9"), 
                SKColor.Parse("eef2f3"), 
                SKColor.Parse("ffdde1")
            },
            SKShaderTileMode.Repeat)
        };
        
        //画框
        var _Bitmap = new SKBitmap(600, 600);

        //画笔1
        var paint = new SKPaint()
        {
            Color = SKColors.Red,
            IsAntialias = true, // 抗锯齿
            Style = SKPaintStyle.Fill,
        };
        //初始化画笔2
        var _paint = new SKPaint()
        {
            Color = SKColors.Goldenrod,
            IsAntialias = true, // 抗锯齿
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 50,
        };

        var canvas = new SKCanvas(Bitmap);
        //背景框画布
        var _canvas = new SKCanvas(_Bitmap);

        //字体对象
        var font = new SKFont()
        {
            Size = 200f,
            Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
        };

        canvas.DrawRect(0, 0, Weight, Height, p);
        p.Dispose();
        canvas.DrawText($"{name}服务器进度", 3000f, 350f, SKTextAlign.Center, font, paint);
        font.Size = 120;
        paint.Color = SKColors.Green;
        canvas.DrawText($"特别提示:浅绿色为已击杀", 3000f, 550f, SKTextAlign.Center, font, paint);
        float x = 550f;
        float y = 700f;
        int i = 0;
        foreach (var item  in data)
        {
            //画背景
            //设置背景颜色绿 代表未击杀
            if (item.Value)
                _canvas.DrawColor(SKColor.Parse("DFFFDF"));
            else
                _canvas.DrawColor(SKColor.Parse("EBD3E8"));
            //绘制BOSS图片到背景
            var bossImage = SKBitmap.Decode((byte[])Properties.Resources.ResourceManager.GetObject(item.Key)!);
            SKBitmap zoom;
            _canvas.DrawRect(0, 0, 600, 600, _paint);
            canvas.DrawBitmap(_Bitmap, x, y);
            if (bossImage.Width > bossImage.Height)
            {
                zoom = Zoom(bossImage, 550, 0);
            }
            else
            {
                zoom = Zoom(bossImage, 0, 550);
            }
            canvas.DrawBitmap(zoom, ((600 - zoom.Width) / 2) + x, ((600 - zoom.Height) / 2) + y);
            if ((i + 1) % 7 == 0)
            {
                y += 800f;
                x = 550f;
            }
            else
            {
                x += 700f;
            }
            i++;
        }
        return SKImage.FromBitmap(Zoom(Bitmap, 1500, 0)).Encode(SKEncodedImageFormat.Jpeg, 100).AsStream();
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
        return bmp.Resize(new SKSizeI(width, height),SKSamplingOptions.Default);
    }


    /// <summary>
    /// 获得某一颜色区间的颜色集合
    /// </summary>
    /// <param name="sourceColor">起始颜色</param>
    /// <param name="destColor">终止颜色</param>
    /// <param name="count">分度数</param>
    /// <returns>返回颜色集合</returns>
    private List<SKColor> GetSingleColorList(SKColor srcColor, SKColor desColor, int count)
    {
        List<SKColor> colorFactorList = new();
        int redSpan = desColor.Red - srcColor.Red;
        int greenSpan = desColor.Green - srcColor.Green;
        int blueSpan = desColor.Blue - srcColor.Blue;
        for (int i = 0; i < count; i++)
        {
            SKColor color = new((byte)(srcColor.Red + (int)((double)i / count * redSpan)),
                (byte)(srcColor.Green + (int)((double)i / count * greenSpan)),
                (byte)(srcColor.Blue + (int)((double)i / count * blueSpan)));

            colorFactorList.Add(color);
        }
        return colorFactorList;
    }

}
