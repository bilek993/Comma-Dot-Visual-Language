using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language.OnpParser
{
    public enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Power,
        Bracket1,
        Bracket2,
        Undefined
    }

    public class Operator : ExpressionNode
    {
        private readonly Operation _operation;
        private readonly ExpressionNode _operand1;
        private readonly ExpressionNode _operand2;

        public Operator(Operation operation, ExpressionNode operand1, ExpressionNode operand2)
        {
            _operation = operation;
            _operand1 = operand1;
            _operand2 = operand2;
        }

        public override float CalculateValue()
        {
            switch (_operation)
            {
                case Operation.Addition:
                    Value = _operand1.CalculateValue() + _operand2.CalculateValue();
                    break;
                case Operation.Subtraction:
                    Value = _operand1.CalculateValue() - _operand2.CalculateValue();
                    break;
                case Operation.Multiplication:
                    Value = _operand1.CalculateValue() * _operand2.CalculateValue();
                    break;
                case Operation.Division:
                    Value = _operand1.CalculateValue() / _operand2.CalculateValue();
                    break;
                case Operation.Power:
                    Value = (float)Math.Pow(_operand1.CalculateValue(), _operand2.CalculateValue());
                    break;
            }

            return Value;
        }
    }
}
