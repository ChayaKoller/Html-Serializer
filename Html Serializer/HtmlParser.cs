using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlParser
    {
        public HtmlElement Parse(IEnumerable<string> htmlLines)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement current = root;

            foreach (string line in htmlLines)
            {
                string content = line.Trim();

                if (string.IsNullOrEmpty(content))
                    continue;

                int index = content.IndexOfAny(new[] { ' ', '\t', '\r', '\n' });

                string name = index == -1
                    ? content
                    : content.Substring(0, index);

                // תגית סוגרת
                if (name.StartsWith("/"))
                {
                    if (name == "/html")
                        break;

                    if (current.Parent != null)
                        current = current.Parent;

                    continue;
                }

                // האם התגית סוגרת את עצמה?
                bool isSelfClosing = content.EndsWith("/");

                // הסרת / מסוף שם התגית, למשל img/ -> img
                name = name.TrimEnd('/');

                // תגית פותחת
                if (HtmlHelper.Instance.HtmlTags.Contains(name))
                {
                    HtmlElement htmlElement1 = new HtmlElement();

                    htmlElement1.Name = name;
                    htmlElement1.Parent = current;

                    current.Children.Add(htmlElement1);

                    var atributs = new Regex(@"([^\s=]+)\s*=\s*""([^""]*)""")
                        .Matches(content);

                    foreach (Match at in atributs)
                    {
                        string key = at.Groups[1].Value;
                        string value = at.Groups[2].Value;

                        htmlElement1.Attributes[key] = value;
                    }

                    // חילוץ מזהה
                    if (htmlElement1.Attributes.TryGetValue("id", out string? id))
                    {
                        htmlElement1.Id = id;
                    }

                    // חילוץ מחלקות
                    if (htmlElement1.Attributes.TryGetValue("class", out string? classes))
                    {
                        string[] res = classes.Split(
                            (char[]?)null,
                            StringSplitOptions.RemoveEmptyEntries
                        );

                        foreach (string s in res)
                        {
                            htmlElement1.Classes.Add(s);
                        }
                    }

                    // מעבר לרכיב החדש רק אם נדרשת תגית סגירה
                    if (!HtmlHelper.Instance.NotCloseHtmlTags.Contains(name)
                        && !isSelfClosing)
                    {
                        current = htmlElement1;
                    }
                }
                else
                {
                    // טקסט פנימי
                    current.InnerHtml += line;
                }
            }

            return root;
        }
    }
}
