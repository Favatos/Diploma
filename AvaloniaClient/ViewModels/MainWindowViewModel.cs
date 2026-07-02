using MsBox.Avalonia;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public Nonogram Nonogram { get; set; }
    public List<CellVm> CellVms { get; set; } = new();
    public GameSession Session { get; }
    public Action CloseWindow { get; }
    public int Lives
    {
        get => Session.Lives;
        set
        {
            Session.Lives = value;
            OnPropertyChanged(nameof(Lives));
        }
    }

    public MainWindowViewModel(Nonogram nonogram, GameSession session, Action closeWindow) 
    {
        Nonogram = nonogram;
        CloseWindow = closeWindow;
        ConvertGgrid();
        Session = session;
        Session.PropertyChanged += OnPropertyChanged;
    }

    public void ConvertGgrid()
    {
        for (int i = 0; i < Nonogram.Height; i++)
        {
            for (int j = 0; j < Nonogram.Width; j++)
            {
                CellVms.Add(new(i, j, State.Blank, this));
            }
        }
    }

    public void HandleLeftClick(CellVm cell)
    {
        Session.ColorCellBlack(cell.Row, cell.Col);

        int right = Session.Nonogram.Solution[cell.Row][cell.Col];
        int current = Session.CurrentGrid[cell.Row][cell.Col];

        if (right == 1 && current == 1)
        {
            cell.State = State.Colored;
        }
        else if (right == 0 && current == 2)
        {
            cell.State = State.Crossed;
        }

        if (Session.IsRowSolved(cell.Row))
        {
            for (int col = 0; col < Nonogram.Solution[cell.Row].Length; col++)
            {
                if (Session.CurrentGrid[cell.Row][col] == 0)
                {
                    Session.CurrentGrid[cell.Row][col] = 2;

                    Session.ColorCellCross(cell.Row, col);
                    CellVm? c = CellVms.FirstOrDefault(c => (c.Row == cell.Row && c.Col == col));

                    if (c != null)
                    {
                        c.State = State.Crossed;
                    }
                }
            }
        }

        if (Session.IsColSolved(cell.Col))
        {
            for (int row = 0; row < Nonogram.Solution.Length; row++)
            {
                if (Session.CurrentGrid[row][cell.Col] == 0)
                {
                    Session.CurrentGrid[row][cell.Col] = 2;

                    Session.ColorCellCross(row, cell.Col);
                    CellVm? c = CellVms.FirstOrDefault(c => (c.Row == row && c.Col == cell.Col));

                    if (c != null)
                    {
                        c.State = State.Crossed;
                    }
                }
            }
        }

        Session.CheckWin();
        CheckWin();
    }

    public void HandleRightClick(CellVm cell)
    {
        int before = Session.CurrentGrid[cell.Row][cell.Col];
        int right = Session.Nonogram.Solution[cell.Row][cell.Col];

        Session.ColorCellCross(cell.Row, cell.Col);

        int current = Session.CurrentGrid[cell.Row][cell.Col];

        if (right == 0 && current == 2)
        {
            cell.State = State.Crossed;
            return;
        }

        if (before == 1 && current == 1)
        {
            return;
        }

        if (right == 1 && current == 0)
        {
            cell.State = State.Colored;
        }

        Session.CheckWin();
        CheckWin();
    }

    public async void CheckWin()
    {
        if (Session.Status == Status.Lost) 
        {
            var mBox =  MessageBoxManager.GetMessageBoxStandard("Loss", "Game over");
            await mBox.ShowAsync();
            CloseWindow();
        }
        else if (Session.Status == Status.Win)
        {
            var mBox = MessageBoxManager.GetMessageBoxStandard("Win", "You win");
            await mBox.ShowAsync();
            CloseWindow();
        }
    }
}
