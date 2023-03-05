using Microsoft.AspNetCore.Mvc;
using ProxyGenerator.Models;
using System.Diagnostics;
using System.Net.Http;

namespace ProxyGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var client = _httpClientFactory.CreateClient();

            string url = "https://www.reddit.com/";
            // Send the request to the original URL
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);

            // Read the response content
            var content = await response.Content.ReadAsStringAsync();

            // Modify the content by adding ™ to each word with six letters
            var words = content.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var modifiedWords = words.Select(w => w.Length == 6 ? w + "™" : w);
            var modifiedContent = string.Join(" ", modifiedWords);

            // Replace all internal links with links to the proxy server
            modifiedContent = modifiedContent.Replace(url, "http://localhost:5000");

            // Return the modified content as the response
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(modifiedContent);
            writer.Flush();
            stream.Position = 0;

            //return Content(modifiedContent);

            return new FileStreamResult(stream, "text/html");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}