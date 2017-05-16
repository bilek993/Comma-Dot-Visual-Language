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

        public override Block Run()
        {
            Regex regex = new Regex(@"^(([A-Za-z]+)[(](([A-Za-z]+,)*[A-Za-z]+)[)])$");
            Match match;
            string commandOptimized = Regex.Replace(Command, @"\s+", "");

            if (!regex.IsMatch(commandOptimized))
                throw new ArgumentException();

            match = regex.Match(commandOptimized);

            string matchedCommand = match.Groups[2].Value;
            string matchedArguments = match.Groups[3].Success ? match.Groups[3].Value : "";
            string[] arguments = CommandExecution.ArgumentsSpliter(matchedArguments);

            switch (matchedCommand)
            {
                case "Inc":
                    CommandExecution.IncrementValues(arguments);
                    break;

                case "Dec":
                    CommandExecution.DecrementValues(arguments);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return NextBlockPrimary;
        }
    }
}
