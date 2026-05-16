using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AvaloniaClient.ViewModels;

public partial class EditCellVm : ViewModelBase
{
    public int Row { get; }
    public int Col { get; }
    public EditWindowVm Vm { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundBrush))]
    private State state;

    public IBrush BackgroundBrush
        => State == State.Blank ? Brushes.White : Brushes.Black;

    public EditCellVm(int row, int col, EditWindowVm vm)
    {
        Row = row;
        Col = col;
        Click = new RelayCommand(HandleClick);
        Vm = vm;
    }

    public ICommand Click { get; }
    private void HandleClick()
    {
        if (State == State.Blank)
        {
            State = State.Colored;
            Vm.Grid[Row][Col] = 1;
        }
        else
        {
            State = State.Blank;
            Vm.Grid[Row][Col] = 0;
        }
    }

        
}

