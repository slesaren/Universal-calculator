using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Calculator.Numbers
{
    public class TFrac : TANumber
    {
        private long _numerator;
        private long _denominator;
        private string _strValue;

        public long Numerator
        {
            get => _numerator;
            set
            {
                _numerator = value;
                Normalize();
                UpdateStringRepresentation();
            }
        }

        public long Denominator
        {
            get => _denominator;
            set
            {
                if (value == 0) throw new ArgumentException("Знаменатель не может быть нулевым");
                _denominator = value;
                Normalize();
                UpdateStringRepresentation();
            }
        }

        public override string Str
        {
            get => _strValue;
            set
            {
                if (!ParseFromString(value, out long num, out long den))
                    throw new FormatException("Некорректный формат дроби");
                _numerator = num;
                _denominator = den;
                Normalize();
                UpdateStringRepresentation();
            }
        }

        public TFrac(long numerator, long denominator)
        {
            _denominator = denominator == 0 ? throw new ArgumentException("Знаменатель не может быть нулевым") : denominator;
            _numerator = numerator;
            Normalize();
            UpdateStringRepresentation();
        }

        public TFrac(string str)
        {
            Str = str;
        }

        public TFrac(TFrac other)
        {
            _numerator = other._numerator;
            _denominator = other._denominator;
            _strValue = other._strValue;
        }

        private void Normalize()
        {
            if (_denominator < 0)
            {
                _numerator = -_numerator;
                _denominator = -_denominator;
            }

            long gcd = GCD(Math.Abs(_numerator), Math.Abs(_denominator));
            if (gcd > 0)
            {
                _numerator /= gcd;
                _denominator /= gcd;
            }
        }

        private long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

        private bool ParseFromString(string str, out long num, out long den)
        {
            num = den = 0;
            str = str.Trim();
            string[] parts = str.Split('/');
            if (parts.Length > 2) return false;

            if (parts.Length == 2)
            {
                if (!long.TryParse(parts[0], out num) || !long.TryParse(parts[1], out den))
                    return false;
                return true;
            }
            else
            {
                if (long.TryParse(parts[0], out num))
                {
                    den = 1;
                    return true;
                }
                return false;
            }
        }

        private void UpdateStringRepresentation()
        {
            _strValue = $"{_numerator}/{_denominator}";
        }

        public override bool IsZero() => _numerator == 0;

        public override bool Equals(TANumber other)
        {
            if (other is TFrac f)
                return _numerator == f._numerator && _denominator == f._denominator;
            return false;
        }

        public override TANumber Add(TANumber b)
        {
            if (b is TFrac f)
                return new TFrac(
                    _numerator * f._denominator + f._numerator * _denominator,
                    _denominator * f._denominator
                );
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Sub(TANumber b)
        {
            if (b is TFrac f)
                return new TFrac(
                    _numerator * f._denominator - f._numerator * _denominator,
                    _denominator * f._denominator
                );
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Mul(TANumber b)
        {
            if (b is TFrac f)
                return new TFrac(_numerator * f._numerator, _denominator * f._denominator);
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Div(TANumber b)
        {
            if (b is TFrac f)
            {
                if (f.IsZero()) throw new DivideByZeroException();
                return new TFrac(_numerator * f._denominator, _denominator * f._numerator);
            }
            throw new ArgumentException("Несовместимые типы чисел");
        }

        public override TANumber Square() => new TFrac(_numerator * _numerator, _denominator * _denominator);

        public override TANumber Inverse()
        {
            if (IsZero()) throw new DivideByZeroException();
            return new TFrac(_denominator, _numerator);
        }

        public override TANumber Copy() => new TFrac(this);
    }
}



