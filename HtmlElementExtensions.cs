using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> FindElements(this HtmlElement element, Selector s)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            FindRecursive(element, s, result);
            return result;
        }

        static void FindRecursive(HtmlElement element, Selector s, HashSet<HtmlElement> res)
        {
            var list = element.Descendants().Skip(1);
            foreach (var child in list)
            {
                if ((child.Name == s.TagName || s.TagName == null)
                    && (s.Classes.All(c => child.Classes.Contains(c)))
                    && (s.Id == null || child.Id == s.Id))
                {
                    if (s.Child == null)
                        res.Add(child);
                    else
                        FindRecursive(child, s.Child, res);
                }
            }
        }
    }
}
