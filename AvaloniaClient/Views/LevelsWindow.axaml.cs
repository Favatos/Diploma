using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaClient.ViewModels;
using Shared;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace AvaloniaClient;

public partial class LevelsWindow : Window
{
    public LevelsWindow()
    {
        InitializeComponent();

        HttpClient client = new HttpClient();
        LevelsDTO levels = client.GetFromJsonAsync<LevelsDTO>("https://localhost:7199/index").Result ?? throw new NullReferenceException();

        DataContext = new LevelsWindowVm(levels, window => window.ShowDialog(this) );
    }
}