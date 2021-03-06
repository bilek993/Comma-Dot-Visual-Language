﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;
using System.Text.RegularExpressions;
using System;
using Comma_Dot_Visual_Language.OnpParser;

namespace Comma_Dot_Visual_Language.Blocks
{
    class CommandBlock : Block
    {
        public CommandBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1, propertiesManager)
        {
            Shape = ImageGenerator.GenerateBlockImage("command_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas();
            OnMouseLeftButtonDown(null, null);
        }

        public CommandBlock(Canvas canvas, PropertiesManager propertiesManager, int id) : base(canvas, 1, propertiesManager)
        {
            Id = id;
            _blocksCounter = Id + 1;

            Shape = ImageGenerator.GenerateBlockImage("command_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas();
        }

        private static void ExceptionHasArguments(string[] arguments)
        {
            if (!(arguments.Length == 1 && arguments[0] == ""))
                throw new ArgumentException();
        }

        private static void ExceptionHasReturnedVariable(string returnedArguments)
        {
            if (returnedArguments != "")
                throw new ArgumentException();
        }

        public override Block Run()
        {
            var regexFunction = new Regex(@"^((([A-Za-z][a-zA-Z0-9]*)=)?([A-Za-z][a-zA-Z0-9]*)[(](([A-Za-z][a-zA-Z0-9]*,)*[A-Za-z][a-zA-Z0-9]*)?[)])$");
            var regexOperation = new Regex(@"^(([A-Za-z][a-zA-Z0-9]*)=)([A-Za-z0-9\(\)\+\-\*\/\,\^\(\)]+)$");
            Match match;

            var commandOptimized = Regex.Replace(Command, @"\s+", "");

            if (regexFunction.IsMatch(commandOptimized))
            {
                match = regexFunction.Match(commandOptimized);

                var matchedReturnedVariable = match.Groups[3].Success ? match.Groups[3].Value : "";
                var matchedCommand = match.Groups[4].Value;
                var matchedArguments = match.Groups[5].Success ? match.Groups[5].Value : "";
                var arguments = CommandExecution.ArgumentsSplitter(matchedArguments);

                switch (matchedCommand)
                {
                    case "Inc":
                        ExceptionHasReturnedVariable(matchedReturnedVariable);
                        CommandExecution.IncrementValues(arguments);
                        break;

                    case "Dec":
                        ExceptionHasReturnedVariable(matchedReturnedVariable);
                        CommandExecution.DecrementValues(arguments);
                        break;

                    case "Pi":
                        ExceptionHasArguments(arguments);
                        CommandExecution.Math_Pi(matchedReturnedVariable);
                        break;

                    case "E":
                        ExceptionHasArguments(arguments);
                        CommandExecution.Math_e(matchedReturnedVariable);
                        break;

                    case "Mod":
                        CommandExecution.Modulo(matchedReturnedVariable, arguments);
                        break;

                    case "Sqrt":
                        CommandExecution.Sqrt(matchedReturnedVariable, arguments);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (regexOperation.IsMatch(commandOptimized))
            {
                match = regexOperation.Match(commandOptimized);

                var matchedReturnedVariable = match.Groups[2].Value;
                var matchedExpression = match.Groups[3].Value;

                var parser = new Parser();
                var e = parser.ParseExpression(matchedExpression);

                Runner.Variables[matchedReturnedVariable] = e.CalculateValue();
            }

            return NextBlockPrimary;
        }
    }
}
