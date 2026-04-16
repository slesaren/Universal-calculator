using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Globalization;
using System.Text.RegularExpressions;



namespace Calculator.Numbers
{
    public class TComp : TANumber
    {
        private double _re;
        private double _im;
        private string _strValue;

        public double Re
        {
            get => _re;
            set
            {
                _re = value;
                UpdateStringRepresentation();
            }
        }

        public double Im
        {
            get => _im;
            set
            {
                _im = value;
                UpdateStringRepresentation();
            }
        }

        public override string Str
        {
            get => _strValue;
            set
            {
                if (!ParseFromString(value, out double re, out double im))
                    throw new FormatException("Некорректный формат комплексного числа");
                _re = re;
                _im = im;
                UpdateStringRepresentation();
            }
        }

        public TComp(double re, double im)
        {
            _re = re;
            _im = im;
            UpdateStringRepresentation();
        }

        public TComp(string str)
        {
            Str = str;
        }

        public TComp(TComp other)
        {
            _re = other._re;
            _im = other._im;
            _strValue = other._strValue;
        }

        private bool ParseFromString(string str, out double re, out double im)
        {
            re = 0;
            im = 0;

            if (string.IsNullOrWhiteSpace(str))
                return false;

            str = str.Replace(" ", "").ToLower();

            if (str == "i") { im = 1; return true; }
            if (str == "-i") { im = -1; return true; }

            int opIndex = -1;
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == '+' || str[i] == '-')
                {
                    opIndex = i;
                    break;
                }
            }

            if (opIndex > 0)
            {
                string rePart = str.Substring(0, opIndex);
                string imPart = str.Substring(opIndex);

                if (!double.TryParse(rePart, NumberStyles.Any, CultureInfo.InvariantCulture, out re))
                    return false;

                if (!imPart.EndsWith("i"))
                    return false;

                string imNumberPart = imPart.Substring(0, imPart.Length - 1);
                if (string.IsNullOrEmpty(imNumberPart) || imNumberPart == "+" || imNumberPart == "-")
                {
                    im = imNumberPart == "-" ? -1 : 1;
                }
                else
                {
                    if (!double.TryParse(imNumberPart, NumberStyles.Any, CultureInfo.InvariantCulture, out im))
                        return false;
                }

                return true;
            }
            else if (str.EndsWith("i"))
            {
                string imPart = str.Substring(0, str.Length - 1);
                if (string.IsNullOrEmpty(imPart) || imPart == "+" || imPart == "-")
                {
                    im = imPart == "-" ? -1 : 1;
                }
                else
                {
                    if (!double.TryParse(imPart, NumberStyles.Any, CultureInfo.InvariantCulture, out im))
                        return false;
                }
                re = 0;
                return true;
            }
            else
            {
                if (!double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out re))
                    return false;
                im = 0;
                return true;
            }
        }

        private void UpdateStringRepresentation()
        {
            string reStr = FormatDouble(_re);
            string imStr = FormatDouble(Math.Abs(_im));

            string sign = _im >= 0 ? "+" : "-";

            if (_im == 0)
                _strValue = reStr;
            else if (_re == 0)
                _strValue = (_im == 1 ? "" : _im == -1 ? "-" : (_im < 0 ? "-" + imStr : imStr)) + "i";
            else
                _strValue = $"{reStr}{sign}{imStr}i";
        }

        private string FormatDouble(double value)
        {
            if (Math.Abs(value - Math.Round(value)) < 1e-10)
                return Math.Round(value).ToString(CultureInfo.InvariantCulture);
            else
                return value.ToString("0.########", CultureInfo.InvariantCulture);
        }

        public override bool IsZero() => Math.Abs(_re) < 1e-10 && Math.Abs(_im) < 1e-10;

        public override bool Equals(TANumber other)
        {
            if (other is TComp c)
                return Math.Abs(_re - c._re) < 1e-10 && Math.Abs(_im - c._im) < 1e-10;
            return false;
        }

        public override TANumber Add(TANumber b)
        {
            if (b is TComp c)
                return new TComp(_re + c._re, _im + c._im);
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Sub(TANumber b)
        {
            if (b is TComp c)
                return new TComp(_re - c._re, _im - c._im);
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Mul(TANumber b)
        {
            if (b is TComp c)
                return new TComp(
                    _re * c._re - _im * c._im,
                    _re * c._im + _im * c._re
                );
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Div(TANumber b)
        {
            if (b is TComp c)
            {
                if (c.IsZero()) throw new DivideByZeroException();
                double denom = c._re * c._re + c._im * c._im;
                return new TComp(
                    (_re * c._re + _im * c._im) / denom,
                    (_im * c._re - _re * c._im) / denom
                );
            }
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Square()
        {
            return new TComp(_re * _re - _im * _im, 2 * _re * _im);
        }

        public override TANumber Inverse()
        {
            if (IsZero()) throw new DivideByZeroException();
            double denom = _re * _re + _im * _im;
            return new TComp(_re / denom, -_im / denom);
        }

        public override TANumber Copy() => new TComp(this);
    }
}


