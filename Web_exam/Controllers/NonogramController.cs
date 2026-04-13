using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Text.Json;
using Web_exam.Data;
using Web_exam.Models;
using Web_exam.ViewModels;

namespace Web_exam.Controllers;

public class NonogramController : Controller
{
    private readonly NonogramDbContext db;

    public NonogramController(NonogramDbContext db)
    {
        this.db = db;
    }

    public async Task<IActionResult> Play(int id)
    {
        NonogramEntity? entity = await db.Nonograms.FindAsync(id);

        if (entity == null) return View("There is no such nonogram... Yet.");

        Nonogram nonogram = new(entity.Name, JsonSerializer.Deserialize<int[][]>(entity.SolutionJson)!);
        PlayVm vm = new PlayVm()
        {
            Id = id,
            SolutionJson = entity.SolutionJson,
            Nonogram = nonogram
        };

        return View(vm);
    }

    public async Task<IActionResult> Index()
    {
        List<NonogramEntity> nonograms = await db.Nonograms.ToListAsync();
        if (!nonograms.Any()) return NotFound("Nonograms not found");

        LevelGroup easy = new()
        {
            Title = "Easy",
            Levels = [.. nonograms.Where(n => n.Difficulty.ToLower() == "easy")]
        };

        LevelGroup mediun = new()
        {
            Title = "Medium",
            Levels = [.. nonograms.Where(n => n.Difficulty.ToLower() == "medium")]
        };

        LevelGroup hard = new()
        {
            Title = "Hard",
            Levels = [.. nonograms.Where(n => n.Difficulty.ToLower() == "hard")]
        };

        LevelsVm vm = new();
        vm.LevelGroups.Add(easy);
        vm.LevelGroups.Add(mediun);
        vm.LevelGroups.Add(hard);

        return View(vm);
    }

    public IActionResult Create() => View(new CreateVm());

    [HttpPost]
    public async Task<IActionResult> Create(CreateVm vm)
    {
        if (!ModelState.IsValid) return View(vm);
        if (!vm.GridJson.Contains('1'))
        {
            ModelState.AddModelError(string.Empty, "Grid is empty");
            return View(vm);
        }

        int[][] solution = JsonSerializer.Deserialize<int[][]>(vm.GridJson)!;
        Nonogram nonogram = new(vm.Name, solution);

        NonogramEntity entity = new()
        {
            Name = vm.Name,
            Difficulty = nonogram.Difficulty.ToString().ToLower(),
            SolutionJson = vm.GridJson
        };

        await db.Nonograms.AddAsync(entity);
        await db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
