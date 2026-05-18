using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Text.Json;


namespace Back.Controllers;

[ApiController]
public class NonogramController : Controller
{
    private readonly NonogramDbContext db;

    public NonogramController(NonogramDbContext db)
    {
        this.db = db;
    }

    [HttpGet("/play")]
    public async Task<IActionResult> Play(int id)
    {
        NonogramEntity? entity = await db.Nonograms.FindAsync(id);

        if (entity == null) return NotFound();

        Nonogram nonogram = entity.ConverToNonogram();
        NonogramDTO vm = new()
        {
            SolutionJson = entity.SolutionJson,
            Nonogram = nonogram
        };

        return Ok(vm);
    }

    [HttpGet("/index")]
    public async Task<IActionResult> Index()
    {
        List<NonogramEntity> entities = await db.Nonograms.ToListAsync();
        if (!entities.Any()) return NotFound("Nonograms not found");

        List<Nonogram> nonograms = entities.Select(e => e.ConverToNonogram()).ToList();

        LevelGroup easy = new()
        {
            Title = "Easy",
            Levels = [.. nonograms.Where(n => n.Difficulty.ToString().ToLower() == "easy")]
        };

        LevelGroup mediun = new()
        {
            Title = "Medium",
            Levels = [.. nonograms.Where(n => n.Difficulty.ToString().ToLower() == "medium")]
        };

        LevelGroup hard = new()
        {
            Title = "Hard",
            Levels = [.. nonograms.Where(n => n.Difficulty.ToString().ToLower() == "hard")]
        };

        LevelsDTO vm = new();
        vm.LevelGroups.Add(easy);
        vm.LevelGroups.Add(mediun);
        vm.LevelGroups.Add(hard);

        return Ok(vm);
    }

    [HttpGet("/create")]
    public IActionResult Create() => Ok(new CreateDTO());

    [HttpPost("/create")]
    public async Task<IActionResult> Create(CreateDTO vm)
    {
        if (!vm.GridJson.Contains('1'))
        {
            return BadRequest("Grid is empty");
        }

        if(String.IsNullOrEmpty(vm.Name))
        {
            return BadRequest("Name is empty");
        }

        int[][] solution = JsonSerializer.Deserialize<int[][]>(vm.GridJson)!;

        NonogramEntity entity = new()
        {
            Name = vm.Name,
            Difficulty = DifficultyCalculator.Calculate(solution).ToString().ToLower(),
            SolutionJson = vm.GridJson
        };

        await db.Nonograms.AddAsync(entity);
        await db.SaveChangesAsync();

        return Ok();
    }
}
