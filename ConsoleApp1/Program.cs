using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Approach - 1: HtmlToPdf with Selenium WebDriver.");
            //Thread.Sleep(100000);

            var url = $"file:///{Path.Combine(Directory.GetCurrentDirectory(), "invoice.html")}";

            var driverOptions = new ChromeOptions();
            // In headless mode, PDF writing is enabled by default (tested with driver major version 85)
            driverOptions.AddArgument("headless");
            driverOptions.AddArgument("no-sandbox");
            driverOptions.AddArgument("disable-dev-shm-usage");

            using var driver = new ChromeDriver(driverOptions);
            driver.Navigate().GoToUrl(url);

            // Output a PDF of the first page in A4 size at 90% scale
            //https://chromedevtools.github.io/devtools-protocol/tot/Page/#method-printToPDF
            var printOptions = new Dictionary<string, object>
            {
                { "paperWidth", 210 / 25.4 },
                { "paperHeight", 297 / 25.4 },
                { "scale", 1.0 },
                { "marginTop", 0.6 },
                { "marginBottom", 0.6},
                { "marginLeft", 1.1},
                { "marginRight", 1.1},
                { "printBackground", true},
                { "displayHeaderFooter", true},
                { "headerTemplate", "<span class=title>pageNumber </span> " }
            };
            var printOutput = driver.ExecuteChromeCommandWithResult("Page.printToPDF", printOptions) as Dictionary<string, object>;
            var pdf = Convert.FromBase64String(printOutput["data"] as string);
            string pathDir = Path.Combine(Directory.GetCurrentDirectory(), "pdf");
            string filePath = Path.Combine(pathDir, $"SeleniumWebDriver_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf");
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }
            File.WriteAllBytes(filePath, pdf);
            Console.WriteLine("filePath " + filePath);
        }
        
    }
}
