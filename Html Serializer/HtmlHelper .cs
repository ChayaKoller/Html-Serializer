using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Html_Serializer
{
    public class HtmlHelper
    {
        public string[]  HtmlTags{ get; set; }
        public string[] NotCloseHtmlTags { get; set; }
        private readonly static HtmlHelper _instance = new HtmlHelper();
        static public HtmlHelper Instance => _instance;
        private HtmlHelper()
        {
            string json = File.ReadAllText("Data/HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(json)!;
            string json1 = File.ReadAllText("Data/HtmlVoidTags.json");
            NotCloseHtmlTags = JsonSerializer.Deserialize<string[]>(json1)!;
        }
    }
}
