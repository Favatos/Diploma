using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaClient.ViewModels;
using AvaloniaClient.Views;
using Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace AvaloniaClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LevelsWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}