using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace PerfWatch.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;

    public ShellViewModel()
    {
        WeakReferenceMessenger.Default.Register<object, string>(this, "ShellViewLoaded", ShellViewLoaded);
    }

    private void ShellViewLoaded<TMessage>(object recipient, TMessage message) where TMessage : class
    {
        CurrentViewModel = new LoginViewModel();
    }
}