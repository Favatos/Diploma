using Microsoft.AspNetCore.Mvc;
using Shared;

namespace FrontWeb.Controllers;

public class NonogramFrontController : Controller
{
    private IHttpClientFactory factory;

    public NonogramFrontController(IHttpClientFactory factory)
    {
        this.factory = factory;
    }

    [HttpGet("/index")]
    public async Task<IActionResult> Index()
    {
        HttpClient client = factory.CreateClient();
        LevelsDTO levels = await client.GetFromJsonAsync<LevelsDTO>("https://localhost:7199/index") ?? throw new NullReferenceException();
        return View(levels);
    }

    [HttpGet("/play")]
    public async Task<IActionResult> Play(int id)
    {
        HttpClient client = factory.CreateClient();
        NonogramDTO nonogram = await client.GetFromJsonAsync<NonogramDTO>($"https://localhost:7199/play?id={id}") ?? throw new NullReferenceException();
        return View(nonogram);
    }

    [HttpGet("/create")]
    public async Task<IActionResult> Create() {
        HttpClient client = factory.CreateClient();
        CreateDTO create = await client.GetFromJsonAsync<CreateDTO>("https://localhost:7199/create") ?? throw new NullReferenceException();
        return View(create);
    }

    [HttpPost("/create")]  
    public async Task<IActionResult> Create(CreateDTO vm)
    {
        HttpClient client = factory.CreateClient();
        HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7199/create", vm);

        if (!response.IsSuccessStatusCode) throw new Exception("Cannot save nonogram");

        return RedirectToAction(nameof(Index));
    }
}
