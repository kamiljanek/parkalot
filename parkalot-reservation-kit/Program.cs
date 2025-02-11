using Microsoft.Playwright;

namespace parkalot_reservation_kit;

class Program
{
    static async Task Main()
    {
        // var playwright = await Playwright.CreateAsync();
        
        using var playwright = await Playwright.CreateAsync();
        var chromium = playwright.Chromium;
        var browser = await playwright.Chromium.ConnectAsync("ws://localhost:9222/devtools/browser/<id>");
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://www.youtube.com");
        await page.GetByPlaceholder("Szukaj").First.FillAsync("how to start play League of Legends");
        await page.GetByPlaceholder("Szukaj").First.PressAsync("Enter");
        // other actions
        await browser.CloseAsync();

        // var browser = await playwright.Chromium.ConnectOverCDPAsync("http://localhost:9222");

        // var page = browser.Contexts.SelectMany(ctx => ctx.Pages)
            // .FirstOrDefault(p => p.Url.Contains("youtube.com"));

        if (page == null)
        {
            Console.WriteLine("Nie znaleziono otwartej karty z YouTube. Otwórz YouTube w przeglądarce i spróbuj ponownie.");
            return;
        }

        Console.WriteLine("🔎 Wyszukiwanie na YouTube...");

        var locator = page.GetByLabel("search_query");
        var locator1 = page.GetByPlaceholder("Szukaj");
        var locator2 = page.GetByRole(AriaRole.Combobox);
        // Wpisanie frazy w pasek wyszukiwania
        await page.GetByPlaceholder("Szukaj").First.FillAsync("how to start play League of Legends");
        await page.GetByPlaceholder("Szukaj").First.PressAsync("Enter");
        // await page.GetByRole(AriaRole.Combobox).FillAsync("how 3");
        // await page.GetByRole(AriaRole.Combobox).First.FillAsync("how 4");

        // Czekamy, aż wyniki się załadują
        await page.WaitForSelectorAsync("yt-image");

        // Pobieramy drugi film z listy wyników i klikamy w niego
        var videos = await page.QuerySelectorAllAsync("ytd-video-renderer");
        if (videos.Count < 2)
        {
            Console.WriteLine("Nie znaleziono wystarczającej liczby filmów!");
            return;
        }

        Console.WriteLine("▶️ Otwieranie drugiego filmu...");
        var link = await videos[1].QuerySelectorAsync("ytd-thumbnail");
        await link.ClickAsync();

        Console.WriteLine("🎬 Gotowe! Odtwarza się drugi film z listy.");
    }
}