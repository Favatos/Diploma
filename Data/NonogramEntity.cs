using Shared;
using System.Text.Json;

namespace Data;

public class NonogramEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
    public string SolutionJson { get; set; } = null!;


    public Nonogram ConverToNonogram()
    {
        return new(Id, Name, JsonSerializer.Deserialize<int[][]>(SolutionJson) ?? throw new FormatException());
    }
}

