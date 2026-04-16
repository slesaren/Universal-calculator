using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Calculator
{
    public abstract class AEditor
    {
        protected string _str;

        public virtual string Str
        {
            get => _str;
            set => _str = value ?? "";
        }

        public abstract bool IsZero { get; }
        public abstract string ToggleSign();
        public abstract string AddDigit(int digit);
        public abstract string AddZero();
        public abstract string AddSeparator();
        public abstract string Backspace();
        public abstract string Clear();
        public virtual string ReadStr() => _str;
        public virtual void WriteStr(string value) => _str = value;

        public virtual string Edit(int commandCode)
        {
            return commandCode switch
            {
                0 => ToggleSign(),
                1 => AddDigit(1),
                2 => AddDigit(2),
                3 => AddDigit(3),
                4 => AddDigit(4),
                5 => AddDigit(5),
                6 => AddDigit(6),
                7 => AddDigit(7),
                8 => AddDigit(8),
                9 => AddDigit(9),
                10 => AddDigit(0),
                11 => AddDigit(10),
                12 => AddDigit(11),
                13 => AddDigit(12),
                14 => AddDigit(13),
                15 => AddDigit(14),
                16 => AddDigit(15),
                17 => AddSeparator(),
                18 => Backspace(),
                19 => Clear(),
                _ => _str
            };
        }

        protected abstract bool CanAddSymbol(char symbol);

        protected string ReplaceLeadingZero(string currentStr, char newChar)
        {
            if (string.IsNullOrEmpty(currentStr)) return newChar.ToString();

            if (currentStr == "0" || currentStr == "00" || currentStr == "000")
            {
                return newChar.ToString();
            }

            if (currentStr == "-0" || currentStr == "-00" || currentStr == "-000")
            {
                return "-" + newChar;
            }

            return currentStr + newChar;
        }
    }
}

namespace Calculator
{
    public class PEditor : AEditor
    {
        private int _base;

        public int Base
        {
            get => _base;
            set
            {
                if (value < 2 || value > 16)
                    throw new ArgumentException("Основание должно быть от 2 до 16");
                _base = value;
                ValidateCurrentString();
            }
        }

        public PEditor(int numBase)
        {
            Base = numBase;
            Clear();
        }

        public override bool IsZero
        {
            get
            {
                if (string.IsNullOrEmpty(_str)) return true;
                string testStr = _str.TrimStart('-');
                if (testStr == "0" || testStr == "") return true;
                if (testStr.Contains("."))
                {
                    string[] parts = testStr.Split('.');
                    if (parts.Length == 2)
                    {
                        bool intPartZero = string.IsNullOrEmpty(parts[0]) || parts[0] == "0" || parts[0].TrimStart('0') == "";
                        bool fracPartZero = string.IsNullOrEmpty(parts[1]) || parts[1].TrimEnd('0') == "";

                        if (intPartZero && fracPartZero)
                            return true;
                    }
                }
                return false;
            }
        }

        public override string ToggleSign()
        {
            if (string.IsNullOrEmpty(_str) || IsZero)
                return _str;

            if (_str.StartsWith("-"))
                _str = _str.Substring(1);
            else
                _str = "-" + _str;

            return _str;
        }

        public override string AddDigit(int digit)
        {
            if (digit < 0 || digit >= _base)
                return _str;

            char digitChar = DigitToChar(digit);

            if (!CanAddSymbol(digitChar))
                return _str;

            if (_str == "0" || _str == "-0")
            {
                if (digit == 0)
                {
                    return _str;
                }
                else
                {
                    _str = ReplaceLeadingZero(_str, digitChar);
                    return _str;
                }
            }
            else if (_str == "00" || _str == "-00")
            {
                if (digit == 0)
                {
                    _str = _str.StartsWith("-") ? "-0" : "0";
                    return _str;
                }
            }

            _str += digitChar;
            return _str;
        }

        public override string AddZero()
        {
            return AddDigit(0);
        }

        public override string AddSeparator()
        {
            char separator = '.';

            if (!CanAddSymbol(separator))
                return _str;

            _str += separator;
            return _str;
        }

        public override string Backspace()
        {
            if (!string.IsNullOrEmpty(_str))
            {
                if (_str.Length == 1)
                {
                    _str = "0";
                }
                else if (_str.Length == 2 && _str[0] == '-')
                {
                    _str = "0";
                }
                else
                {
                    _str = _str.Substring(0, _str.Length - 1);
                }
            }
            return _str;
        }

        public override string Clear()
        {
            _str = "0";
            return _str;
        }

        protected override bool CanAddSymbol(char symbol)
        {
            if (symbol == '-') return false;

            if (symbol == '.')
            {
                if (_str.Contains(".")) return false;
                if (_str == "-") return false;
                if (_str == "" || _str == "-") return false;
                return true;
            }

            int digit = CharToDigit(symbol);
            if (digit < 0 || digit >= _base) return false;

            if (_str == "-") return false;

            return true;
        }

