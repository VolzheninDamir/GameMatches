using System;
using System.Windows.Input;

namespace MatchGame.Commands.Base
{   //настраиваемые методы доступа к событиям
    public abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);
    }
}
