using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaClient.ViewModels;

namespace AvaloniaClient;

public partial class EditWindow : Window
{
    public EditWindow()
    {
        InitializeComponent();

        DataContext = new EditWindowVm(Close);
    }
}