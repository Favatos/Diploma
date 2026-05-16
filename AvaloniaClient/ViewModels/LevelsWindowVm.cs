using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Shared;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaClient.ViewModels;

public partial class LevelsWindowVm : ViewModelBase
{
    public LevelsDTO LevelsDTO { get; set; }
    public List<LevelVm> LevelsEasy {  get; set; }
    public List<LevelVm> LevelsMedium {  get; set; }
    public List<LevelVm> LevelsHard {  get; set; }
    public Action<Window> ShowWindow { get; }

    public ICommand OpenEditor { get; }
    private void HandleOpeEditor()
    {
        EditWindow window = new();
        window.Show();
    }

    public LevelsWindowVm(LevelsDTO l, Action<Window> showWindow) 
    {
        ShowWindow = showWindow;

        LevelsDTO = l;
        LevelsEasy = Convert(0);
        LevelsMedium = Convert(1);
        LevelsHard = Convert(2);

        OpenEditor = new RelayCommand(HandleOpeEditor);
    }

    public List<LevelVm> Convert(int index)
    {
        List<LevelVm> levels = [];
        for (int i = 0; i < LevelsDTO.LevelGroups[index].Levels.Count; i++)
        {
            levels.Add(new(LevelsDTO.LevelGroups[index].Levels[i], ShowWindow));
        }

        return levels;
    }
}
