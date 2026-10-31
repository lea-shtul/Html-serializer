using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }

        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            HtmlElement root;
            while (queue.Count > 0)
            {
                root = queue.Dequeue();
                yield return root;
                foreach (HtmlElement ch in root.Children)
                {
                    queue.Enqueue(ch);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement cur = this.Parent;
            while (cur != null)
            {
                yield return cur;
                cur = cur.Parent;
            }
        }
    }
}
