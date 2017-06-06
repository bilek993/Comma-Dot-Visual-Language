using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language.OnpParser
{
    public class Parser
    {
        private static readonly int[] _operatorsPriorities = new int[] { 1, 1, 2, 2, 3, 0, 1 };

        private Stack<Operation> _stack = new Stack<Operation>();
        private Stack<ExpressionNode> _operationTreeElements = new Stack<ExpressionNode>();

        private Operation ParseOperator(char c)
        {
            Operation op = Operation.Undefined;
            if (c == '+')
                op = Operation.Addition;
            else if (c == '-')
                op = Operation.Subtraction;
            else if (c == '*')
                op = Operation.Multiplication;
            else if (c == '/')
                op = Operation.Division;
            else if (c == '^')
                op = Operation.Power;
            else if (c == '(')
                op = Operation.Bracket1;
            else if (c == ')')
                op = Operation.Bracket2;

            return op;
        }

        private void AddedValueToTree(string value)
        {
            float result;
            if (float.TryParse(value, out result))
                _operationTreeElements.Push(new ExpressionNode(result));
            else
                _operationTreeElements.Push(new Variable(value));

        }

        private void AddedNewExpressionToTree(Operation operation)
        {
            ExpressionNode operand2 = _operationTreeElements.Pop();
            ExpressionNode operand1 = _operationTreeElements.Pop();
            Operator newExpression = new Operator(operation, operand1, operand2);
            _operationTreeElements.Push(newExpression);
        }

        public ExpressionNode ParseExpression(string expression)
        {
            string value = "";

            foreach (char c in expression)
            {
                Operation op = ParseOperator(c);
                                
                if (op != Operation.Undefined)
                {
                    if (op == Operation.Bracket2)
                    {
                        if (!String.IsNullOrEmpty(value))
                        {
                            AddedValueToTree(value);
                            value = "";
                        }

                        while (_stack.Count > 0)
                        {
                            Operation o = _stack.Pop();

                            if (o == Operation.Bracket1)
                                break;
                            else
                            {
                                AddedNewExpressionToTree(o);
                            }
                        }
                    }
                    else if (op == Operation.Bracket1)
                    {
                        _stack.Push(op);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(value))
                        {
                            AddedValueToTree(value);
                            value = "";
                        }

                        while (_stack.Count > 0 && _operatorsPriorities[(int)_stack.First()] >= _operatorsPriorities[(int)op])
                        {
                            Operation o = _stack.Pop();

                            AddedNewExpressionToTree(o);
                        }

                        _stack.Push(op);
                    }
                }
                else
                {
                    value += c;
                }
            }

            if (!String.IsNullOrEmpty(value))
            {
                AddedValueToTree(value);
                value = "";
            }

            while (_stack.Count > 0)
            {
                Operation o = _stack.Pop();

                AddedNewExpressionToTree(o);
            }

            return _operationTreeElements.Pop();
        }
    }
}
