using System.ComponentModel.DataAnnotations;

namespace Shared;

public class CreateDTO
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [Range(5, 25)]
    public int Width { get; set; } = 5;
    [Required]
    [Range(5, 25)]
    public int Height { get; set; } = 5;
    [Required]
    public string GridJson { get; set; } = null!;
}
