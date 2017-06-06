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
        private Operation operation;
        private ExpressionNode operand1;
        private ExpressionNode operand2;

        public Operator(Operation operation, ExpressionNode operand1, ExpressionNode operand2)
        {
            this.operation = operation;
            this.operand1 = operand1;
            this.operand2 = operand2;
        }

        public override float calculateValue()
        {
            if (operation == Operation.Addition)
                Value = operand1.calculateValue() + operand2.calculateValue();
            else if (operation == Operation.Subtraction)
                Value = operand1.calculateValue() - operand2.calculateValue();
            else if (operation == Operation.Multiplication)
                Value = operand1.calculateValue() * operand2.calculateValue();
            else if (operation == Operation.Division)
                Value = operand1.calculateValue() / operand2.calculateValue();
            else if (operation == Operation.Power)
                Value = (float)Math.Pow(operand1.calculateValue(), operand2.calculateValue());

            return Value;
        }
    }
}
