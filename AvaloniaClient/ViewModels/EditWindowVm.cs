using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Input;
using Tmds.DBus.Protocol;

namespace AvaloniaClient.ViewModels;

public partial class EditWindowVm : ViewModelBase
{
    public CreateDTO CreateDTO { get; set; } = new();
    public int[][] Grid {  get; set; }
    public List<EditCellVm> Cells { get; set; }
    public Action CloseWindow { get; }

    [Required]
    public string Name
    {
        get => CreateDTO.Name;
        set
        {
            CreateDTO.Name = value;
            OnPropertyChanged(nameof(Name));
            Save.NotifyCanExecuteChanged();
        }
    }

    [Required]
    [Range(5, 25, ErrorMessage = "Size must be from 5 to 25")]
    public int Height
    {
        get => CreateDTO.Height;
        set
        {
            CreateDTO.Height = 0;
            OnPropertyChanged(nameof(Height));

            CreateDTO.Height = Math.Clamp(value, 5, 25);
            OnPropertyChanged(nameof(Height));
            Grid = CreateEmptyGrid(Height, Width);

            ConvertGrid();
            OnPropertyChanged(nameof(Cells));
        }
    }

    [Required]
    [Range(5, 25)]
    public int Width
    {
        get => CreateDTO.Width;
        set
        {
            CreateDTO.Width = 0;
            OnPropertyChanged(nameof(Width));

            CreateDTO.Width = Math.Clamp(value, 5, 25);
            OnPropertyChanged(nameof(Width));
            Grid = CreateEmptyGrid(Height, Width);

            ConvertGrid();
            OnPropertyChanged(nameof(Cells));
        }
    }

    public IRelayCommand Save { get; }
    private async void HandleSave()
    {
        CreateDTO.GridJson = JsonSerializer.Serialize(Grid);

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7199/create", CreateDTO);

        if (!response.IsSuccessStatusCode)
        {
            var mBox = MessageBoxManager.GetMessageBoxStandard("Error", response.Content.ReadAsStringAsync().Result);
            await mBox.ShowAsync();
            return;
        }

        CloseWindow();
    }

    private bool CanSave()
    {
        if(string.IsNullOrEmpty(CreateDTO.Name)) return false;
        return true;
    }

    public EditWindowVm(Action closeWindow)
    {
        CloseWindow = closeWindow;
        Save = new RelayCommand(HandleSave, CanSave);
        Grid = CreateEmptyGrid(5, 5);
        ConvertGrid();
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

    public void ConvertGrid()
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
