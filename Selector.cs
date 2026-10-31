using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();

        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector ConvertQueryToSelector(string query)
        {
            var pieces = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Selector root = new Selector();
            Selector current = root;

            foreach (var piece in pieces)
            {
                var parts = Regex.Split(piece, "(?=[#.])").Where(p => p.Length > 0).ToArray();
                foreach (var part in parts)
                {
                    if (part[0] == '#')
                        current.Id = part[1..];
                    else if (part[0] == '.')
                        current.Classes.Add(part[1..]);
                    else
                    {
                        if (HtmlHelper.Instance.HtmlTags.Contains(part))
                            current.TagName = part;
                    }
                }

                Selector children = new Selector();
                children.Parent = current;
                current.Child = children;
                current = children;
            }

            // ניתוק הכרטיס הריק שנוצר אחרי החתיכה האחרונה
            if (current.Parent != null)
            {
                current = current.Parent;
                current.Child = null;
            }

            return root;
        }
    }
}
