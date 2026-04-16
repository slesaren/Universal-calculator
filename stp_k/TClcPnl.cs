using Calculator;
using Calculator.Numbers;

using CalculatorMemory;

using CalculatorProcessor;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Calculator
{
    public enum CalculatorMode
    {
        PNumber,    
        Fraction,   
        Complex     
    }

    public class TClcPnl : Form
    {
        private ToolTip toolTip;
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
        private Panel displayPanel;
        private Panel buttonPanel;

        private TextBox displayNumber;
        private Label memoryState;

        private Button[,] digitButtons;
        private Button buttonPoint;
        private Button buttonSign;
        private Button buttonBackspace;
        private Button buttonClear;

        private Button buttonAdd;
        private Button buttonSub;
        private Button buttonMul;
        private Button buttonDiv;
        private Button buttonEqual;

        private Button buttonSqr;
        private Button buttonRev;

        private Button buttonMS;
        private Button buttonMR;
        private Button buttonMPlus;
        private Button buttonMC;

        private Label labelBase;
        private ComboBox comboBase;
        private Button buttonBaseApply;

        private Button buttonFracSimplify;
        private Button buttonFracMixed;
        private Button buttonFracProper;
        private Button buttonFracReciprocal;

        private Button buttonComplexI;
        private Button buttonComplexConjugate;
        private Button buttonComplexReal;
        private Button buttonComplexImaginary;
        private Button buttonComplexModulus;
        private Button buttonComplexArgument;
        private Button buttonComplexSwitchPart;

        private MenuStrip mainMenu;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuEdit;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuHelp;

        private ToolStripMenuItem menuEditCopy;
        private ToolStripMenuItem menuEditPaste;
        private ToolStripMenuItem menuEditCut;

        private ToolStripMenuItem menuViewPNumber;
        private ToolStripMenuItem menuViewFraction;
        private ToolStripMenuItem menuViewComplex;

        private ToolStripMenuItem menuHelpAbout;

        private TCtrl _calculator;

        private CalculatorMode _currentMode;
        private string _clipboardText = "";
        private string _memoryStateText = "";
        private int _currentBase = 10;

        public CalculatorMode CurrentMode
        {
            get => _currentMode;
            set
            {
                _currentMode = value;
                UpdateCalculatorMode();
                UpdateButtonVisibility();
            }
        }

        public string DisplayText
        {
            get => displayNumber.Text;
            set => displayNumber.Text = value;
        }

        public string MemoryStateText
        {
            get => memoryState.Text;
            set => memoryState.Text = value;
        }

        public TClcPnl()
        {
            InitializeComponent();
            InitializeCalculator();

            this.Load += FormCreate;
            this.KeyPress += FormKeyPress;
            this.Resize += Form_Resize;
        }

        private void InitializeToolTips()
        {
            toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 100,
                ShowAlways = true,
                ToolTipIcon = ToolTipIcon.Info,
                ToolTipTitle = "",
                UseAnimation = true,
                UseFading = true
            };

            SetupDigitToolTips();
            SetupOperationToolTips();
            SetupMemoryToolTips();
            SetupModeSpecificToolTips();
        }

        private void SetupDigitToolTips()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (digitButtons[i, j] != null)
                    {
                        string digit = digitButtons[i, j].Text;
                        string tooltipText = GetDigitToolTip(digit);
                        toolTip.SetToolTip(digitButtons[i, j], tooltipText);
                    }
                }
            }

            toolTip.SetToolTip(buttonPoint, "Десятичный разделитель (.)\nВвод десятичной точки");
            toolTip.SetToolTip(buttonSign, "Смена знака (+/-)\nИзменяет знак текущего числа на противоположный");
            toolTip.SetToolTip(buttonBackspace, "Забой (⌫)\nУдаляет последний введённый символ");
            toolTip.SetToolTip(buttonClear, "Очистка (C)\nСбрасывает текущее значение и состояние калькулятора");
        }

        private string GetDigitToolTip(string digit)
        {
            return digit switch
            {
                "0" => "Цифра 0\nВвод нуля",
                "1" => "Цифра 1\nВвод единицы",
                "2" => "Цифра 2\nВвод двойки",
                "3" => "Цифра 3\nВвод тройки",
                "4" => "Цифра 4\nВвод четвёрки",
                "5" => "Цифра 5\nВвод пятёрки",
                "6" => "Цифра 6\nВвод шестёрки",
                "7" => "Цифра 7\nВвод семёрки",
                "8" => "Цифра 8\nВвод восьмёрки",
                "9" => "Цифра 9\nВвод девятки",
                "A" => "Цифра A (10)\nИспользуется в 16-ричной системе",
                "B" => "Цифра B (11)\nИспользуется в 16-ричной системе",
                "C" => "Цифра C (12)\nИспользуется в 16-ричной системе",
                "D" => "Цифра D (13)\nИспользуется в 16-ричной системе",
                "E" => "Цифра E (14)\nИспользуется в 16-ричной системе",
                "F" => "Цифра F (15)\nИспользуется в 16-ричной системе",
                _ => $"Цифра {digit}"
            };
        }

        private void SetupOperationToolTips()
        {
            toolTip.SetToolTip(buttonAdd, "Сложение (+)\nСкладывает текущее значение с введённым");
            toolTip.SetToolTip(buttonSub, "Вычитание (-)\nВычитает введённое значение из текущего");
            toolTip.SetToolTip(buttonMul, "Умножение (×)\nУмножает текущее значение на введённое");
            toolTip.SetToolTip(buttonDiv, "Деление (÷)\nДелит текущее значение на введённое");
            toolTip.SetToolTip(buttonEqual, "Равно (=)\nВычисляет результат текущей операции");
            toolTip.SetToolTip(buttonSqr, "Квадрат (x²)\nВозводит текущее число в квадрат");
            toolTip.SetToolTip(buttonRev, "Обратное число (1/x)\nВычисляет число, обратное текущему");
        }

        private void SetupMemoryToolTips()
        {
            toolTip.SetToolTip(buttonMS, "MS (Memory Store)\nСохраняет текущее значение в память");
            toolTip.SetToolTip(buttonMR, "MR (Memory Recall)\nИзвлекает значение из памяти");
            toolTip.SetToolTip(buttonMPlus, "M+ (Memory Add)\nДобавляет текущее значение к значению в памяти");
            toolTip.SetToolTip(buttonMC, "MC (Memory Clear)\nОчищает память");
        }

        private void SetupModeSpecificToolTips()
        {
            toolTip.SetToolTip(comboBase, "Выбор основания системы счисления\nДоступны основания от 2 до 16");
            toolTip.SetToolTip(buttonBaseApply, "Применить основание\nИзменяет систему счисления текущего числа");

            toolTip.SetToolTip(buttonFracSimplify, "Сократить дробь\nПриводит дробь к несократимому виду");
            toolTip.SetToolTip(buttonFracMixed, "Смешанная дробь\nПреобразует неправильную дробь в смешанную");
            toolTip.SetToolTip(buttonFracProper, "Правильная дробь\nВыделяет целую часть из неправильной дроби");
            toolTip.SetToolTip(buttonFracReciprocal, "Обратная дробь\nМеняет числитель и знаменатель местами");

            toolTip.SetToolTip(buttonComplexI, "Мнимая единица (i)\nДобавляет мнимую единицу к числу");
            toolTip.SetToolTip(buttonComplexConjugate, "Сопряжённое число\nЗаменяет знак мнимой части на противоположный");
            toolTip.SetToolTip(buttonComplexReal, "Действительная часть\nИзвлекает действительную часть комплексного числа");
            toolTip.SetToolTip(buttonComplexImaginary, "Мнимая часть\nИзвлекает мнимую часть комплексного числа");
            toolTip.SetToolTip(buttonComplexModulus, "Модуль |z|\nВычисляет модуль комплексного числа");
            toolTip.SetToolTip(buttonComplexArgument, "Аргумент (arg)\nВычисляет аргумент комплексного числа в градусах");
            toolTip.SetToolTip(buttonComplexSwitchPart, "Переключение частей\nПереключает редактирование между действительной и мнимой частями");
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.Text = "Универсальный калькулятор";
            this.Size = new Size(700, 700);
            this.MinimumSize = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.KeyPreview = true;

            CreateMenu();

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            displayPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(5)
            };

            buttonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5),
                AutoScroll = true
            };

            displayNumber = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 20, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.Fixed3D,
                Text = "0",
                ReadOnly = true,
                Multiline = true,
                WordWrap = false,
                ScrollBars = ScrollBars.Horizontal,
                MaxLength = 100
            };

            memoryState = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 20,
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = ""
            };

            displayPanel.Controls.Add(displayNumber);
            displayPanel.Controls.Add(memoryState);

            CreateButtons();
            CreateModeSpecificButtons();

            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(displayPanel);

            this.Controls.Add(mainMenu);
            this.Controls.Add(mainPanel);

            this.MainMenuStrip = mainMenu;

            UpdateButtonVisibility();

            InitializeToolTips();
        }

        private void CreateMenu()
        {
            mainMenu = new MenuStrip();

            menuFile = new ToolStripMenuItem("Файл");
            menuFile.DropDownItems.Add("Выход", null, (s, e) => Application.Exit());

            menuEdit = new ToolStripMenuItem("Правка");
            menuEditCopy = new ToolStripMenuItem("Копировать", null, OnEditCopy);
            menuEditCopy.ShortcutKeys = Keys.Control | Keys.C;
            menuEditPaste = new ToolStripMenuItem("Вставить", null, OnEditPaste);
            menuEditPaste.ShortcutKeys = Keys.Control | Keys.V;
            menuEditCut = new ToolStripMenuItem("Вырезать", null, OnEditCut);
            menuEditCut.ShortcutKeys = Keys.Control | Keys.X;

            menuEdit.DropDownItems.Add(menuEditCopy);
            menuEdit.DropDownItems.Add(menuEditPaste);
            menuEdit.DropDownItems.Add(menuEditCut);

            menuView = new ToolStripMenuItem("Вид");
            menuViewPNumber = new ToolStripMenuItem("р-ичные числа", null, OnModeChange);
            menuViewFraction = new ToolStripMenuItem("Простые дроби", null, OnModeChange);
            menuViewComplex = new ToolStripMenuItem("Комплексные числа", null, OnModeChange);

            menuViewPNumber.Checked = true;
            _currentMode = CalculatorMode.PNumber;

            menuView.DropDownItems.Add(menuViewPNumber);
            menuView.DropDownItems.Add(menuViewFraction);
            menuView.DropDownItems.Add(menuViewComplex);

            menuHelp = new ToolStripMenuItem("Справка");

            var menuHelpContents = new ToolStripMenuItem("Содержание", null, OnHelpContents);
            menuHelpContents.ShortcutKeys = Keys.F1;

            menuHelpAbout = new ToolStripMenuItem("О программе", null, OnHelpAbout);

            menuHelp.DropDownItems.Add(menuHelpContents);
            menuHelp.DropDownItems.Add(new ToolStripSeparator());
            menuHelp.DropDownItems.Add(menuHelpAbout);

            mainMenu.Items.Add(menuFile);
            mainMenu.Items.Add(menuEdit);
            mainMenu.Items.Add(menuView);
            mainMenu.Items.Add(menuHelp);
        }

        private void OnHelpContents(object sender, EventArgs e)
        {
            CalculatorHelp.ShowHelp();
        }

        private void CreateButtons()
        {
            int buttonWidth = 70;
            int buttonHeight = 50;
            int spacing = 5;
            int startX = 5;
            int startY = 5;

            string[] digits = { "7", "8", "9", "A", "4", "5", "6", "B", "1", "2", "3", "C", "0", "D", "E", "F" };

            digitButtons = new Button[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int index = i * 4 + j;
                    if (index < digits.Length)
                    {
                        Button btn = new Button
                        {
                            Text = digits[index],
                            Size = new Size(buttonWidth, buttonHeight),
                            Location = new Point(startX + j * (buttonWidth + spacing),
                                                startY + i * (buttonHeight + spacing)),
                            Font = new Font("Arial", 14, FontStyle.Bold),
                            Tag = GetDigitCommand(digits[index][0]),
                            BackColor = Char.IsLetter(digits[index][0]) ? Color.LightSteelBlue : SystemColors.Control
                        };
                        btn.Click += ButtonClick;
                        buttonPanel.Controls.Add(btn);
                        digitButtons[i, j] = btn;
                    }
                }
            }

            int rowOffset = 4 * (buttonHeight + spacing) + 10;

            buttonPoint = new Button
            {
                Text = ".",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX, startY + rowOffset),
                Font = new Font("Arial", 18, FontStyle.Bold),
                Tag = 17
            };
            buttonPoint.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonPoint);

            buttonSign = new Button
            {
                Text = "+/-",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Tag = 0
            };
            buttonSign.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonSign);

            buttonBackspace = new Button
            {
                Text = "⌫",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + 2 * (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 18, FontStyle.Bold),
                Tag = 18
            };
            buttonBackspace.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonBackspace);

            buttonClear = new Button
            {
                Text = "C",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + 3 * (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.LightCoral,
                Tag = 28
            };
            buttonClear.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonClear);

            rowOffset += (buttonHeight + spacing);

            int opStartX = startX + 4 * (buttonWidth + spacing);

            buttonAdd = new Button
            {
                Text = "+",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(opStartX, startY),
                Font = new Font("Arial", 20, FontStyle.Bold),
                BackColor = Color.LightBlue,
                Tag = 21
            };
            buttonAdd.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonAdd);

            buttonSub = new Button
            {
                Text = "-",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(opStartX, startY + (buttonHeight + spacing)),
                Font = new Font("Arial", 20, FontStyle.Bold),
                BackColor = Color.LightBlue,
                Tag = 22
            };
            buttonSub.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonSub);

            buttonMul = new Button
            {
                Text = "×",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(opStartX, startY + 2 * (buttonHeight + spacing)),
                Font = new Font("Arial", 20, FontStyle.Bold),
                BackColor = Color.LightBlue,
                Tag = 23
            };
            buttonMul.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonMul);

            buttonDiv = new Button
            {
                Text = "÷",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(opStartX, startY + 3 * (buttonHeight + spacing)),
                Font = new Font("Arial", 20, FontStyle.Bold),
                BackColor = Color.LightBlue,
                Tag = 24
            };
            buttonDiv.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonDiv);

            buttonSqr = new Button
            {
                Text = "x²",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(opStartX, startY + 4 * (buttonHeight + spacing)),
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.LightGreen,
                Tag = 26
            };
            buttonSqr.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonSqr);

            buttonRev = new Button
            {
                Text = "1/x",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(opStartX, startY + 5 * (buttonHeight + spacing)),
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.LightGreen,
                Tag = 25
            };
            buttonRev.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonRev);

            buttonEqual = new Button
            {
                Text = "=",
                Size = new Size(buttonWidth * 2 + spacing, buttonHeight),
                Location = new Point(startX + 2 * (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 24, FontStyle.Bold),
                BackColor = Color.LightSalmon,
                Tag = 27
            };
            buttonEqual.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonEqual);

            CreateMemoryButtons(startX, startY, buttonWidth, buttonHeight, spacing, rowOffset + buttonHeight + spacing);
        }

        private void CreateMemoryButtons(int startX, int startY, int buttonWidth, int buttonHeight, int spacing, int rowOffset)
        {
            buttonMS = new Button
            {
                Text = "MS",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX, startY + rowOffset),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightYellow,
                Tag = 29
            };
            buttonMS.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonMS);

            buttonMR = new Button
            {
                Text = "MR",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightYellow,
                Tag = 30
            };
            buttonMR.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonMR);

            buttonMPlus = new Button
            {
                Text = "M+",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + 2 * (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightYellow,
                Tag = 31
            };
            buttonMPlus.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonMPlus);

            buttonMC = new Button
            {
                Text = "MC",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + 3 * (buttonWidth + spacing), startY + rowOffset),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightYellow,
                Tag = 32
            };
            buttonMC.Click += ButtonClick;
            buttonPanel.Controls.Add(buttonMC);
        }

        private void CreateModeSpecificButtons()
        {
            int buttonWidth = 70;
            int buttonHeight = 40;
            int spacing = 5;
            int startX = 5;
            int startY = 400;

            labelBase = new Label
            {
                Text = "Основание:",
                Location = new Point(startX, startY),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };
            buttonPanel.Controls.Add(labelBase);

            comboBase = new ComboBox
            {
                Location = new Point(startX + 90, startY),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10),
                Visible = false
            };
            for (int i = 2; i <= 16; i++)
            {
                comboBase.Items.Add(i.ToString());
            }
            comboBase.SelectedIndex = 8;
            comboBase.SelectedIndexChanged += ComboBase_SelectedIndexChanged;
            buttonPanel.Controls.Add(comboBase);

            buttonBaseApply = new Button
            {
                Text = "Применить",
                Location = new Point(startX + 200, startY),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightGreen,
                Tag = "applyBase",
                Visible = false
            };
            buttonBaseApply.Click += BaseApply_Click;
            buttonPanel.Controls.Add(buttonBaseApply);

            int fracY = startY + 40;

            buttonFracSimplify = new Button
            {
                Text = "Сократить",
                Location = new Point(startX, fracY),
                Size = new Size(90, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightPink,
                Tag = "fracSimplify",
                Visible = false
            };
            buttonFracSimplify.Click += FracButton_Click;
            buttonPanel.Controls.Add(buttonFracSimplify);

            buttonFracMixed = new Button
            {
                Text = "Смешанная",
                Location = new Point(startX + 95, fracY),
                Size = new Size(110, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightPink,
                Tag = "fracMixed",
                Visible = false
            };
            buttonFracMixed.Click += FracButton_Click;
            buttonPanel.Controls.Add(buttonFracMixed);

            buttonFracProper = new Button
            {
                Text = "Правильная",
                Location = new Point(startX + 210, fracY),
                Size = new Size(110, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightPink,
                Tag = "fracProper",
                Visible = false
            };
            buttonFracProper.Click += FracButton_Click;
            buttonPanel.Controls.Add(buttonFracProper);

            buttonFracReciprocal = new Button
            {
                Text = "Обратная",
                Location = new Point(startX + 325, fracY),
                Size = new Size(90, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightPink,
                Tag = "fracReciprocal",
                Visible = false
            };
            buttonFracReciprocal.Click += FracButton_Click;
            buttonPanel.Controls.Add(buttonFracReciprocal);

            int complexY = startY + 40;

            buttonComplexI = new Button
            {
                Text = "i",
                Location = new Point(startX, complexY),
                Size = new Size(50, buttonHeight),
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexI",
                Visible = false
            };
            buttonComplexI.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexI);

            buttonComplexConjugate = new Button
            {
                Text = "Сопр",
                Location = new Point(startX + 55, complexY),
                Size = new Size(60, buttonHeight),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexConjugate",
                Visible = false
            };
            buttonComplexConjugate.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexConjugate);

            buttonComplexReal = new Button
            {
                Text = "Re",
                Location = new Point(startX + 120, complexY),
                Size = new Size(50, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexReal",
                Visible = false
            };
            buttonComplexReal.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexReal);

            buttonComplexImaginary = new Button
            {
                Text = "Im",
                Location = new Point(startX + 175, complexY),
                Size = new Size(50, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexImaginary",
                Visible = false
            };
            buttonComplexImaginary.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexImaginary);

            buttonComplexModulus = new Button
            {
                Text = "|z|",
                Location = new Point(startX + 230, complexY),
                Size = new Size(50, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexModulus",
                Visible = false
            };
            buttonComplexModulus.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexModulus);

            buttonComplexArgument = new Button
            {
                Text = "arg",
                Location = new Point(startX + 285, complexY),
                Size = new Size(50, buttonHeight),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexArgument",
                Visible = false
            };
            buttonComplexArgument.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexArgument);

            buttonComplexSwitchPart = new Button
            {
                Text = "Re<->Im",
                Location = new Point(startX + 340, complexY),
                Size = new Size(70, buttonHeight),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                Tag = "complexSwitch",
                Visible = false
            };
            buttonComplexSwitchPart.Click += ComplexButton_Click;
            buttonPanel.Controls.Add(buttonComplexSwitchPart);
        }

        private void UpdateButtonVisibility()
        {
            if (labelBase != null)
            {
                labelBase.Visible = (_currentMode == CalculatorMode.PNumber);
                comboBase.Visible = (_currentMode == CalculatorMode.PNumber);
                buttonBaseApply.Visible = (_currentMode == CalculatorMode.PNumber);

                buttonFracSimplify.Visible = (_currentMode == CalculatorMode.Fraction);
                buttonFracMixed.Visible = (_currentMode == CalculatorMode.Fraction);
                buttonFracProper.Visible = (_currentMode == CalculatorMode.Fraction);
                buttonFracReciprocal.Visible = (_currentMode == CalculatorMode.Fraction);

                buttonComplexI.Visible = (_currentMode == CalculatorMode.Complex);
                buttonComplexConjugate.Visible = (_currentMode == CalculatorMode.Complex);
                buttonComplexReal.Visible = (_currentMode == CalculatorMode.Complex);
                buttonComplexImaginary.Visible = (_currentMode == CalculatorMode.Complex);
                buttonComplexModulus.Visible = (_currentMode == CalculatorMode.Complex);
                buttonComplexArgument.Visible = (_currentMode == CalculatorMode.Complex);
                buttonComplexSwitchPart.Visible = (_currentMode == CalculatorMode.Complex);
            }
        }

        private void BaseButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int newBase = 10;
                switch (btn.Tag.ToString())
                {
                    case "base2": newBase = 2; break;
                    case "base8": newBase = 8; break;
                    case "base10": newBase = 10; break;
                    case "base16": newBase = 16; break;
                }
                SetBase(newBase);
            }
        }

        private void ComboBase_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void BaseApply_Click(object sender, EventArgs e)
        {
            if (comboBase.SelectedItem != null)
            {
                int newBase = int.Parse(comboBase.SelectedItem.ToString());
                SetBase(newBase);
            }
        }

        private void SetBase(int newBase)
        {
            if (_currentMode == CalculatorMode.PNumber && _calculator != null)
            {
                var method = typeof(TCtrl).GetMethod("SetBase",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);

                if (method != null)
                {
                    string result = method.Invoke(_calculator, new object[] { newBase }) as string;
                    if (result != null)
                    {
                        DisplayText = result;
                    }
                }

                UpdateDisplay();
            }
        }

        private void FracButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string tag = btn.Tag.ToString();
                int commandCode = -1;

                switch (tag)
                {
                    case "fracSimplify": commandCode = 41; break;
                    case "fracMixed": commandCode = 42; break;
                    case "fracProper": commandCode = 43; break;
                    case "fracReciprocal": commandCode = 44; break;
                }

                if (commandCode != -1)
                {
                    ExecuteCommand(commandCode);
                }
            }
        }

        private void ComplexButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string tag = btn.Tag.ToString();
                int commandCode = -1;

                switch (tag)
                {
                    case "complexI": commandCode = 51; break;
                    case "complexConjugate": commandCode = 52; break;
                    case "complexReal": commandCode = 53; break;
                    case "complexImaginary": commandCode = 54; break;
                    case "complexModulus": commandCode = 55; break;
                    case "complexArgument": commandCode = 56; break;
                    case "complexSwitch": commandCode = 57; break;
                }

                if (commandCode != -1)
                {
                    ExecuteCommand(commandCode);
                }
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (displayNumber != null)
            {
                int newFontSize = Math.Max(12, Math.Min(24, this.Width / 25));
                displayNumber.Font = new Font("Consolas", newFontSize, FontStyle.Bold);
            }
        }

        private void InitializeCalculator()
        {
            AEditor editor = CreateEditor();
            TANumber number = CreateNumber();
            TProc processor = new TProc(number.Copy(), number.Copy());
            TMemory memory = new TMemory(number.Copy());

            _calculator = new TCtrl(editor, processor, memory, number);
            UpdateDisplay();
        }

        private AEditor CreateEditor()
        {
            return _currentMode switch
            {
                CalculatorMode.PNumber => new PEditor(_currentBase),
                CalculatorMode.Fraction => new FEditor(),
                CalculatorMode.Complex => new CEditor(),
                _ => new PEditor(10)
            };
        }

        private TANumber CreateNumber()
        {
            return _currentMode switch
            {
                CalculatorMode.PNumber => new TPNumber(0, _currentBase),
                CalculatorMode.Fraction => new TFrac(0, 1),
                CalculatorMode.Complex => new TComp(0, 0),
                _ => new TPNumber(0, 10)
            };
        }

        private void UpdateCalculatorMode()
        {
            menuViewPNumber.Checked = (_currentMode == CalculatorMode.PNumber);
            menuViewFraction.Checked = (_currentMode == CalculatorMode.Fraction);
            menuViewComplex.Checked = (_currentMode == CalculatorMode.Complex);

            InitializeCalculator();
            UpdateDisplay();
        }

        private int GetDigitCommand(char digit)
        {
            return digit switch
            {
                '0' => 10,
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                'A' or 'a' => 11,
                'B' or 'b' => 12,
                'C' or 'c' => 13,
                'D' or 'd' => 14,
                'E' or 'e' => 15,
                'F' or 'f' => 16,
                _ => -1
            };
        }

        private void UpdateDisplay()
        {
            if (_calculator != null)
            {
                DisplayText = _calculator.Editor.Str;
                MemoryStateText = _calculator.Memory.StateString;
            }

            displayNumber.SelectionStart = displayNumber.Text.Length;
            displayNumber.ScrollToCaret();
        }

        private void FormCreate(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int commandCode)
            {
                ExecuteCommand(commandCode);
            }
        }

        private void FormKeyPress(object sender, KeyPressEventArgs e)
        {
            int commandCode = -1;
            char key = e.KeyChar;

            if (char.IsDigit(key))
            {
                commandCode = GetDigitCommand(key);
            }
            else switch (key)
                {
                    case '+': commandCode = 21; break;
                    case '-': commandCode = 22; break;
                    case '*': commandCode = 23; break;
                    case '/': commandCode = 24; break;
                    case '=':
                    case (char)13: commandCode = 27; break;
                    case '.':
                    case ',': commandCode = 17; break;
                    case (char)8: commandCode = 18; break;
                    case 'C':
                    case 'c': commandCode = 28; break;
                    case 'S':
                    case 's': commandCode = 26; break;
                    case 'R':
                    case 'r': commandCode = 25; break;
                }

            if (commandCode != -1)
            {
                ExecuteCommand(commandCode);
                e.Handled = true;
            }
        }

        private void ExecuteCommand(int commandCode)
        {
            try
            {
                string result = _calculator.ExecuteCalculatorCommand(
                    commandCode,
                    ref _clipboardText,
                    ref _memoryStateText
                );

                DisplayText = result;
                MemoryStateText = _memoryStateText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnEditCopy(object sender, EventArgs e) => ExecuteCommand(33);
        private void OnEditPaste(object sender, EventArgs e) => ExecuteCommand(34);
        private void OnEditCut(object sender, EventArgs e) => ExecuteCommand(35);

        private void OnModeChange(object sender, EventArgs e)
        {
            if (sender == menuViewPNumber)
                CurrentMode = CalculatorMode.PNumber;
            else if (sender == menuViewFraction)
                CurrentMode = CalculatorMode.Fraction;
            else if (sender == menuViewComplex)
                CurrentMode = CalculatorMode.Complex;
        }

        private void OnHelpAbout(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Универсальный калькулятор\n" +
                "Версия 2.0\n\n" +
                "Поддерживаемые режимы:\n" +
                "- р-ичные числа (2-16) с выбором основания\n" +
                "- простые дроби (сокращение, смешанные)\n" +
                "- комплексные числа (Re, Im, модуль, аргумент)\n\n" +
                "© 2025",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _calculator?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


namespace Calculator
{
    public static class CalculatorHelp
    {
        public static void ShowHelp()
        {
            string helpText = GetHelpText();
            MessageBox.Show(helpText, "Справка по калькулятору",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowAbout()
        {
            string aboutText = GetAboutText();
            MessageBox.Show(aboutText, "О программе",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string GetHelpText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("=== УНИВЕРСАЛЬНЫЙ КАЛЬКУЛЯТОР ===");
            sb.AppendLine();
            sb.AppendLine("ОБЩИЕ СВЕДЕНИЯ:");
            sb.AppendLine("• Калькулятор поддерживает три режима работы: p-ичные числа, простые дроби и комплексные числа");
            sb.AppendLine("• Переключение режимов осуществляется через меню \"Вид\"");
            sb.AppendLine("• Результат вычислений отображается в верхнем поле");
            sb.AppendLine("• Состояние памяти отображается под полем результата");
            sb.AppendLine();

            sb.AppendLine("РЕЖИМ P-ИЧНЫХ ЧИСЕЛ:");
            sb.AppendLine("• Поддерживаются основания от 2 до 16");
            sb.AppendLine("• Для ввода цифр A-F используйте соответствующие кнопки");
            sb.AppendLine("• Десятичный разделитель - точка (.)");
            sb.AppendLine("• Доступны все стандартные операции и функции");
            sb.AppendLine("• Выбор основания осуществляется через выпадающий список");
            sb.AppendLine();

            sb.AppendLine("РЕЖИМ ПРОСТЫХ ДРОБЕЙ:");
            sb.AppendLine("• Дроби вводятся в формате: числитель/знаменатель");
            sb.AppendLine("• Например: 3/4, -5/2, 7/1");
            sb.AppendLine("• Специальные операции:");
            sb.AppendLine("  - Сократить: приводит дробь к несократимому виду");
            sb.AppendLine("  - Смешанная: преобразует неправильную дробь в смешанную");
            sb.AppendLine("  - Правильная: выделяет целую часть из неправильной дроби");
            sb.AppendLine("  - Обратная: меняет числитель и знаменатель местами");
            sb.AppendLine();

            sb.AppendLine("РЕЖИМ КОМПЛЕКСНЫХ ЧИСЕЛ:");
            sb.AppendLine("• Числа вводятся в формате: a+bi или a-bi");
            sb.AppendLine("• Например: 3+2i, -1-4i, 5i, -3i");
            sb.AppendLine("• Кнопка i добавляет мнимую единицу");
            sb.AppendLine("• Специальные операции:");
            sb.AppendLine("  - Сопряжённое: меняет знак мнимой части");
            sb.AppendLine("  - Re: извлекает действительную часть");
            sb.AppendLine("  - Im: извлекает мнимую часть");
            sb.AppendLine("  - |z|: вычисляет модуль комплексного числа");
            sb.AppendLine("  - arg: вычисляет аргумент в градусах");
            sb.AppendLine();

            sb.AppendLine("ПАМЯТЬ:");
            sb.AppendLine("• MS - сохранить текущее значение в память");
            sb.AppendLine("• MR - извлечь значение из памяти");
            sb.AppendLine("• M+ - добавить текущее значение к значению в памяти");
            sb.AppendLine("• MC - очистить память");
            sb.AppendLine("• Индикатор памяти показывает состояние: On/Off");
            sb.AppendLine();

            sb.AppendLine("БУФЕР ОБМЕНА:");
            sb.AppendLine("• Копировать (Ctrl+C) - копирует текущее значение");
            sb.AppendLine("• Вставить (Ctrl+V) - вставляет значение из буфера");
            sb.AppendLine("• Вырезать (Ctrl+X) - копирует и очищает текущее значение");
            sb.AppendLine();

            sb.AppendLine("УПРАВЛЕНИЕ С КЛАВИАТУРЫ:");
            sb.AppendLine("• Цифры 0-9 - ввод цифр");
            sb.AppendLine("• A-F - ввод цифр 10-15 (в p-ичном режиме)");
            sb.AppendLine("• + - сложение");
            sb.AppendLine("• - - вычитание");
            sb.AppendLine("• * - умножение");
            sb.AppendLine("• / - деление");
            sb.AppendLine("• Enter или = - равно");
            sb.AppendLine("• . или , - десятичный разделитель");
            sb.AppendLine("• Backspace - забой");
            sb.AppendLine("• C - очистка");
            sb.AppendLine("• S - квадрат");
            sb.AppendLine("• R - обратное число");

            return sb.ToString();
        }

        private static string GetAboutText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("УНИВЕРСАЛЬНЫЙ КАЛЬКУЛЯТОР");
            sb.AppendLine("Версия 2.0");
            sb.AppendLine();
            sb.AppendLine("Разработчик: ИП-216 Слесаренко");
            sb.AppendLine("Год: 2026");
            sb.AppendLine();
            sb.AppendLine("Поддерживаемые режимы:");
            sb.AppendLine("• p-ичные числа (основания 2-16)");
            sb.AppendLine("• простые дроби");
            sb.AppendLine("• комплексные числа");
            sb.AppendLine();
            sb.AppendLine("Все права защищены.");

            return sb.ToString();
        }
    }
}

