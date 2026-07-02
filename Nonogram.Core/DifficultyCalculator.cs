namespace Shared;

public class DifficultyCalculator
{
    public static Difficulty Calculate(int[][] solution)
    {
        int height = solution.Length;
        int width = solution[0].Length;

        int total = height * width;
        int filled = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (solution[i][j] == 1)
                    filled++;
            }
        }

        double ratio = (double)filled / total;
        int maxSide = Math.Max(height, width);

        Difficulty result;

        if (maxSide <= 5)
            result = Difficulty.Easy;
        else if (maxSide <= 10)
            result = Difficulty.Medium;
        else
            result = Difficulty.Hard;

        if (ratio >= 0.3 && ratio <= 0.6)
        {
            if (result == Difficulty.Easy)
                result = Difficulty.Medium;
            else if (result == Difficulty.Medium)
                result = Difficulty.Hard;
        }

        else if (ratio < 0.2 || ratio > 0.7)
        {
            if (result == Difficulty.Hard)
                result = Difficulty.Medium;
            else if (result == Difficulty.Medium)
                result = Difficulty.Easy;
        }

        return result;
    }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
