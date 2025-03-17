using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calculator.ViewModels;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel();
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
            Loaded += (s, e) =>
            {
                Keyboard.Focus(this);
            };
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = this.DataContext as Calculator.ViewModels.MainViewModel;
            if (viewModel == null) return;


            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                viewModel.GetPressedButton((e.Key - Key.D0).ToString());
                e.Handled = true;
            }

            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                viewModel.GetPressedButton((e.Key - Key.NumPad0).ToString());
                e.Handled = true;
            }

            else switch (e.Key)
                {
                    case Key.Add:
                        viewModel.GetPressedButton("+");
                        e.Handled = true;
                        break;
                    case Key.Subtract:
                        viewModel.GetPressedButton("-");
                        e.Handled = true;
                        break;
                    case Key.Multiply:
                        viewModel.GetPressedButton("*");
                        e.Handled = true;
                        break;
                    case Key.Divide:
                        viewModel.GetPressedButton("/");
                        e.Handled = true;
                        break;
                    case Key.Decimal:
                    case Key.OemPeriod:
                        viewModel.GetPressedButton(".");
                        e.Handled = true;
                        break;
                    case Key.Enter:
                        viewModel.GetPressedButton("=");
                        e.Handled = true;
                        break;
                    case Key.Back:
                        viewModel.GetPressedButton("BackSpace");
                        e.Handled = true;
                        break;
                    case Key.Escape:
                        viewModel.GetPressedButton("Clear");
                        e.Handled = true;
                        break;

                }
        }
            private void About_Click(object sender, RoutedEventArgs e)
            {
                MessageBox.Show("Alecsa Alin-Claudiu  10LF231", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    
}