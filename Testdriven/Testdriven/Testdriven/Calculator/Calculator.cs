using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testdriven.Calculator
{
    public class Calculator
    {
        Stack<int> arguments = new Stack<int>();

        public enum Operation
        {
            Add
        }

        public int Result { get; set; }

        public void PushArgument(int p0)
        {
            arguments.Push(p0);
        }

        public void ApplyOperation(Operation op)
        {
            switch(op)
            {
                case Operation.Add:
                    Result = Add(arguments.Pop(), arguments.Pop());
                    break;
            }
        }

        private int Add(int v1, int v2)
        {
            return v1 + v2;
        }
    }
}
