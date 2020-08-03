using System;
using System.Collections.Generic;
using System.Text;

namespace ZenitechCalculator
{
    public class CalculatorStack
    {

        private int _limit;
        private LinkedList<uint> _list;
        private int _maxValue;

        private const string OPERATION_ADD = "ADD";
        private const string OPERATION_SUB = "SUB";

        public CalculatorStack(int maxSize, int maxValue)
        {
            _limit = maxSize;
            _list = new LinkedList<uint>();
            _maxValue = maxValue;
        }

        public bool Push(uint value)
        {
            if (_list.Count == _limit) return false;
            _list.AddLast(value);
            return true;
        }

        public uint? Pop()
        {
            if (_list.Count > 0)
            {
                var value = _list.Last.Value;
                _list.RemoveLast();
                return value;
            }
            return null;
        }

        public uint? Add()
        {
            return Operation(OPERATION_ADD);
        }

        public uint? Sub()
        {
            return Operation(OPERATION_SUB);
        }

        public uint? Operation(string operation)
        {
            var (op1, op2) = GetTop2Values();
            if (op1 == null || op2 == null) return null;

            uint result;
            if (operation == OPERATION_ADD)
                result = (uint)(op1.Value + op2.Value);
            else if (operation == OPERATION_SUB)
                result = (uint)(op1.Value - op2.Value);
            else
                return null;

            result = result > _maxValue ? FixOverflow(op1.Value, op2.Value, operation) : result;
            Push(result);
            return result;

        }

        public override string ToString()
        {
            if (_list.Count == 0) return "Stack is empty";
            return $"(stack is {string.Join(", ", _list)})";
        }

        private uint FixOverflow(uint op1, uint op2, string action)
        {
            switch (action)
            {
                case OPERATION_SUB:
                    return (uint)mod((int)op1 - (int)op2, _maxValue);
                case OPERATION_ADD:
                    return (uint)mod((int)op1 + (int)op2, _maxValue);
                default:
                    return 0;
            }
        }

        private int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }

        private (uint?, uint?) GetTop2Values()
        {
            if (_list.Count < 2)
                return (null, null);
            return (Pop(), Pop());
        }
    }
}
