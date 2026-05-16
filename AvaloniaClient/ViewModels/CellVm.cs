using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AvaloniaClient.ViewModels;

public partial class CellVm : ViewModelBase
{
    public int Row { get; }
    public int Col { get; }
    private static ImageBrush cross = new(new Bitmap("Assets/close.png"));
    public MainWindowViewModel Vm { get; }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundBrush))]
    [NotifyPropertyChangedFor(nameof(BorderBrush))]
    private State state;

    public IBrush BackgroundBrush
        => state == State.Blank ? Brushes.White : state == State.Colored ?  SolidColorBrush.Parse("#490092") : cross;

    public IBrush BorderBrush
        => state == State.Colored ? SolidColorBrush.Parse("#BDBDBD") : SolidColorBrush.Parse("#BFAEDC");

    public CellVm(int row, int col, State state, MainWindowViewModel vm)
    {
        Row = row;
        Col = col;
        Vm = vm;
        this.state = state;
        //LeftClickCommand = new RelayCommand(HandleLeftClick);
        //RightClickCommand = new RelayCommand<PointerPressedEventArgs>(HandleRightClick);
        Click = new RelayCommand<PointerPressedEventArgs>(HandleClick);
    }

    //public ICommand LeftClickCommand { get; }
    //private void HandleLeftClick()
    //{
    //    Vm.HandleLeftClick(this);
    //}

    //public ICommand RightClickCommand { get; }
    //private void HandleRightClick(PointerPressedEventArgs e)
    //{
    //    if (e.GetCurrentPoint(null).Properties.IsRightButtonPressed)
    //        Vm.HandleRightClick(this);
    //        //State = State.Crossed;
    //}

    public ICommand Click { get; }
    private void HandleClick(PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsRightButtonPressed)
            Vm.HandleRightClick(this);
        else if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
            Vm.HandleLeftClick(this);
    }
}

public enum State
{
    Blank, 
    Colored, 
    Crossed
}
