using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaClient;

public partial class TopHintControll : UserControl
{
    private List<List<int>> colHints;
    public List<List<int>> ColHints { get => colHints;  set { 
            colHints = value;
            BuildHints();
        } 
    }
    public int MaxColHintsCount => ColHints.Max(r => r.Count);

    public TopHintControll()
    {
         InitializeComponent();
    } 

    public void BuildHints()
    {
        Grid grid = new Grid();

        for (int col = 0; col < ColHints.Count; col++)
        {
            grid.ColumnDefinitions.Add(new());
        }

        for (int row = 0; row < MaxColHintsCount; row++)
        {
            grid.RowDefinitions.Add(new());
        }

        for (int col = 0; col < ColHints.Count; col++)
        {
            for (int row = 0; row < ColHints[col].Count; row++)
            {
                Grid cell = new Grid();
                cell.Width = 20;
                cell.Height = 20;
                TextBlock number = new();
                number.TextAlignment = Avalonia.Media.TextAlignment.Center;
                number.Text = ColHints[col][row].ToString();
                cell.Children.Add(number);

                grid.Children.Add(cell);
                Grid.SetRow(cell, MaxColHintsCount - ColHints[col].Count + row);
                Grid.SetColumn(cell, col);
            }
        }

        Content = grid;
    }
}
