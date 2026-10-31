using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlHelper
    {
        public string[] HtmlTags { get; set; }

        public string[] HtmlVoidTags { get; set; }

        private readonly static HtmlHelper instance = new HtmlHelper();
        public static HtmlHelper Instance => instance;

        private HtmlHelper()
        {
            // קריאת תוכן הקבצים בעזרת מחלקת File
            var allTagsJson = File.ReadAllText("HtmlTags.json");
            var voidTagsJson = File.ReadAllText("HtmlVoidTags.json");

            // המרת ה-JSON למערכים
            HtmlTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidTagsJson);
        }
    }
}
