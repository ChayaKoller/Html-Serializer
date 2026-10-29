using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    public class HtmlElement

    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InnerHtml { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<String> Classes { get; set; } = new List<String>();

        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement h = queue.Dequeue();

                yield return h;

                for (int i = 0; i < h.Children.Count; i++)
                {
                    queue.Enqueue(h.Children[i]);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = this.Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }

        }
        public bool Matches(Selector selector)
        {
            if (selector.TagName != null && selector.TagName != this.Name)
            {
                return false;
            }
            if (selector.Id != null && selector.Id != this.Id)
            {
                return false;
            }
            if (!selector.Classes.All(c => Classes.Contains(c)))
            {
                return false;
            }
            return true;
        }
        public HashSet<HtmlElement> Find(Selector selector)
        {
            HashSet<HtmlElement> results = new();
            foreach (HtmlElement h in this.Descendants())
            {
                if (h.Matches(selector))
                {
                    if (selector.Child != null)
                        foreach (HtmlElement child in h.Children)
                        {
                            results.UnionWith(child.Find(selector.Child));
                        }
                    else
                    {
                        results.Add(h);

                    }
                }

            }

            return results;
        }
    }


}

