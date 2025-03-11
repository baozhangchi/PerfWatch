using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;

namespace PerfWatch.Views;

public partial class ShellView : UserControl
{
    public ShellView()
    {
        InitializeComponent();
        Loaded += ShellView_Loaded;
    }

    private void ShellView_Loaded(object? sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send<object, string>(this, "ShellViewLoaded");
    }
}