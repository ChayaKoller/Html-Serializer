
using Html_Serializer;
using System.Text.RegularExpressions;

// Load HTML from a website
string html = await Load("https://www.w3.org/");


// Split HTML into parts
var htmlLines = Regex.Split(html, "<(.*?)>")
    .Where(s => !string.IsNullOrWhiteSpace(s));

// Build HTML tree
HtmlSerializer parser = new HtmlSerializer();
HtmlElement root = parser.Parse(htmlLines);

// Search elements using CSS-like selector
Selector selector = Selector.Parse("div");
HashSet<HtmlElement> results = root.Find(selector);

// Display results
Console.WriteLine($"Found {results.Count} matching elements:");

foreach (HtmlElement element in results)
{
    Console.WriteLine(
        $"Tag: {element.Name}, " +
        $"Id: {element.Id}, " +
        $"Classes: {string.Join(", ", element.Classes)}");
}

// Demonstrate tree traversal
Console.WriteLine(
    $"Total elements in tree: {root.Descendants().Count()}");
static async Task<string> Load(string url)
{
    using HttpClient client = new HttpClient();

    using HttpResponseMessage response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    return await response.Content.ReadAsStringAsync();
}