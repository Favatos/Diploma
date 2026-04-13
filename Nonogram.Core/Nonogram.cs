namespace Shared;

public class Nonogram
{
    public int Id { get; set; }
    public string Name { get; } = null!;
    public Difficulty Difficulty { get; }
    public int[][] Solution { get; }
    public int Height => Solution.Length;
    public int Width => Solution[0].Length;

    public List<List<int>> RowHints => GetRowHints(Solution);
    public List<List<int>> ColHints => GetColumnHints(Solution);
    public int MaxRowHintsCount => RowHints.Max(r => r.Count);
    public int MaxColHintsCount => ColHints.Max(c => c.Count);


    public Nonogram(int id, string name, int[][] solution)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Название нонограммы не может быть пустым");
        if (solution == null || solution.Length == 0)
            throw new ArgumentException("Решение нонограммы не может быть пустым.", nameof(solution));

        int width = solution[0].Length;

        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] == null || solution[i].Length != width)
                throw new ArgumentException("Все строки должны быть одинаковой длины");

            for (int j = 0; j < solution[i].Length; j++) {
                if (solution[i][j] != 0 && solution[i][j] != 1)
                    throw new ArgumentException("Неверное значение поля");
            }
        }

        Id = id;
        Name = name;
        Solution = solution;
        Difficulty = DifficultyCalculator.Calculate(Solution);
    }

    public static List<int> GetLineHints(int[] line)
    {
        var result = new List<int>();
        int currentCount = 0;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == 1)
            {
                currentCount++;
            }
            else
            {
                if (currentCount > 0)
                {
                    result.Add(currentCount);
                    currentCount = 0;
                }
            }
        }

        if (currentCount > 0)
        {
            result.Add(currentCount);
        }

        return result;
    }

    public static List<List<int>> GetRowHints(int[][] solution)
    {
        int height = solution.Length;
        var rowHints = new List<List<int>>(height);

        for (int row = 0; row < height; row++)
        {
            var line = solution[row];
            rowHints.Add(GetLineHints(line));
        }

        return rowHints;
    }

    public static List<List<int>> GetColumnHints(int[][] solution)
    {
        int height = solution.Length;
        int width = solution[0].Length;

        var colHints = new List<List<int>>(width);

        for (int col = 0; col < width; col++)
        {
            var line = new int[height];
            for (int row = 0; row < height; row++)
            {
                line[row] = solution[row][col];
            }

            colHints.Add(GetLineHints(line));
        }

        return colHints;
    }

    public override string ToString()
    {
        return Name;
    }
}
