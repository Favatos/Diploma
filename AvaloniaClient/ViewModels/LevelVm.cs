using Avalonia.Controls;
using AvaloniaClient.Views;
using CommunityToolkit.Mvvm.Input;
using Shared;
using System;
using System.Windows.Input;

namespace AvaloniaClient.ViewModels;

public partial class LevelVm : ViewModelBase
{
    public Nonogram Nonogram { get; set; }
    public Action<Window> ShowWindow {  get; }
    
    public LevelVm(Nonogram nonogram, Action<Window> showWindow)
    {
        Nonogram = nonogram;
        ShowWindow = showWindow;
        ClickCommand = new RelayCommand(HandleClick);
    }

    public ICommand ClickCommand { get; }
    private void HandleClick()
    {
        MainWindow mainWindow = new MainWindow(Nonogram.Id);
        ShowWindow(mainWindow);
    }
}
