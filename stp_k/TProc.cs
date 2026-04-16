using Calculator.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Calculator.Numbers;

namespace CalculatorProcessor
{
    public enum TOprtn
    {
        None,
        Add,
        Sub,
        Mul,
        Dvd
    }

    public enum TFunc
    {
        Rev,
        Sqr
    }

    public class TProc
    {
        private TANumber _lopRes;
        private TANumber _rop;
        private TOprtn _operation;
        private string _error;

        public TANumber LopRes
        {
            get => _lopRes?.Copy();
            private set => _lopRes = value?.Copy();
        }

        public TANumber Rop
        {
            get => _rop?.Copy();
            private set => _rop = value?.Copy();
        }

        public TOprtn Operation
        {
            get => _operation;
            private set => _operation = value;
        }

        public string Error
        {
            get => _error ?? string.Empty;
            private set => _error = value;
        }

        public string LopResString => _lopRes?.Str ?? "0";
        public string RopString => _rop?.Str ?? "0";
        public string OperationString => _operation.ToString();
        public bool HasError => !string.IsNullOrEmpty(_error);

        public TProc(TANumber left, TANumber right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            _lopRes = left.Copy();
            _rop = right.Copy();
            _operation = TOprtn.None;
            _error = string.Empty;
        }

        public void ReSet()
        {
            _lopRes = CreateZeroOfSameType(_lopRes);
            _rop = CreateZeroOfSameType(_rop);
            _operation = TOprtn.None;
            _error = string.Empty;
        }

        public void OprtnClear()
        {
            _operation = TOprtn.None;
        }

        public void OprtnSet(TOprtn optn)
        {
            _operation = optn;
        }

        public void OprtnRun()
        {
            if (_operation == TOprtn.None)
                return;

            if (_lopRes == null || _rop == null)
            {
                _error = "Операнды не инициализированы";
                return;
            }

            try
            {
                TANumber result = null;

                switch (_operation)
                {
                    case TOprtn.Add:
                        result = _lopRes.Add(_rop);
                        break;
                    case TOprtn.Sub:
                        result = _lopRes.Sub(_rop);
                        break;
                    case TOprtn.Mul:
                        result = _lopRes.Mul(_rop);
                        break;
                    case TOprtn.Dvd:
                        result = _lopRes.Div(_rop);
                        break;
                }

                if (result != null)
                {
                    _lopRes = result;
                    _error = string.Empty;
                }
            }
            catch (DivideByZeroException)
            {
                _error = "Деление на ноль";
            }
            catch (Exception ex)
            {
                _error = $"Ошибка выполнения операции: {ex.Message}";
            }
        }

        public void FuncRun(TFunc func)
        {
            if (_rop == null)
            {
                _error = "Правый операнд не инициализирован";
                return;
            }

            try
            {
                TANumber result = null;

                switch (func)
                {
                    case TFunc.Rev:
                        result = _rop.Inverse();
                        break;
                    case TFunc.Sqr:
                        result = _rop.Square();
                        break;
                }

                if (result != null)
                {
                    _rop = result;
                    _error = string.Empty;
                }
            }
            catch (DivideByZeroException)
            {
                _error = "Деление на ноль при вычислении обратного числа";
            }
            catch (Exception ex)
            {
                _error = $"Ошибка выполнения функции: {ex.Message}";
            }
        }

        public void LopResSet(TANumber operand)
        {
            if (operand == null)
                throw new ArgumentNullException(nameof(operand));

            _lopRes = operand.Copy();
        }

        public TANumber LopResGet()
        {
            return _lopRes?.Copy();
        }

        public void RopSet(TANumber operand)
        {
            if (operand == null)
                throw new ArgumentNullException(nameof(operand));

            _rop = operand.Copy();
        }

        public TANumber RopGet()
        {
            return _rop?.Copy();
        }

        public TOprtn OprtnGet()
        {
            return _operation;
        }

        public string ErrorGet()
        {
            return _error ?? string.Empty;
        }

        public void ErrorClear()
        {
            _error = string.Empty;
        }

        private TANumber CreateZeroOfSameType(TANumber prototype)
        {
            if (prototype == null)
                return null;

            var zeroCopy = prototype.Copy();
            zeroCopy.Str = "0";
            return zeroCopy;
        }

        public bool IsOperationSet => _operation != TOprtn.None;

        public override string ToString()
        {
            return $"TProc[LopRes={LopResString}, Rop={RopString}, Op={OperationString}, Error={Error}]";
        }
    }
}

