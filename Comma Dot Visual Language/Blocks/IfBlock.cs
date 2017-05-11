using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    class IfBlock : Block
    {
        public IfBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 2, propertiesManager)
        {
            Shape = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Points = { new Point(0, 15), new Point(50, 30), new Point(100, 15), new Point(50, 0) }
            };

            AddShapeToCanvas();
            AddTextBlockToCanvas("if (", ")");
            OnMouseLeftButtonDown(null, null);
        }

        protected override void SetConnectionsPositions(double shapeLeft, double shapeTop)
        {
            ConnectionInputX = shapeLeft + Shape.ActualWidth / 2;
            ConnectionInputY = shapeTop;

            ConnectionOutput1X = shapeLeft;
            ConnectionOutput1Y = shapeTop + Shape.ActualHeight / 2;

            ConnectionOutput2X = shapeLeft + Shape.ActualWidth;
            ConnectionOutput2Y = shapeTop + Shape.ActualHeight / 2;
        }

        public override Block Run()
        {
            Regex regex = new Regex(@"(([a-zA-Z][a-zA-Z0-9]*)|([0-9]+))((==)|(>=)|(<=)|(>)|(<)|(!=))(([a-zA-Z][a-zA-Z0-9]*)|([0-9]+))");
            Match match;
            string commandOptimized = Regex.Replace(Command, @"\s+", "");

            if (!regex.IsMatch(commandOptimized))
                throw new ArgumentException();

            match = regex.Match(commandOptimized);

            return null;
        }
    }
}
