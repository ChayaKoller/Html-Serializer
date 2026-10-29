using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new();

        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector ParseSingle(string text)
        {
            Selector selector = new Selector();

            string[] parts = Regex.Split(text, @"(?=[#\.])");

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                    continue;

                if (part[0] == '#')
                {
                    string t = part.Substring(1);
                    selector.Id = t;
                }
                else if (part[0] == '.')
                {
                    string t = part.Substring(1);
                    selector.Classes.Add(t);
                }
                else
                {
                    selector.TagName = part;
                }
            }

            return selector;
        }

        public static Selector Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("הסלקטור לא יכול להיות ריק", nameof(text));

            string[] parts = text.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            Selector? root = null;
            Selector? current = null;

            foreach (string part in parts)
            {
                Selector selector = ParseSingle(part);

                if (root == null)
                {
                    root = selector;
                    current = selector;
                }
                else
                {
                    current!.Child = selector;
                    selector.Parent = current;
                    current = selector;
                }
            }

            return root!;
        }

    }
}
