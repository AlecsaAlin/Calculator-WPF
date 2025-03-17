using Calculator.ViewModels;
using System;
using System.Windows.Input;

namespace Calculator.Commands
{
    public class CutCommand : ICommand
    {
        private MainViewModel _viewModel;

        public CutCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Entered_Number) && _viewModel.Entered_Number != "0" && _viewModel.Entered_Number != "Error";
        }

        public void Execute(object parameter)
        {
            System.Windows.Clipboard.SetText(_viewModel.Entered_Number);
            _viewModel.Entered_Number = "0";
        }
    }

    public class CopyCommand : ICommand
    {
        private MainViewModel _viewModel;

        public CopyCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Entered_Number) && _viewModel.Entered_Number != "Error";
        }

        public void Execute(object parameter)
        {
            System.Windows.Clipboard.SetText(_viewModel.Entered_Number);
        }
    }

    public class PasteCommand : ICommand
    {
        private MainViewModel _viewModel;

        public PasteCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return System.Windows.Clipboard.ContainsText();
        }

        public void Execute(object parameter)
        {
            string text = System.Windows.Clipboard.GetText();
            if (double.TryParse(text.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, ""), out double result))
            {
                _viewModel.Entered_Number = text;
                // Format according to settings
                _viewModel.FormatDisplayNumber();
            }
        }
    }
}