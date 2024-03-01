using MomoAPI.Entities;
using MomoAPI.EventArgs;
using System.Collections;
using System.Text;

namespace MorMor.Utils;

public static class PaginationTools
{
    public delegate Tuple<string> LineFormatterDelegate(object lineData, int lineIndex, int pageNumber);

    #region [Nested: Settings Class]
    public class Settings
    {
        public bool IncludeHeader { get; set; }

        private string headerFormat;
        public string HeaderFormat
        {
            get { return headerFormat; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                headerFormat = value;
            }
        }

        public bool IncludeFooter { get; set; }

        private string footerFormat;
        public string FooterFormat
        {
            get { return footerFormat; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                footerFormat = value;
            }
        }

        public string NothingToDisplayString { get; set; }
        public LineFormatterDelegate LineFormatter { get; set; }

        private int maxLinesPerPage;

        public int MaxLinesPerPage
        {
            get { return maxLinesPerPage; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("该值必须大于0!");

                maxLinesPerPage = value;
            }
        }

        private int pageLimit;

        public int PageLimit
        {
            get { return pageLimit; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("该值必须大于0!");

                pageLimit = value;
            }
        }


        public Settings()
        {
            IncludeHeader = true;
            headerFormat = "第 {{0}} 页，共 {{1}} 页";
            IncludeFooter = true;
            footerFormat = "输入 /<指令> {{0}} 查看更多";
            NothingToDisplayString = null;
            LineFormatter = null;
            maxLinesPerPage = 4;
            pageLimit = 0;
        }
    }
    #endregion

    public static void SendPage(
      GroupMessageEventArgs args, int pageNumber, IEnumerable dataToPaginate, int dataToPaginateCount, Settings settings = null)
    {
        settings ??= new Settings();

        if (dataToPaginateCount == 0)
        {
            if (settings.NothingToDisplayString != null)
            {
                args.Group.Reply(settings.NothingToDisplayString);
            }
            return;
        }

        int pageCount = ((dataToPaginateCount - 1) / settings.MaxLinesPerPage) + 1;
        if (settings.PageLimit > 0 && pageCount > settings.PageLimit)
            pageCount = settings.PageLimit;
        if (pageNumber > pageCount)
            pageNumber = pageCount;
        MessageBody body = new();
        if (settings.IncludeHeader)
        {
            body.Add(string.Format(settings.HeaderFormat, pageNumber, pageCount));
        }

        int listOffset = (pageNumber - 1) * settings.MaxLinesPerPage;
        int offsetCounter = 0;
        int lineCounter = 0;
        foreach (object lineData in dataToPaginate)
        {
            if (lineData == null)
                continue;
            if (offsetCounter++ < listOffset)
                continue;
            if (lineCounter++ == settings.MaxLinesPerPage)
                break;

            string lineMessage;
            if (lineData is Tuple<string>)
            {
                var lineFormat = (Tuple<string>)lineData;
                lineMessage = lineFormat.Item1;
            }
            else if (settings.LineFormatter != null)
            {
                try
                {
                    Tuple<string> lineFormat = settings.LineFormatter(lineData, offsetCounter, pageNumber);
                    if (lineFormat == null)
                        continue;

                    lineMessage = lineFormat.Item1;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("LineFormatter 引用的方法引发了异常。有关详细信息，请参阅内部异常。", ex);
                }
            }
            else
            {
                lineMessage = lineData.ToString();
            }

            if (lineMessage != null)
            {
                body.Add("\n" + lineMessage);
            }
        }

        if (lineCounter == 0)
        {
            if (settings.NothingToDisplayString != null)
            {
                args.Group.Reply(settings.NothingToDisplayString);
            }
        }
        else if (settings.IncludeFooter && pageNumber + 1 <= pageCount)
        {
            body.Add("\n" + string.Format(settings.FooterFormat, pageNumber + 1, pageNumber, pageCount));

        }
        args.Group.Reply(body);
    }

    public static void SendPage(GroupMessageEventArgs args, int pageNumber, IList dataToPaginate, Settings settings = null)
    {
        PaginationTools.SendPage(args, pageNumber, dataToPaginate, dataToPaginate.Count, settings);
    }

    public static List<string> BuildLinesFromTerms(IEnumerable terms, Func<object, string> termFormatter = null, string separator = ", ", int maxCharsPerLine = 80)
    {
        List<string> lines = new();
        StringBuilder lineBuilder = new();

        foreach (object term in terms)
        {
            if (term == null && termFormatter == null)
                continue;

            string termString;
            if (termFormatter != null)
            {
                try
                {
                    if ((termString = termFormatter(term)) == null)
                        continue;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("术语格式化程序表示的方法引发了异常。有关详细信息，请参阅内部异常。", ex);
                }
            }
            else
            {
                termString = term.ToString();
            }

            if (lineBuilder.Length + termString.Length + separator.Length < maxCharsPerLine)
            {
                lineBuilder.Append(termString).Append(separator);
            }
            else
            {
                lines.Add(lineBuilder.ToString());
                lineBuilder.Clear().Append(termString).Append(separator);
            }
        }

        if (lineBuilder.Length > 0)
        {
            lines.Add(lineBuilder.ToString().Substring(0, lineBuilder.Length - separator.Length));
        }
        return lines;
    }

    public static bool TryParsePageNumber(List<string> commandParameters, int expectedParameterIndex, GroupMessageEventArgs errorMessageReceiver, out int pageNumber)
    {
        pageNumber = 1;
        if (commandParameters.Count <= expectedParameterIndex)
            return true;

        string pageNumberRaw = commandParameters[expectedParameterIndex];
        if (!int.TryParse(pageNumberRaw, out pageNumber) || pageNumber < 1)
        {
            if (errorMessageReceiver != null)
                errorMessageReceiver.Reply(string.Format("“{0}”不是有效的页码。", pageNumberRaw));

            pageNumber = 1;
            return false;
        }

        return true;
    }
}
