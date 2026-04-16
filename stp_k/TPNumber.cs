using Calculator.Numbers;
using System;
// TPNumber.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace Calculator.Numbers
{
    public class TPNumber : TANumber
    {
        private double _value;
        private int _base;
        private string _strValue;

        private const string Separator = ".";
        private const string ZeroString = "0";

        public int Base
        {
            get => _base;
            set
            {
                if (value < 2 || value > 16)
                    throw new ArgumentException("Основание должно быть от 2 до 16");
                _base = value;
                UpdateStringRepresentation();
            }
        }

        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                UpdateStringRepresentation();
            }
        }

        public override string Str
        {
            get => _strValue;
            set
            {
                if (ParseFromString(value, _base, out double result))
                {
                    _value = result;
                    _strValue = value;
                }
                else
                    throw new FormatException("Некорректный формат числа для данного основания");
            }
        }

        public TPNumber(double value, int numBase)
        {
            Base = numBase;
            Value = value;
        }

        public TPNumber(string str, int numBase)
        {
            Base = numBase;
            Str = str;
        }

        public TPNumber(TPNumber other)
        {
            _value = other._value;
            _base = other._base;
            _strValue = other._strValue;
        }

        private string DoubleToBaseString(double number, int numBase)
        {
            if (number == 0) return ZeroString;

            long intPart = (long)Math.Abs(number);
            double fracPart = Math.Abs(number) - intPart;

            string intStr = ConvertIntToBase(intPart, numBase);
            string fracStr = ConvertFracToBase(fracPart, numBase, 10);

            string sign = number < 0 ? "-" : "";
            return fracStr.Length > 0 ? $"{sign}{intStr}{Separator}{fracStr}" : $"{sign}{intStr}";
        }

        private string ConvertIntToBase(long number, int numBase)
        {
            if (number == 0) return "0";
            string digits = "0123456789ABCDEF";
            string result = "";
            while (number > 0)
            {
                result = digits[(int)(number % numBase)] + result;
                number /= numBase;
            }
            return result;
        }

        private string ConvertFracToBase(double fraction, int numBase, int precision)
        {
            if (fraction == 0) return "";
            string digits = "0123456789ABCDEF";
            string result = "";
            for (int i = 0; i < precision; i++)
            {
                fraction *= numBase;
                int digit = (int)fraction;
                result += digits[digit];
                fraction -= digit;
                if (fraction < 1e-10) break;
            }
            return result;
        }

        private bool ParseFromString(string str, int numBase, out double result)
        {
            result = 0;
            str = str.Trim().Replace(',', '.');
            bool negative = str.StartsWith("-");
            if (negative) str = str.Substring(1);

            string[] parts = str.Split(new[] { Separator }, StringSplitOptions.None);
            if (parts.Length > 2) return false;

            long intPart = 0;
            if (!TryParseBaseString(parts[0], numBase, out intPart))
                return false;

            double fracPart = 0;
            if (parts.Length == 2)
            {
                double fracMultiplier = 1.0 / numBase;
                for (int i = 0; i < parts[1].Length; i++)
                {
                    int digit = GetDigitValue(parts[1][i]);
                    if (digit < 0 || digit >= numBase) return false;
                    fracPart += digit * fracMultiplier;
                    fracMultiplier /= numBase;
                }
            }

            result = intPart + fracPart;
            if (negative) result = -result;
            return true;
        }

        private bool TryParseBaseString(string str, int numBase, out long result)
        {
            result = 0;
            foreach (char c in str)
            {
                int digit = GetDigitValue(c);
                if (digit < 0 || digit >= numBase) return false;
                result = result * numBase + digit;
            }
            return true;
        }

        private int GetDigitValue(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return 10 + (c - 'A');
            if (c >= 'a' && c <= 'f') return 10 + (c - 'a');
            return -1;
        }

        private void UpdateStringRepresentation()
        {
            _strValue = DoubleToBaseString(_value, _base);
        }

        public override bool IsZero() => Math.Abs(_value) < 1e-10;

        public override bool Equals(TANumber other)
        {
            if (other is TPNumber p)
                return Math.Abs(_value - p._value) < 1e-10 && _base == p._base;
            return false;
        }

        public override TANumber Add(TANumber b)
        {
            if (b is TPNumber p)
                return new TPNumber(_value + p._value, _base);
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Sub(TANumber b)
        {
            if (b is TPNumber p)
                return new TPNumber(_value - p._value, _base);
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Mul(TANumber b)
        {
            if (b is TPNumber p)
                return new TPNumber(_value * p._value, _base);
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Div(TANumber b)
        {
            if (b is TPNumber p)
            {
                if (p.IsZero()) throw new DivideByZeroException();
                return new TPNumber(_value / p._value, _base);
            }
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Square() => new TPNumber(_value * _value, _base);

        public override TANumber Inverse()
        {
            if (IsZero()) throw new DivideByZeroException();
            return new TPNumber(1.0 / _value, _base);
        }

        public override TANumber Copy() => new TPNumber(this);
    }
}

