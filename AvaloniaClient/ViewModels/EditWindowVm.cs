using CommunityToolkit.Mvvm.Input;
using Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Input;

namespace AvaloniaClient.ViewModels;

public partial class EditWindowVm : ViewModelBase
{
    public CreateDTO CreateDTO { get; set; } = new();
    public int[][] Grid {  get; set; }
    public List<EditCellVm> Cells { get; set; }
    public Action CloseWindow { get; }

    public int Height
    {
        get => CreateDTO.Height;
        set
        {
            CreateDTO.Height = value;
            OnPropertyChanged(nameof(Height));
            Grid = CreateEmptyGrid(Height, Width);

            ConvertGgrid();
            OnPropertyChanged(nameof(Cells));
        }
    }
    public int Width
    {
        get => CreateDTO.Width;
        set
        {
            CreateDTO.Width = value;
            OnPropertyChanged(nameof(Width));
            Grid = CreateEmptyGrid(Height, Width);

            ConvertGgrid();
            OnPropertyChanged(nameof(Cells));
        }
    }

    public ICommand Save { get; }
    private async void HandleSave()
    {
        CreateDTO.GridJson = JsonSerializer.Serialize(Grid);

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7199/create", CreateDTO);

        if (!response.IsSuccessStatusCode) throw new Exception("Cannot save nonogram");

        CloseWindow();
    }

    public EditWindowVm(Action closeWindow)
    {
        CloseWindow = closeWindow;
        Save = new RelayCommand(HandleSave);
    }

    public static int[][] CreateEmptyGrid(int height, int width)
    {
        int[][] grid = new int[height][];

        for (int i = 0; i < height; i++)
        {
            grid[i] = new int[width];

            for (int j = 0; j < width; j++)
            {
                grid[i][j] = 0;
            }
        }

        return grid;
    }

    public void ConvertGgrid()
    {
        Cells = new();

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Cells.Add(new(i, j, this));
            }
        }
    }

}
