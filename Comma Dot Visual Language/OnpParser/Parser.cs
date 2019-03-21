using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language.OnpParser
{
    public class Parser
    {
        private static readonly int[] OperatorsPriorities = { 1, 1, 2, 2, 3, 0, 1 };

        private readonly Stack<Operation> _stack = new Stack<Operation>();
        private readonly Stack<ExpressionNode> _operationTreeElements = new Stack<ExpressionNode>();

        private static Operation ParseOperator(char c)
        {
            var op = Operation.Undefined;
            switch (c)
            {
                case '+':
                    op = Operation.Addition;
                    break;
                case '-':
                    op = Operation.Subtraction;
                    break;
                case '*':
                    op = Operation.Multiplication;
                    break;
                case '/':
                    op = Operation.Division;
                    break;
                case '^':
                    op = Operation.Power;
                    break;
                case '(':
                    op = Operation.Bracket1;
                    break;
                case ')':
                    op = Operation.Bracket2;
                    break;
            }

            return op;
        }

        private void AddedValueToTree(string value)
        {
            _operationTreeElements.Push(float.TryParse(value, out var result)
                ? new ExpressionNode(result)
                : new Variable(value));
        }

        private void AddedNewExpressionToTree(Operation operation)
        {
            var operand2 = _operationTreeElements.Pop();
            var operand1 = _operationTreeElements.Pop();
            var newExpression = new Operator(operation, operand1, operand2);
            _operationTreeElements.Push(newExpression);
        }

        public ExpressionNode ParseExpression(string expression)
        {
            var value = "";

            foreach (var c in expression)
            {
                var op = ParseOperator(c);
                                
                if (op != Operation.Undefined)
                {
                    if (op == Operation.Bracket2)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            AddedValueToTree(value);
                            value = "";
                        }

                        while (_stack.Count > 0)
                        {
                            var o = _stack.Pop();

                            if (o == Operation.Bracket1)
                                break;
                            AddedNewExpressionToTree(o);
                        }
                    }
                    else if (op == Operation.Bracket1)
                    {
                        _stack.Push(op);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            AddedValueToTree(value);
                            value = "";
                        }

                        while (_stack.Count > 0 && OperatorsPriorities[(int)_stack.First()] >= OperatorsPriorities[(int)op])
                        {
                            var o = _stack.Pop();

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

            if (!string.IsNullOrEmpty(value))
            {
                AddedValueToTree(value);
            }

            while (_stack.Count > 0)
            {
                var o = _stack.Pop();

                AddedNewExpressionToTree(o);
            }

            return _operationTreeElements.Pop();
        }
    }
}