        private void ValidateCurrentString()
        {
            if (string.IsNullOrEmpty(_str))
            {
                _str = "0";
                return;
            }

            if (_str.StartsWith("00") && !_str.StartsWith("0."))
            {
                _str = _str.TrimStart('0');
                if (string.IsNullOrEmpty(_str)) _str = "0";
            }
            else if (_str.StartsWith("-00") && !_str.StartsWith("-0."))
            {
                _str = "-" + _str.Substring(2).TrimStart('0');
                if (_str == "-") _str = "-0";
            }
        }

        private char DigitToChar(int digit)
        {
            if (digit < 10) return (char)('0' + digit);
            return (char)('A' + (digit - 10));
        }

        private int CharToDigit(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return 10 + (c - 'A');
            if (c >= 'a' && c <= 'f') return 10 + (c - 'a');
            return -1;
        }
    }
}

namespace Calculator
{
    public class FEditor : AEditor
    {
        public FEditor()
        {
            Clear();
        }

        public override bool IsZero
        {
            get
            {
                if (string.IsNullOrEmpty(_str)) return true;

                if (_str == "0" || _str == "-0") return true;

                if (_str.Contains("/"))
                {
                    string[] parts = _str.Split('/');
                    if (parts.Length == 2)
                    {
                        string numerator = parts[0];
                        if (numerator == "0" || numerator == "-0") return true;
                        if (long.TryParse(numerator, out long num) && num == 0)
                            return true;
                    }
                }

                return false;
            }
        }

        public override string ToggleSign()
        {
            if (string.IsNullOrEmpty(_str) || IsZero)
                return _str;

            if (_str.StartsWith("-"))
                _str = _str.Substring(1);
            else
                _str = "-" + _str;

            return _str;
        }

        public override string AddDigit(int digit)
        {
            if (digit < 0 || digit > 9)
                return _str;

            char digitChar = (char)('0' + digit);

            if (!CanAddSymbol(digitChar))
                return _str;

            if (digit != 0)
            {
                if (!_str.Contains("/"))
                {
                    if (_str == "0" || _str == "-0" || _str == "00" || _str == "-00")
                    {
                        _str = ReplaceLeadingZero(_str, digitChar);
                        return _str;
                    }
                }
                else
                {
                    string[] parts = _str.Split('/');
                    if (parts.Length == 2)
                    {
                        string denominator = parts[1];
                        if (denominator == "0" || denominator == "00")
                        {
                            _str = parts[0] + "/" + digitChar;
                            return _str;
                        }
                    }
                }
            }

            _str += digitChar;
            return _str;
        }

        public override string AddZero()
        {
            return AddDigit(0);
        }

        public override string AddSeparator()
        {
            char separator = '/';

            if (!CanAddSymbol(separator))
                return _str;

            _str += separator;
            return _str;
        }

        public override string Backspace()
        {
            if (!string.IsNullOrEmpty(_str))
            {
                if (_str.Length == 1)
                {
                    _str = "0";
                }
                else if (_str.Length == 2 && _str[0] == '-')
                {
                    _str = "0";
                }
                else
                {
                    _str = _str.Substring(0, _str.Length - 1);

                    if (_str == "-" || string.IsNullOrEmpty(_str))
                        _str = "0";
                }
            }
            return _str;
        }

        public override string Clear()
        {
            _str = "0";
            return _str;
        }

        protected override bool CanAddSymbol(char symbol)
        {
            if (symbol == '-') return false;

            if (symbol == '/')
            {
                if (_str.Contains("/")) return false;

                if (string.IsNullOrEmpty(_str) || _str == "-") return false;

                string numerator = _str.TrimStart('-');
                if (string.IsNullOrEmpty(numerator) || numerator == "0") return false;

                return true;
            }

            if (symbol < '0' || symbol > '9') return false;

            if (!_str.Contains("/"))
            {
                string numPart = _str.TrimStart('-');
                if (numPart == "0" && symbol == '0')
                    return false;
            }
            else
            {
                string[] parts = _str.Split('/');
                if (parts.Length == 2)
                {
                    string denomPart = parts[1];
                    if (denomPart == "0" && symbol == '0')
                        return false;
                }
            }

            return true;
        }

        public bool TryGetFraction(out long numerator, out long denominator)
        {
            numerator = 0;
            denominator = 1;

            if (string.IsNullOrEmpty(_str))
                return false;

            if (_str.Contains("/"))
            {
                string[] parts = _str.Split('/');
                if (parts.Length == 2 &&
                    long.TryParse(parts[0], out numerator) &&
                    long.TryParse(parts[1], out denominator) &&
                    denominator != 0)
                {
                    return true;
                }
            }
            else
            {
                if (long.TryParse(_str, out numerator))
                {
                    denominator = 1;
                    return true;
                }
            }

            return false;
        }

        public void SetFraction(long numerator, long denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Знаменатель не может быть нулем");

            _str = $"{numerator}/{denominator}";
        }
    }
}

namespace Calculator
{
    public class CEditor : AEditor
    {
        private bool _editingImaginaryPart = false;

        public CEditor()
        {
            Clear();
        }

