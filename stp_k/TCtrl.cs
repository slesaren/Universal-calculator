using Calculator.Numbers;
using CalculatorMemory;
using CalculatorProcessor;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace Calculator
{
    public enum TCtrlState
    {
        cStart,     
        cEditing,   
        cFunDone,   
        cValDone,   
        cExpDone,   
        cOpChange,  
        cError      
    }

    public class TCtrl : IDisposable
    {
        private const int CMD_TOGGLE_SIGN = 0;
        private const int CMD_DIGIT_1 = 1;
        private const int CMD_DIGIT_2 = 2;
        private const int CMD_DIGIT_3 = 3;
        private const int CMD_DIGIT_4 = 4;
        private const int CMD_DIGIT_5 = 5;
        private const int CMD_DIGIT_6 = 6;
        private const int CMD_DIGIT_7 = 7;
        private const int CMD_DIGIT_8 = 8;
        private const int CMD_DIGIT_9 = 9;
        private const int CMD_DIGIT_0 = 10;
        private const int CMD_DIGIT_A = 11;
        private const int CMD_DIGIT_B = 12;
        private const int CMD_DIGIT_C = 13;
        private const int CMD_DIGIT_D = 14;
        private const int CMD_DIGIT_E = 15;
        private const int CMD_DIGIT_F = 16;
        private const int CMD_SEPARATOR = 17;
        private const int CMD_BACKSPACE = 18;
        private const int CMD_EDITOR_CLEAR = 19;

        private const int CMD_OP_ADD = 21;
        private const int CMD_OP_SUB = 22;
        private const int CMD_OP_MUL = 23;
        private const int CMD_OP_DIV = 24;
        private const int CMD_FUNC_REV = 25;
        private const int CMD_FUNC_SQR = 26;
        private const int CMD_EQUAL = 27;
        private const int CMD_RESET = 28;

        private const int CMD_MEM_MS = 29;
        private const int CMD_MEM_MR = 30;
        private const int CMD_MEM_MPLUS = 31;
        private const int CMD_MEM_MC = 32;

        private const int CMD_CLIP_COPY = 33;
        private const int CMD_CLIP_PASTE = 34;
        private const int CMD_CLIP_CUT = 35;

  
        private const int CMD_FRAC_SIMPLIFY = 41;
        private const int CMD_FRAC_MIXED = 42;
        private const int CMD_FRAC_PROPER = 43;
        private const int CMD_FRAC_RECIPROCAL = 44;


        private const int CMD_COMPLEX_I = 51;
        private const int CMD_COMPLEX_CONJUGATE = 52;
        private const int CMD_COMPLEX_REAL = 53;
        private const int CMD_COMPLEX_IMAGINARY = 54;
        private const int CMD_COMPLEX_MODULUS = 55;
        private const int CMD_COMPLEX_ARGUMENT = 56;
        private const int CMD_COMPLEX_SWITCH = 57;





       
        private TCtrlState _state;
        private AEditor _editor;
        private TProc _processor;
        private TMemory _memory;
        private TANumber _number;

        private int _currentBase = 10;
        private CalculatorMode _currentMode;

        private TANumber _lastRightOperand = null;

        private TOprtn _lastOperation = TOprtn.None;

        public TCtrlState State
        {
            get => _state;
            set => _state = value;
        }

        public AEditor Editor => _editor;
        public TProc Processor => _processor;
        public TMemory Memory => _memory;
        public TANumber Number => _number?.Copy();

        public string StateString
        {
            get
            {
                return _state switch
                {
                    TCtrlState.cStart => "Start",
                    TCtrlState.cEditing => "Editing",
                    TCtrlState.cFunDone => "FunDone",
                    TCtrlState.cValDone => "ValDone",
                    TCtrlState.cExpDone => "ExpDone",
                    TCtrlState.cOpChange => "OpChange",
                    TCtrlState.cError => "Error",
                    _ => "Unknown"
                };
            }
        }

  
        public TCtrl(AEditor editor, TProc processor, TMemory memory, TANumber number)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _number = number?.Copy() ?? throw new ArgumentNullException(nameof(number));
            _state = TCtrlState.cStart;
        }

    
        public void Dispose()
        {
    
            _number = null;
            _editor = null;
            _processor = null;
            _memory = null;

      
            GC.SuppressFinalize(this);
        }

      
        public string ExecuteCalculatorCommand(int commandCode, ref string clipboardText, ref string memoryState)
        {
            Console.WriteLine($"Executing command: {commandCode}");
            string result = "";

            try
            {
           
                if (commandCode >= 0 && commandCode <= 20) 
                {
                    result = ExecuteEditorCommand(commandCode);
                }
                else if (commandCode >= 21 && commandCode <= 24) 
                {
                    result = ExecuteOperation(commandCode);
                }
                else if (commandCode >= 25 && commandCode <= 26) 
                {
                    result = ExecuteFunction(commandCode);
                }
                else if (commandCode == 27) 
                {
                    result = CalculateExpression(commandCode);
                }
                else if (commandCode == 28) 
                {
                    result = SetInitialState(commandCode);
                }
                else if (commandCode >= 29 && commandCode <= 32) 
                {
                    result = ExecuteMemoryCommand(commandCode, ref memoryState);
                }
                else if (commandCode >= 33 && commandCode <= 35) 
                {
                    result = ExecuteClipboardCommand(commandCode, ref clipboardText);
                }
    
                else if (commandCode >= 41 && commandCode <= 44)
                {
                    result = ExecuteFractionCommand(commandCode);
                }
            
                else if (commandCode >= 51 && commandCode <= 57)
                {
                    result = ExecuteComplexCommand(commandCode);
                }
                else
                {
                    _state = TCtrlState.cError;
                    result = "Неизвестная команда";
                }

                Console.WriteLine($"Command result: {result}, State: {_state}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                _state = TCtrlState.cError;
                result = $"Ошибка: {ex.Message}";
            }

            return result;
        }

        private string ExecuteFractionCommand(int commandCode)
        {
            string result = "";

            if (!(_editor is FEditor))
            {
                _state = TCtrlState.cError;
                return "Не в режиме дробей";
            }

            try
            {
                TANumber current = CreateNumberFromEditor();

                if (current is TFrac frac)
                {
                    switch (commandCode)
                    {
                        case CMD_FRAC_SIMPLIFY: 
                                               
                            result = frac.Str;
                            break;

                        case CMD_FRAC_MIXED: 
                            result = ConvertToMixedFraction(frac);
                            break;

                        case CMD_FRAC_PROPER: 
                            result = ConvertToProperFraction(frac);
                            break;

                        case CMD_FRAC_RECIPROCAL: 
                            if (frac.IsZero())
                            {
                                _state = TCtrlState.cError;
                                return "Деление на ноль";
                            }
                            var inverse = frac.Inverse();
                            if (inverse is TFrac invFrac)
                            {
                                _editor.Str = invFrac.Str;
                                result = invFrac.Str;
                                _state = TCtrlState.cFunDone;
                            }
                            break;
                    }

                    if (!string.IsNullOrEmpty(result) && commandCode != CMD_FRAC_RECIPROCAL)
                    {
                        _editor.Str = result;
                        _state = TCtrlState.cFunDone;
                    }
                }
                else
                {
                    _state = TCtrlState.cError;
                    return "Текущее число не является дробью";
                }
            }
            catch (Exception ex)
            {
                _state = TCtrlState.cError;
                return $"Ошибка: {ex.Message}";
            }

            return string.IsNullOrEmpty(result) ? _editor.Str : result;
        }

       

        private string ConvertToMixedFraction(TFrac frac)
        {
            long num = frac.Numerator;
            long den = frac.Denominator;

            if (den == 0) return frac.Str;

            long wholePart = num / den;
            long remainder = Math.Abs(num % den);
            long absDen = Math.Abs(den);

            if (remainder == 0)
                return wholePart.ToString();
            else if (wholePart == 0)
                return frac.Str;
            else
                return $"{wholePart} {remainder}/{absDen}";
        }

        private string ConvertToProperFraction(TFrac frac)
        {
            long num = frac.Numerator;
            long den = frac.Denominator;

            if (Math.Abs(num) < Math.Abs(den))
                return frac.Str;

            long wholePart = num / den;
            long remainder = Math.Abs(num % den);
            long absDen = Math.Abs(den);

            if (remainder == 0)
                return $"{wholePart}";
            else if (wholePart == 0)
                return frac.Str;
            else
            {
                long newNum = remainder;
                long newDen = absDen;
                if (num < 0)
                    newNum = -newNum;
                return $"{newNum}/{newDen}";
            }
        }


        private string ExecuteComplexCommand(int commandCode)
        {
            string result = "";

     
            if (!(_editor is CEditor))
            {
                _state = TCtrlState.cError;
                return "Не в режиме комплексных чисел";
            }

            try
            {
                TANumber current = CreateNumberFromEditor();

                if (current is TComp comp)
                {
                    switch (commandCode)
                    {
                        
                        case CMD_COMPLEX_I: 
                            if (_editor is CEditor cEditor)
                            {
                                _editor.Str = cEditor.AddI(); 
                                result = _editor.Str;
                                _state = TCtrlState.cEditing;
                            }
                            break;

                        case CMD_COMPLEX_CONJUGATE: 
                            var conj = new TComp(comp.Re, -comp.Im);
                            _editor.Str = conj.Str;
                            result = conj.Str;
                            _number = conj.Copy();
                            _state = TCtrlState.cFunDone;
                            break;

                        case CMD_COMPLEX_REAL: 
                            result = comp.Re.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            _editor.Str = result;
                            _number = new TPNumber(comp.Re, 10);
                            _state = TCtrlState.cFunDone;
                            break;

                        case CMD_COMPLEX_IMAGINARY:
                            result = comp.Im.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            if (comp.Im >= 0)
                                result = result + "i";
                            else
                                result = result + "i";
                            _editor.Str = result;
                            _number = new TPNumber(comp.Im, 10);
                            _state = TCtrlState.cFunDone;
                            break;

                        case CMD_COMPLEX_MODULUS: 
                            double modulus = Math.Sqrt(comp.Re * comp.Re + comp.Im * comp.Im);
                            result = modulus.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            _editor.Str = result;
                            _number = new TPNumber(modulus, 10);
                            _state = TCtrlState.cFunDone;
                            break;

                        case CMD_COMPLEX_ARGUMENT: 
                            double arg = Math.Atan2(comp.Im, comp.Re);
                            double argDegrees = arg * 180 / Math.PI;
                            result = argDegrees.ToString("F2") + "°";
                            _editor.Str = result;
                            _number = new TPNumber(argDegrees, 10);
                            _state = TCtrlState.cFunDone;
                            break;

                        case CMD_COMPLEX_SWITCH: 
                            if (_editor is CEditor swEditor)
                            {

                                result = _editor.Str;
                            }
                            break;
                    }
                }
                else
                {
                    _state = TCtrlState.cError;
                    return "Текущее число не является комплексным";
                }
            }
            catch (Exception ex)
            {
                _state = TCtrlState.cError;
                return $"Ошибка: {ex.Message}";
            }

            return string.IsNullOrEmpty(result) ? _editor.Str : result;
        }




        public void SetBase(int newBase)
        {
            if (_editor is PEditor pEditor)
            {
                try
                {
                    string currentValue = _editor.Str;
                    pEditor.Base = newBase;

                    if (!string.IsNullOrEmpty(currentValue) && currentValue != "0")
                    {
                        try
                        {
                            var tempNumber = new TPNumber(currentValue, _currentBase);
                            var newNumber = new TPNumber(tempNumber.Value, newBase);
                            _editor.Str = newNumber.Str;
                        }
                        catch
                        {
                            _editor.Clear();
                        }
                    }

                    _currentBase = newBase;
                    _state = TCtrlState.cEditing;
                }
                catch (Exception ex)
                {
                    _state = TCtrlState.cError;
                    _errorMessage = $"Ошибка при изменении основания: {ex.Message}";
                }
            }
        }
        private string _errorMessage = "";

        


        public string ExecuteEditorCommand(int commandCode)
        {
            string result = "";

            bool isDigit = (commandCode >= 1 && commandCode <= 10) || (commandCode >= 11 && commandCode <= 16);
            if (isDigit && (_state == TCtrlState.cValDone || _state == TCtrlState.cExpDone ||
                            _state == TCtrlState.cFunDone || _state == TCtrlState.cOpChange))
            {
                _editor.Clear();
            }

            result = _editor.Edit(commandCode);

            if (isDigit || commandCode == 17)
            {
                _state = TCtrlState.cEditing;
            }

            return result;
        }

        

        public string ExecuteOperation(int commandCode)
        {
            TOprtn operation = MapCommandToOperation(commandCode);
            TANumber currentValue;
            if (_state == TCtrlState.cEditing)
                currentValue = CreateNumberFromEditor();
            else
                currentValue = _number?.Copy() ?? CreateNumberFromEditor();

            TOprtn pendingOp = _processor.Operation;

            if (pendingOp != TOprtn.None)
            {
                _processor.RopSet(currentValue);
                _processor.OprtnRun();

                if (_processor.HasError)
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }

                _number = _processor.LopResGet();
            }
            else
            {
                _processor.LopResSet(currentValue);
                _number = currentValue.Copy();
            }

            _processor.OprtnSet(operation);

            _state = TCtrlState.cValDone;
            _lastOperation = TOprtn.None;
            _lastRightOperand = null;

            return _number.Str;
        }




        public string ExecuteFunction(int commandCode)
        {
            TFunc function = MapCommandToFunction(commandCode);
            TANumber target = null;

            if (_state == TCtrlState.cEditing || _state == TCtrlState.cValDone)
                target = CreateNumberFromEditor();
            else if (_state == TCtrlState.cExpDone || _state == TCtrlState.cFunDone)
                target = _number?.Copy();

            if (target != null)
            {
                _processor.RopSet(target);
                _processor.FuncRun(function);

                if (_processor.HasError)
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }

                var funcResult = _processor.RopGet();
                _editor.Str = funcResult.Str;
                _number = funcResult.Copy();
                _state = TCtrlState.cFunDone;

                _lastOperation = TOprtn.None;
                _lastRightOperand = null;

                return funcResult.Str;
            }
            return _editor.Str;
        }



        public string CalculateExpression(int commandCode)
        {
            TANumber currentValue;
            if (_state == TCtrlState.cEditing)
                currentValue = CreateNumberFromEditor();
            else
                currentValue = _number?.Copy() ?? CreateNumberFromEditor();

            TOprtn pendingOp = _processor.Operation;
            string result;

            if (pendingOp == TOprtn.None && _lastOperation != TOprtn.None && _lastRightOperand != null)
            {
                _processor.LopResSet(currentValue);
                _processor.RopSet(_lastRightOperand);
                _processor.OprtnSet(_lastOperation);
                _processor.OprtnRun();

                if (_processor.HasError)
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }

                _number = _processor.LopResGet();
                _processor.OprtnClear(); 

                _state = TCtrlState.cExpDone;
                _editor.Str = _number.Str;
                result = _number.Str;
            }
            else if (pendingOp != TOprtn.None)
            {
                _processor.RopSet(currentValue);
                _processor.OprtnRun();

                if (_processor.HasError)
                {
                    _state = TCtrlState.cError;
                    return _processor.Error;
                }

                _number = _processor.LopResGet();
                _lastOperation = pendingOp;
                _lastRightOperand = currentValue.Copy();

                _processor.OprtnClear(); 
                _state = TCtrlState.cExpDone;
                _editor.Str = _number.Str;
                result = _number.Str;
            }
            else
            {
                result = currentValue.Str;
            }

            return result;
        }

        

        public string SetInitialState(int commandCode)
        {
            _editor.Clear();
            _processor.ReSet();
            _number = CreateNumberFromString("0");
            _state = TCtrlState.cStart;

            _lastOperation = TOprtn.None;
            _lastRightOperand = null;

            return "0";
        }

        
        public string ExecuteMemoryCommand(int commandCode, ref string memoryState)
        {
            string result = "";

            switch (commandCode)
            {
                case 29: 
                    if (!string.IsNullOrEmpty(_editor.Str))
                    {
                        var number = CreateNumberFromEditor();
                        _memory.Store(number);
                        _state = TCtrlState.cValDone;
                    }
                    break;

                case 30: 
                    if (_memory.IsOn)
                    {
                        var memValue = _memory.Retrieve();
                        _editor.Str = memValue.Str;
                        result = memValue.Str;
                        _state = TCtrlState.cEditing; 
                    }
                    break;

                case 31:
                    if (!string.IsNullOrEmpty(_editor.Str))
                    {
                        var number = CreateNumberFromEditor();
                        _memory.Add(number);
                        _state = TCtrlState.cValDone; 
                    }
                    break;

                case 32: 
                    if (_memory.IsOn)
                    {
                        _memory.Clear();
                    }
                    break;
            }

            memoryState = _memory.StateString;
            if (string.IsNullOrEmpty(result))
                result = _editor.Str;
            return result;
        }


        public string ExecuteClipboardCommand(int commandCode, ref string clipboardText)
        {
            string result = _editor.Str;

            switch (commandCode)
            {
                case 33: 
                    clipboardText = _editor.Str;
                    if (!string.IsNullOrEmpty(clipboardText))
                    {
                        Clipboard.SetText(clipboardText);
                    }
                    break;

                case 34: 
                    if (!string.IsNullOrEmpty(clipboardText) || Clipboard.ContainsText())
                    {
                        string text = clipboardText ?? Clipboard.GetText();
                        _editor.Str = text;
                        result = text;
                        _state = TCtrlState.cEditing;
                    }
                    break;

                case 35: 
                    clipboardText = _editor.Str;
                    if (!string.IsNullOrEmpty(clipboardText))
                    {
                        Clipboard.SetText(clipboardText);
                        _editor.Clear();
                        result = "0";
                        _state = TCtrlState.cStart;
                    }
                    break;
            }

            return result;
        }

 

        private TOprtn MapCommandToOperation(int commandCode)
        {
            return commandCode switch
            {
                21 => TOprtn.Add, 
                22 => TOprtn.Sub, 
                23 => TOprtn.Mul, 
                24 => TOprtn.Dvd, 
                _ => TOprtn.None
            };
        }

        private TFunc MapCommandToFunction(int commandCode)
        {
            return commandCode switch
            {
                25 => TFunc.Rev, 
                26 => TFunc.Sqr, 
                _ => TFunc.Rev
            };
        }

      

        private TANumber CreateNumberFromEditor()
        {
            if (_editor == null || string.IsNullOrEmpty(_editor.Str))
                return CreateNumberFromString("0");

            try
            {
 
                if (_editor is PEditor pEditor)
                {
                    return new TPNumber(_editor.Str, pEditor.Base);
                }
                else if (_editor is FEditor fEditor)
                {
                    if (fEditor.TryGetFraction(out long num, out long den))
                        return new TFrac(num, den);
                    return new TFrac(0, 1); 
                }
                else if (_editor is CEditor cEditor)
                {
                    string str = _editor.Str;
                    double re = 0, im = 0;

                    str = str.Replace(" ", "").ToLower();
                    if (str.Contains("+") || (str.Contains("-") && str.LastIndexOf('-') > 0))
                    {
                        int opIndex = str.LastIndexOfAny(new[] { '+', '-' });
                        if (opIndex > 0)
                        {
                            string rePart = str.Substring(0, opIndex);
                            string imPart = str.Substring(opIndex).Replace("i", "");
                            double.TryParse(rePart, out re);
                            double.TryParse(imPart, out im);
                        }
                    }
                    else if (str.Contains("i"))
                    {
                        string imPart = str.Replace("i", "");
                        if (string.IsNullOrEmpty(imPart) || imPart == "+" || imPart == "-")
                            im = imPart == "-" ? -1 : 1;
                        else
                            double.TryParse(imPart, out im);
                    }
                    else
                    {
                        double.TryParse(str, out re);
                    }

                    return new TComp(re, im);
                }
                else
                {
                    return new TPNumber(0, 10);
                }
            }
            catch
            {
                return CreateNumberFromString("0");
            }
        }

        private TANumber CreateNumberFromString(string str)
        {
            if (double.TryParse(str, out double value))
            {
                return new TPNumber(value, 10);
            }
            return new TPNumber(0, 10);
        }

        public TCtrlState ReadState() => _state;
        public void WriteState(TCtrlState state) => _state = state;



    }
}
