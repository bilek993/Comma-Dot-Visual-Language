using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;
using System.Text.RegularExpressions;
using System;

namespace Comma_Dot_Visual_Language.Blocks
{
    class CommandBlock : Block
    {
        public CommandBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1, propertiesManager)
        {
            Shape = new Rectangle()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Width = 100,
                Height = 30
            };

            AddShapeToCanvas();
            AddTextBlockToCanvas();
            OnMouseLeftButtonDown(null, null);
        }

        private void ExceptionHasArguments(string[] arguments)
        {
            if (arguments.Length > 0)
                throw new ArgumentException();
        }

        private void ExceptionHasReturnedVariable(string returnedArguments)
        {
            if (returnedArguments != "")
                throw new ArgumentException();
        }

        public override Block Run()
        {
            Regex regex = new Regex(@"^((([A-Za-z][a-zA-Z0-9]*)=)?([A-Za-z][a-zA-Z0-9]*)[(](([A-Za-z][a-zA-Z0-9]*,)*[A-Za-z][a-zA-Z0-9]*)?[)])$");
            Match match;
            string commandOptimized = Regex.Replace(Command, @"\s+", "");

            if (!regex.IsMatch(commandOptimized))
                throw new ArgumentException();

            match = regex.Match(commandOptimized);

            string matchedReturnedVariable = match.Groups[3].Success ? match.Groups[3].Value : "";
            string matchedCommand = match.Groups[4].Value;
            string matchedArguments = match.Groups[5].Success ? match.Groups[5].Value : "";
            string[] arguments = CommandExecution.ArgumentsSpliter(matchedArguments);

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

                default:
                    throw new InvalidOperationException();
            }

            return NextBlockPrimary;
        }
    }
}
