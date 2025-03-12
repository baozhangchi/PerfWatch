using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace PerfWatch.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _password;

    partial void OnUsernameChanged(string? value)
    {
        LoginCommand.NotifyCanExecuteChanged();
    }

    partial void OnPasswordChanged(string? value)
    {
        LoginCommand.NotifyCanExecuteChanged();
    }

    private bool CanLogin() => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private void Login()
    {
        GlobalCache.Instance.HasLogined = true;
        WeakReferenceMessenger.Default.Send<object,string>(this, "LoginSuccessed");
    }
}