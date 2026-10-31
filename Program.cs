using Html_Serializer;
using System.Text.RegularExpressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var html = await Load("https://hebrewbooks.org/beis");

var cleanHtml = new Regex("\\s+").Replace(html, " ");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml)
    .Select(s => s.Trim())
    .Where(s => s.Length > 0);


async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


// ---- בניית העץ ----
var root = new HtmlElement { Name = "root" };
var current = root;

foreach (var line in htmlLines)
{
    var parts = line.Split(' ', 2);
    var firstPart = parts[0];

    if (firstPart == "/html")
        break;

    if (firstPart[0] == '/')
    {
        if (current.Parent != null)
            current = current.Parent;
        continue;
    }

    if (HtmlHelper.Instance.HtmlTags.Contains(firstPart))
    {
        HtmlElement child = new HtmlElement()
        {
            Parent = current,
            Name = firstPart
        };

        current.Children.Add(child);

        if (parts.Length > 1)
        {
            var matches = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(parts[1]);
            foreach (Match match in matches)
            {
                var attrName = match.Groups[1].Value;
                var attrValue = match.Groups[2].Value;

                if (attrName == "id")
                {
                    child.Id = attrValue;
                }
                else if (attrName == "class")
                {
                    child.Classes.AddRange(attrValue.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }
                else
                {
                    child.Attributes.Add($"{attrName}=\"{attrValue}\"");
                }
            }
        }

        bool isSelfClosing = line.EndsWith("/")
                         || HtmlHelper.Instance.HtmlVoidTags.Contains(firstPart);

        if (!isSelfClosing)
            current = child;

        continue;
    }

    current.InnerHtml += line;
}


// ---- חיפוש לפי סלקטור ----
var selector = Selector.ConvertQueryToSelector("div");
var result = root.FindElements(selector);

Console.WriteLine($"נמצאו {result.Count} אלמנטים");

foreach (var el in result)
    Console.WriteLine($"{el.Name}  id={el.Id}  classes={string.Join(",", el.Classes)}");
