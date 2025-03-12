using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PerfWatch.ViewModels;
using PerfWatch.Views;

namespace PerfWatch;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);
        if (type == typeof(LoginView)) return new LoginView { DataContext = param };

        if (type == typeof(MainView)) return new MainView { DataContext = param };

        if (type != null) return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}