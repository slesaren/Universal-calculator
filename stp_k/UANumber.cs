using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Numbers
{
    public abstract class TANumber
    {

        public const string Separator = ".";
        public const string ZeroString = "0";

        public abstract string Str { get; set; }

        public abstract bool IsZero();

    
        public abstract bool Equals(TANumber other);

        public abstract TANumber Add(TANumber b);
        public abstract TANumber Sub(TANumber b);
        public abstract TANumber Mul(TANumber b);
        public abstract TANumber Div(TANumber b);

  
        public abstract TANumber Square();
        public abstract TANumber Inverse();

        public abstract TANumber Copy();
    }
}

