using Shared;
namespace Web_exam.ViewModels;

public class PlayVm
{
    public int Id { get; set; }
    public string SolutionJson { get; set; } = null!;
    public Nonogram Nonogram { get; set; } = null!;
}
