using Markdig;
using ProtoBuf.Meta;
using PuppeteerSharp;
using System.Diagnostics;
namespace MorMor.Utils;

public class Utility
{
    private static IBrowser? browser = null;

    private static IPage? Page = null;

    private static IElementHandle? App;

    public static T ReadProtobufItem<T>(MemoryStream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        var mode = RuntimeTypeModel.Create();
        mode.Add(typeof(T), true);
        return mode.Deserialize<T>(stream);
    }

    private static Dictionary<string, string> ReplaceDic = new()
    {
        { "\n", "\\n" },
        { "\r\n", "\\n" },
        { "\r", "\\n" },
        { "'", "\\'" }
    };



    public static void KillChrome()
    {
        foreach (var process in Process.GetProcesses())
        {
            if (process.ProcessName.Contains("chrome"))
            {
                process.Kill();
            }
        }
    }
    public static async Task<Stream> Markdown(string md)
    {
        if (browser == null || !browser.IsConnected || browser.IsClosed || browser.Process.HasExited)
        {
            await new BrowserFetcher().DownloadAsync();
            browser = await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = true,
            });
        }
        if (Page == null || Page.IsClosed || Page.Browser.Process.HasExited)
        {
            Page = await browser.NewPageAsync();
            await Page.GoToAsync($"http://docs.oiapi.net/view.php?theme=light", 5000).ConfigureAwait(false);
            App = await Page.QuerySelectorAsync("body").ConfigureAwait(false);
        }
        await Page.WaitForNetworkIdleAsync(new()
        {
            Timeout = 5000
        });
        var guid = Guid.NewGuid().ToString();
        var option = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseAlertBlocks()
            .UsePipeTables()
            .UseEmphasisExtras()
            .UseListExtras()
            .UseSoftlineBreakAsHardlineBreak()
            .UseFootnotes()
            .UseFooters()
            .UseCitations()
            .UseGenericAttributes()
            .UseGridTables()
            .UseAbbreviations()
            .UseEmojiAndSmiley()
            .UseDefinitionLists()
            .UseCustomContainers()
            .UseFigures()
            .UseMathematics()
            .UseBootstrap()
            .UseMediaLinks()
            .UseSmartyPants()
            .UseAutoIdentifiers()
            .UseTaskLists()
            .UseDiagrams()
            .UseYamlFrontMatter()
            .UseNonAsciiNoEscape()
            .UseAutoLinks()
            .UseGlobalization()
            .Build();
        var postData = Markdig.Markdown.ToHtml(md, option);
        foreach (var (oldChar, newChar) in ReplaceDic)
        {
            postData = postData.Replace(oldChar, newChar);
        }

        await Page.EvaluateExpressionAsync($"document.querySelector('#app').innerHTML = '{postData.Trim()}'");
        await App!.EvaluateFunctionAsync("element => element.style.width = 'fit-content'");
        var clip = await App!.BoundingBoxAsync().ConfigureAwait(false);
        var ret = await Page.ScreenshotStreamAsync(new()
        {
            Clip = new()
            {
                Width = clip!.Width,
                Height = clip.Height,
                X = clip.X,
                Y = clip.Y
            },
            Type = ScreenshotType.Png,
        });
        return ret;
    }
}