        public override bool IsZero
        {
            get
            {
                if (string.IsNullOrEmpty(_str)) return true;

                if (_str == "0" || _str == "-0") return true;

                if (_str == "0+0i" || _str == "0-0i") return true;

                if (_str == "0i" || _str == "-0i") return true;

                if (_str == "+0i") return true;

                return false;
            }
        }

        public override string ToggleSign()
        {
            if (string.IsNullOrEmpty(_str) || IsZero)
                return _str;

            if (_editingImaginaryPart)
            {
                int operatorIndex = FindOperatorIndex();
                if (operatorIndex > 0)
                {
                    char currentOp = _str[operatorIndex];
                    char newOp = currentOp == '+' ? '-' : '+';
                    _str = _str.Substring(0, operatorIndex) + newOp + _str.Substring(operatorIndex + 1);
                }
                else
                {
                    if (_str.Contains("i"))
                    {
                        if (_str.StartsWith("-"))
                            _str = _str.Substring(1);
                        else
                            _str = "-" + _str;
                    }
                }
            }
            else
            {
                if (_str.StartsWith("-"))
                    _str = _str.Substring(1);
                else
                    _str = "-" + _str;
            }

            return _str;
        }

        private int FindOperatorIndex()
        {
            for (int i = 1; i < _str.Length; i++)
            {
                if (_str[i] == '+' || _str[i] == '-')
                    return i;
            }
            return -1;
        }

        private int FindLastOperatorIndex()
        {
            int lastOpIndex = -1;
            for (int i = 1; i < _str.Length; i++)
            {
                if (_str[i] == '+' || _str[i] == '-')
                    lastOpIndex = i;
            }
            return lastOpIndex;
        }

        public override string AddDigit(int digit)
        {
            if (digit < 0 || digit > 9)
                return _str;

            char digitChar = (char)('0' + digit);

            if (!CanAddSymbol(digitChar))
                return _str;

            if (!_editingImaginaryPart)
            {
                int operatorIndex = FindOperatorIndex();
                string realPart = operatorIndex > 0 ? _str.Substring(0, operatorIndex) : _str;

                if (realPart == "0" || realPart == "-0" || realPart == "00" || realPart == "-00")
                {
                    string sign = realPart.StartsWith("-") ? "-" : "";
                    string newRealPart = sign + digitChar;

                    if (operatorIndex > 0)
                        _str = newRealPart + _str.Substring(operatorIndex);
                    else
                        _str = newRealPart;

                    return _str;
                }
            }
            else
            {
                int lastOpIndex = FindLastOperatorIndex();
                if (lastOpIndex > 0)
                {
                    string imaginaryPart = _str.Substring(lastOpIndex + 1);
                    if (imaginaryPart == "0" || imaginaryPart == "-0" || imaginaryPart == "00" || imaginaryPart == "-00")
                    {
                        string sign = imaginaryPart.StartsWith("-") ? "-" : "";
                        _str = _str.Substring(0, lastOpIndex + 1) + sign + digitChar;
                        return _str;
                    }
                }
            }

            _str += digitChar;
            return _str;
        }

        public override string AddZero()
        {
            return AddDigit(0);
        }

        public override string AddSeparator()
        {
            char separator = '+';

            if (!CanAddSymbol(separator))
                return _str;

            _str += separator;
            _editingImaginaryPart = true;
            return _str;
        }

        public override string Backspace()
        {
            if (!string.IsNullOrEmpty(_str))
            {
                char lastChar = _str[_str.Length - 1];
                _str = _str.Substring(0, _str.Length - 1);

                if (lastChar == 'i')
                    _editingImaginaryPart = false;

                if (lastChar == '+' || lastChar == '-')
                    _editingImaginaryPart = false;

                if (string.IsNullOrEmpty(_str) || _str == "-")
                    _str = "0";
            }
            return _str;
        }

        public override string Clear()
        {
            _str = "0";
            _editingImaginaryPart = false;
            return _str;
        }

        protected override bool CanAddSymbol(char symbol)
        {
            if (symbol == '-') return false;

            if (symbol == 'i' || symbol == 'I')
            {
                if (_str.Contains("i")) return false;

                if (string.IsNullOrEmpty(_str) || _str == "-") return false;

                int lastOpIndex = FindLastOperatorIndex();
                if (lastOpIndex > 0 && lastOpIndex == _str.Length - 1)
                    return false;

                _editingImaginaryPart = true;
                return true;
            }

            if (symbol == '+')
            {
                if (string.IsNullOrEmpty(_str) || _str == "-") return false;

                if (FindOperatorIndex() > 0) return false;

                if (_str.Contains("i")) return false;

                _editingImaginaryPart = true;
                return true;
            }

            if (symbol < '0' || symbol > '9') return false;

            return true;
        }

        public string SwitchPart()
        {
            if (_str.Contains("i"))
            {
                _editingImaginaryPart = !_editingImaginaryPart;
            }
            return _str;
        }

        public string AddI()
        {
            if (!CanAddSymbol('i'))
                return _str;

            _str += 'i';
            _editingImaginaryPart = true;
            return _str;
        }
    }
}



