using System.Web;

namespace ImageFinder.Service
{
    internal static class StringExtensions
    {
        internal static string ConvertHtmlToXml(this string html, string customRootElementName = null)
        {
            // Remove all HTML Entities
            html = HttpUtility.HtmlDecode(html);

            // Handling XML escape characters
            html = html.Replace("&", "&amp;");

            return string.IsNullOrWhiteSpace(customRootElementName) ? html : $"<{customRootElementName}>{html}</{customRootElementName}>";
        }
    }
}
