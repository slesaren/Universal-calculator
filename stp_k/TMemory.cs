using Calculator.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Calculator.Numbers;

namespace CalculatorMemory
{
    public enum MemoryState
    {
        Off,
        On
    }

    public class TMemory
    {
        private TANumber _mem;
        private MemoryState _state;

        public TANumber Mem
        {
            get => _mem?.Copy();
            private set => _mem = value?.Copy();
        }

        public MemoryState State
        {
            get => _state;
            private set => _state = value;
        }

        public string StateString
        {
            get
            {
                return _state switch
                {
                    MemoryState.On => "On",
                    MemoryState.Off => "Off",
                    _ => "Unknown"
                };
            }
        }

        public string NumberString
        {
            get
            {
                if (_mem == null)
                    return "0";
                return _mem.Str;
            }
        }

        public TMemory(TANumber initialNumber)
        {
            if (initialNumber == null)
                throw new ArgumentNullException(nameof(initialNumber));

            _mem = initialNumber.Copy();
            _state = MemoryState.Off;
        }

        public void Store(TANumber number)
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            _mem = number.Copy();
            _state = MemoryState.On;
        }

        public TANumber Retrieve()
        {
            if (_mem == null)
                throw new InvalidOperationException("В памяти нет числа");

            _state = MemoryState.On;
            return _mem.Copy();
        }

        public void Add(TANumber number)
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            if (_mem == null)
            {
                _mem = number.Copy();
            }
            else
            {
                TANumber result = _mem.Add(number);
                _mem = result;
            }

            _state = MemoryState.On;
        }

        public void Clear()
        {
            if (_state != MemoryState.On)
                throw new InvalidOperationException("Память выключена. Невозможно выполнить очистку.");

            if (_mem != null)
            {
                _mem = CreateZeroOfSameType(_mem);
            }

            _state = MemoryState.Off;
        }

        private TANumber CreateZeroOfSameType(TANumber prototype)
        {
            var zeroCopy = prototype.Copy();
            zeroCopy.Str = "0";
            return zeroCopy;
        }

        public bool IsOn => _state == MemoryState.On;
        public bool IsOff => _state == MemoryState.Off;

        public override string ToString()
        {
            return $"Memory[State={StateString}, Number={NumberString}]";
        }
    }
}


