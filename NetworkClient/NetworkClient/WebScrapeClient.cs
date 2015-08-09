using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetworkClient
{
    public class WebScrapeClient : BaseHttpClient
    {
        public WebScrapeClient(string appName) : base(appName) { }
        public WebScrapeClient(string appName, Dictionary<string, string> headers) : base(appName, headers) { }
        public WebScrapeClient(string appName, KeyValuePair<string, string> header) : base(appName, header) { }
        public WebScrapeClient(string appName, string key, string value) : base(appName, key, value) { }

        public async Task<HtmlNode> GetDocumentNodeFromUri(string uri)
        {
            NetworkConnection.UpdateNetworkInformation();
            if (NetworkConnection.IsConnected && NetworkConnection.IsInternetAvailable)
            {
                HttpResponseMessage httpResponseMessage = await GetAsync(uri);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    return GetHtmlNodeFromContent(content);
                }
                else
                {
                    ShowMessageDialog(httpResponseMessage.ReasonPhrase);
                }
            }
            else
            {
                ShowMessageDialog("Please check your internet connection.");
            }

            return null;
        }

        public HtmlNode GetHtmlNodeFromContent(string content)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionFixNestedTags = true;
            htmlDocument.LoadHtml(content);
            return htmlDocument.DocumentNode;
        }

        public IEnumerable<HtmlNode> GetNodes(HtmlNode parentNode, string type, bool hasAttribute = true, string attributeName = null, string attributeValue = null)
        {
            var nodes = parentNode.Descendants(type).Where((n) => n.HasAttributes == hasAttribute);

            if (!string.IsNullOrEmpty(attributeName))
            {
                if (string.IsNullOrEmpty(attributeValue))
                {
                    nodes = nodes.Where((n) => n.Attributes.Contains(attributeName));
                }
                else
                {
                    nodes = nodes.Where((n) => n.GetAttributeValue(attributeName, string.Empty).Equals(attributeValue));
                }
            }

            return nodes;
        }
    }
}
