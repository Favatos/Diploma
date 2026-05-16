using MsBox.Avalonia;
using Shared;
using System;
using System.Collections.Generic;

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
            //GameView.animateError(cell, 2);
            cell.State = State.Crossed;
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
            //GameView.animateError(cell, 1);
            cell.State = State.Colored;
        }

        Session.CheckWin();
        CheckWin();
    }

    public async void CheckWin()
    {
        if (Session.Status == Status.Lost) 
        {
            var mBox =  MessageBoxManager.GetMessageBoxStandard("Проигрыш", "Игра окончена");
            await mBox.ShowAsync();
            CloseWindow();
        }
        else if (Session.Status == Status.Win)
        {
            var mBox = MessageBoxManager.GetMessageBoxStandard("Победа", "Вы выиграли");
            await mBox.ShowAsync();
            CloseWindow();
        }
    }
}
