using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Calculator.Commands;
using System.Globalization;
using System.IO;
using System.Windows;


namespace Calculator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Members
        List<string> EnteredKeys;
        double Number = 0;
        bool FirstNumberEntered = true;
        bool EqualToFlag = true;
        bool FunctionPressed = false;
        string SelectedFunction = "";
        public string PreviousEnteredKey = "";
        private bool _digitGroupingEnabled;
        private ButtonPressedCommand _buttonPressedCommand;
        private string _KeyPressedString;
        private string _Entered_Number;
        private CutCommand _cutCommand;
        private CopyCommand _copyCommand;
        private PasteCommand _pasteCommand;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public MainViewModel()
        {
            KeyPressedString = "";
            Entered_Number = "";
            EnteredKeys = new List<string>();
            buttonPressedCommand = new ButtonPressedCommand(this);

            CutCommand = new CutCommand(this);
            CopyCommand = new CopyCommand(this);
            PasteCommand = new PasteCommand(this);
            LoadSettings();

        }
        #endregion
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public string KeyPressedString
        {
            get { return _KeyPressedString; }
            set
            {
                _KeyPressedString = value;
                OnPropertyChanged("KeyPressedString");
            }
        }


        public string Entered_Number
        {
            get { return _Entered_Number; }
            set
            {
                _Entered_Number = value;
                OnPropertyChanged("Entered_Number");
            }
        }


        #region Comands
        public ButtonPressedCommand buttonPressedCommand
        {
            get { return _buttonPressedCommand; }
            set
            {
                _buttonPressedCommand = value;

            }
        }

        public CutCommand CutCommand
        {
            get { return _cutCommand; }
            set { _cutCommand = value; }
        }


        public CopyCommand CopyCommand
        {
            get { return _copyCommand; }
            set { _copyCommand = value; }
        }


        public PasteCommand PasteCommand
        {
            get { return _pasteCommand; }
            set { _pasteCommand = value; }
        }
        #endregion

        #region Digit Grouping
        public bool DigitGroupingEnabled
        {
            get { return _digitGroupingEnabled; }
            set
            {
                _digitGroupingEnabled = value;
                OnPropertyChanged("DigitGroupingEnabled");

                FormatDisplayNumber();

                SaveSettings();
            }
        }
        public void FormatDisplayNumber()
        {
            if (Entered_Number == "Error" || string.IsNullOrEmpty(Entered_Number))
                return;

            try
            {
                double value = Convert.ToDouble(Entered_Number);

                if (DigitGroupingEnabled)
                {

                    Entered_Number = value.ToString("N", CultureInfo.CurrentCulture);


                    if (value == Math.Floor(value))
                    {
                        Entered_Number = Entered_Number.Split(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)[0];
                    }
                }
                else
                {

                    Entered_Number = value.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch
            {

            }
        }


        private double GetNumericValue(string displayText)
        {
            if (displayText == "Error")
                return 0;

            string numericText = displayText.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "");
            return Convert.ToDouble(numericText, CultureInfo.CurrentCulture);
        }
        #endregion

        #region ConfigSave

        private void SaveSettings()
        {
            try
            {
                string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.txt");
                File.WriteAllText(settingsPath, DigitGroupingEnabled.ToString());
            }
            catch
            {

            }
        }

        private void LoadSettings()
        {
            try
            {
                string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.txt");
                if (File.Exists(settingsPath))
                {
                    string setting = File.ReadAllText(settingsPath);
                    DigitGroupingEnabled = bool.Parse(setting);
                }
            }
            catch
            {

                DigitGroupingEnabled = false;
            }
        }
        #endregion

        #region Functions

        void Update()
        {
            string temp = "";
            for (int i = 0; i < EnteredKeys.Count; i++)
            {
                temp += EnteredKeys[i];
            }
            KeyPressedString = temp;
        }
        void Addition()
        {
            Number += Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void Multiplication()
        {
            Number *= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void Division()
        {
            double divisor = Convert.ToDouble(Entered_Number);
            if (divisor == 0)
            {
                Entered_Number = "Error";
                Number = 0;
                return;

            }
            Number /= divisor;
            Entered_Number = Number.ToString();
            FormatDisplayNumber();

        }
        void BackSpace()
        {
            if (Entered_Number == "Error")
            {
                Entered_Number = "0";
                return;
            }

            if (Entered_Number.Length > 1)
            {


                KeyPressedString = KeyPressedString.Substring(0, KeyPressedString.Length - 1);
                Entered_Number = Entered_Number.Substring(0, Entered_Number.Length - 1);
                Number = Convert.ToDouble(Entered_Number);
            }
            else
            {

                Entered_Number = "0";
            }


            if (EnteredKeys.Count > 0 && !FunctionPressed)
            {
                EnteredKeys.RemoveAt(EnteredKeys.Count - 1);
                Update();
            }

        }

        void Subtraction()
        {
            Number -= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void EqualTo()
        {
            EnteredKeys.Clear();
            EnteredKeys.Add(Entered_Number);
            EqualToFlag = false;
        }
        void Reciprocal()
        {
            Number = Convert.ToDouble(Entered_Number);
            if (Number == 0)
            {
                Entered_Number = "Error";
                return;
            }
            Number = 1 / Number;
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void Percentage()
        {
            Number = Convert.ToDouble(Entered_Number);
            Number /= 100;
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void ClearEntry()
        {
            
        }
        
        void Square()
        {
            Number = Convert.ToDouble(Entered_Number);
            Number *= Number;
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void SquareRoot()
        {
            Number = Convert.ToDouble(Entered_Number);
            Number = Math.Sqrt(Number);
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void Negate()
        {
            Number = Convert.ToDouble(Entered_Number);
            Number = -Number;
            Entered_Number = Number.ToString();
            FormatDisplayNumber();
        }
        void Clear()
        {
            EnteredKeys.Clear();
            KeyPressedString = "";
            Entered_Number = "0";
            Number = 0;
            FirstNumberEntered = true;
            EqualToFlag = false;

        }

        #endregion

        #region ButtonPressed
        bool PressedButtonOperator(string pressedButton)
        {
            if (pressedButton == "0" || pressedButton == "1" || pressedButton == "2" || pressedButton == "3" || pressedButton == "4"
            || pressedButton == "5" || pressedButton == "6" || pressedButton == "7" || pressedButton == "8" || pressedButton == "9" || pressedButton == ".")

            {
                if (EqualToFlag)
                {
                    Clear();
                }

                if (pressedButton == "." && Entered_Number.Contains("."))
                    return false;

                EnteredKeys.Add(pressedButton);
                Update();

                PreviousEnteredKey = pressedButton;
                if (FunctionPressed)
                {
                    Entered_Number = "0";
                    FunctionPressed = false;
                }

                if (Entered_Number == "0")
                {
                    Entered_Number = pressedButton;
                }
                else
                {
                    Entered_Number += pressedButton;
                }
                return false;
            }
            else
                return true;
        }
        public void GetPressedButton(string pressedButton)
        {
            if (PressedButtonOperator(pressedButton))
            {
                if (PreviousEnteredKey != pressedButton && PreviousEnteredKey != "+" && PreviousEnteredKey != "-" && PreviousEnteredKey != "/" && PreviousEnteredKey != "*")
                {
                    if (EnteredKeys.Count == 0)
                    {
                        EnteredKeys.Add(Entered_Number);
                        Update();
                    }
                    if (FirstNumberEntered)
                    {
                        Number = Convert.ToDouble(Entered_Number);
                        FirstNumberEntered = false;
                    }
                    else
                    {
                        switch (SelectedFunction)
                        {
                            case "Addition":
                                Addition();
                                break;
                            case "Multiplication":
                                Multiplication();
                                break;
                            case "Division":
                                Division();
                                break;
                            case "Subtraction":
                                Subtraction();
                                break;
                            case "Percentage":
                                Percentage();
                                break;
                            case "BackSpace":
                                BackSpace();
                                break;
                            case "Reciprocal":
                                Reciprocal();
                                break;
                            case "Square":
                                Square();
                                break;
                            case "SquareRoot":
                                SquareRoot();
                                break;
                            case "Negate":
                                Negate();
                                break;

                        }
                    }
                    switch (pressedButton)
                    {
                        case "+":
                            SelectedFunction = "Addition";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "-":
                            SelectedFunction = "Subtraction";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "*":
                            SelectedFunction = "Multiplication";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "/":
                            SelectedFunction = "Division";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "%":
                            SelectedFunction = "Percentage";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "Negate":
                            SelectedFunction = "Negate";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "Square":
                            SelectedFunction = "Square";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "SquareRoot":
                            SelectedFunction = "SquareRoot";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "1/x":
                            SelectedFunction = "Reciprocal";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "BackSpace":
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            BackSpace();
                            break;
                        case "=":
                            SelectedFunction = "EqualTo";
                            EnteredKeys.Add(pressedButton);
                            Update();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            EqualTo();
                            break;
                        case "ClearEntry":
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            ClearEntry();
                            break;
                        case "Clear":
                            FunctionPressed = true;
                            PreviousEnteredKey = pressedButton;
                            Clear();
                            break;
                    }

                }
                else if (PreviousEnteredKey == "+" || PreviousEnteredKey == "-" || PreviousEnteredKey == "/" || PreviousEnteredKey == "*" || PreviousEnteredKey == "Clear" ||
                    PreviousEnteredKey == "ClearEntry" || PreviousEnteredKey == "BackSpace" || PreviousEnteredKey == "Percentage" || PreviousEnteredKey == "Negate" || PreviousEnteredKey == "Sqare"
                    || PreviousEnteredKey == "SquareRoot" || PreviousEnteredKey == "Reciprocal")
                {
                    EnteredKeys.RemoveAt(EnteredKeys.Count - 1);
                    EnteredKeys.Add(pressedButton);
                    Update();
                    PreviousEnteredKey = pressedButton;
                    FunctionPressed = true;
                    switch (pressedButton)
                    {
                        case "+":
                            SelectedFunction = "Addition";
                            break;
                        case "-":
                            SelectedFunction = "Subtraction";
                            break;
                        case "*":
                            SelectedFunction = "Multiplication";
                            break;
                        case "/":
                            SelectedFunction = "Division";
                            break;
                        case "%":
                            SelectedFunction = "Percentage";
                            break;
                        case "=":
                            SelectedFunction = "EqualTo";
                            EqualTo();
                            break;
                        case "Negate":
                            SelectedFunction = "Negate";
                            Negate();
                            break;
                        case "Square":
                            SelectedFunction = "Square";
                            Square();
                            break;
                        case "SquareRoot":
                            SelectedFunction = "SquareRoot";
                            SquareRoot();
                            break;
                        case "Reciprocal":
                            SelectedFunction = "Reciprocal";
                            Reciprocal();
                            break;
                        case "BackSpace":
                            BackSpace();
                            break;
                        case "Clear":
                            Clear();
                            break;
                        case "ClearEntry":
                            ClearEntry();
                            break;
                    }
                }
            }

        }
        #endregion
    }
}

