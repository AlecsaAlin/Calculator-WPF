using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Calculator.ViewModels;
namespace Calculator.ViewModels
{
    public class ButtonPressedCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainViewModel _viewModel;

        public MainViewModel viewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            viewModel.GetPressedButton(parameter.ToString());
        }

        public ButtonPressedCommand(MainViewModel mainViewModel)
        {
            viewModel = mainViewModel;
        } 
    }
}
