using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using Avalonia.Controls;
using PerfWatch.Views;

namespace PerfWatch.ViewModels;

internal partial class ShellViewModel : ViewModelBase
{
    [ObservableProperty] private UserControl? _currentView;

    public ShellViewModel()
    {
        WeakReferenceMessenger.Default.Register<object, string>(this, "ShellViewLoaded", ShellViewLoaded);
        WeakReferenceMessenger.Default.Register<object, string>(this, "LoginSuccessed", LoginSuccessed);
    }

    private void LoginSuccessed(object recipient, object message)
    {
        CurrentView = new MainView() { DataContext = new MainViewModel() };
    }

    private void ShellViewLoaded<TMessage>(object recipient, TMessage message) where TMessage : class
    {
        if (!GlobalCache.Instance.HasLogined)
        {
            CurrentView = new LoginView() { DataContext = new LoginViewModel() };
        }
        else
        {
            CurrentView = new MainView() { DataContext = new MainViewModel() };
        }
    }
}