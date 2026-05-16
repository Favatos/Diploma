using Avalonia.Controls;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaClient;

public partial class LeftHintControll : UserControl
{
    private List<List<int>> rowHints;
    public List<List<int>> RowHints
    {
        get => rowHints; set
        {
            rowHints = value;
            BuildHints();
        }
    }
    public int MaxRowHintsCount => RowHints.Max(r => r.Count);

    public LeftHintControll()
    {
        InitializeComponent();
    }

    public void BuildHints()
    {
        Grid grid = new Grid();

        for (int row = 0; row < RowHints.Count; row++)
        {
            grid.RowDefinitions.Add(new());
        }

        for (int col = 0; col < MaxRowHintsCount; col++)
        {
            grid.ColumnDefinitions.Add(new());
        }

        for (int row = 0; row < RowHints.Count; row++)
        {
            for (int col = 0; col < RowHints[row].Count; col++)
            {
                Grid cell = new Grid();
                cell.Width = 20;
                cell.Height = 20;
                TextBlock number = new();
                number.TextAlignment = Avalonia.Media.TextAlignment.Center;
                number.Text = RowHints[row][col].ToString();
                cell.Children.Add(number);

                grid.Children.Add(cell);
                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, MaxRowHintsCount - RowHints[row].Count + col);
            }
        }

        Content = grid;
    }
}