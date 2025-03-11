using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;

namespace PerfWatch.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;

    public ShellViewModel()
    {
        WeakReferenceMessenger.Default.Register<object, string>(this, "ShellViewLoaded", ShellViewLoaded);
        WeakReferenceMessenger.Default.Register<object, string>(this, "LoginSuccessed", LoginSuccessed);
    }

    private void LoginSuccessed(object recipient, object message)
    {
        CurrentViewModel = new MainViewModel();
    }

    private void ShellViewLoaded<TMessage>(object recipient, TMessage message) where TMessage : class
    {
        if (!GlobalCache.Instance.HasLogined)
        {
            CurrentViewModel = new LoginViewModel();
        }
        else
        {
            CurrentViewModel = new MainViewModel();
        }
    }
}