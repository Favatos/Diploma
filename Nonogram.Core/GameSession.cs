namespace Shared;

public class GameSession
{
    public Nonogram Nonogram { get; set; }
    public int[][] CurrentGrid { get; set; }
    public int Lives { get; set; }
    public Status Status { get; set; }

    public GameSession(Nonogram nonogram, int lives) { 
        Nonogram = nonogram;
        Lives = lives;
        CurrentGrid = CreateEmptyGrid(nonogram.Height, nonogram.Width);
        Status = Status.Playing;
    }

    public static int[][] CreateEmptyGrid(int height, int width)
    {
        int[][] grid = new int[height][];

        for (int i = 0; i < height; i++){
            grid[i] = new int[width];

            for (int j = 0; j < width; j++){
                grid[i][j] = 0;
            }
        }

        return grid;
    }

    public void ColorCellBlack(int row, int col)
    {
        int current = CurrentGrid[row][col];
        int right = Nonogram.Solution[row][col];

        if (right == 1)
        {
            if (current != 1)
            {
                CurrentGrid[row][col] = 1;
            }
            return;
        }

        CheckLives();
        CurrentGrid[row][col] = 2;
    }

    public void ColorCellCross(int row, int col)
    {
        int current = CurrentGrid[row][col];
        int right = Nonogram.Solution[row][col];

        if (right == 0)
        {
            if (current != 2)
            {
                CurrentGrid[row][col] = 2;
            }
            return;
        }

        CheckLives();
        CurrentGrid[row][col] = 0;
    }

    public void CheckLives()
    {
        if (Lives == -1) return;

        Lives--;

        if (Lives <= 0)
        {
            Status = Status.Lost;
        }
    }

    public bool CheckWin()
    {
        for(int row = 0; row < Nonogram.Solution.Length; row++)
        {
            if (!IsRowSolved(row))
            {
                Console.WriteLine("not win");
                return false;
            }
        }

        Status = Status.Win;
        return true;
    }

    public bool IsRowSolved(int row)
    {
        for (int col = 0; col < Nonogram.Solution[row].Length; col++)
        {
            int right = Nonogram.Solution[row][col];
            int current = CurrentGrid[row][col];

            if (right == 1 && current != 1)
            {
                return false;
            }

            if (right == 0 && current == 1)
            {
                return false;
            }
        }

        Console.WriteLine("right");
        return true;
    }

    public bool IsColSolved(int col)
    {
        for(int row = 0;row < Nonogram.Solution.Length; row++)
        {
            int right = Nonogram.Solution[row][col];
            int current = CurrentGrid[row][col];

            if(right == 1 && current != 1) {
                return false;
            }

            if (right == 0 && current == 1)
            {
                return false;
            }
        }

        Console.WriteLine("right");
        return true;
    }
}

public enum Status
{
    Playing, 
    Lost,
    Win
}
