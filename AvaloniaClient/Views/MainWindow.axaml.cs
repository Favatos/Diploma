using Avalonia.Animation.Easings;
using Avalonia.Controls;
using AvaloniaClient.ViewModels;
using Shared;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace AvaloniaClient.Views;

public partial class MainWindow : Window
{
    public MainWindow(int id)
    {
        InitializeComponent();

        HttpClient client = new HttpClient();
        NonogramDTO nonogramDTO = client.GetFromJsonAsync<NonogramDTO>($"https://localhost:7199/play?id={id}").Result ?? throw new NullReferenceException();
        int lives = nonogramDTO.Nonogram.Difficulty == Difficulty.Easy ? 5 : nonogramDTO.Nonogram.Difficulty == Difficulty.Medium ? 10 : 15;

        DataContext = new MainWindowViewModel(nonogramDTO.Nonogram, new(nonogramDTO.Nonogram, lives), Close);
        TopHints.ColHints = nonogramDTO.Nonogram.ColHints;
        LeftHints.RowHints = nonogramDTO.Nonogram.RowHints;
    }
}