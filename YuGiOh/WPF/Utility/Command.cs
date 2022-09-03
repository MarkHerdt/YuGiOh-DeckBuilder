using System;
using System.Windows.Input;

namespace YuGiOh_DeckBuilder.WPF.Utility;

/// <summary>
/// <see cref="ICommand"/>
/// </summary>
internal class Command : ICommand
{
    #region Members
    private readonly Action<string?> action;
    #endregion

    #region Constrcutor
    public Command(Action<string?> action)
    {
        this.action = action;
    }
    #endregion

    #region Events
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    #endregion

    #region Methods
    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => this.action(parameter?.ToString());
    #endregion
}