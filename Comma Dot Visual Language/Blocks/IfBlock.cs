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
            Regex regex = new Regex(@"^((([a-zA-Z][a-zA-Z0-9]*)|([0-9,]+))((==)|(>=)|(<=)|(>)|(<)|(!=))(([a-zA-Z][a-zA-Z0-9]*)|([0-9,]+)))$");
            Match match;
            string commandOptimized = Regex.Replace(Command, @"\s+", "");

            if (!regex.IsMatch(commandOptimized))
                throw new ArgumentException();

            match = regex.Match(commandOptimized);

            object variable1;
            object variable2;

            if (match.Groups[3].Success)
            {
                variable1 = Runner.Variables[match.Groups[3].Value];
            }
            else
            {
                variable1 = ParseVariableValue(match.Groups[4].Value);
            }

            if (match.Groups[13].Success)
            {
                variable2 = Runner.Variables[match.Groups[13].Value];
            }
            else
            {
                variable2 = ParseVariableValue(match.Groups[14].Value);
            }

            return CalculateResult(match, variable1, variable2) ? NextBlockPrimary : NextBlockOptional;
        }

        private object ParseVariableValue(string value)
        {
            int intResult;
            if (int.TryParse(value, out intResult))
            {
                return intResult;
            }

            float floatResult;
            if (float.TryParse(value, out floatResult))
            {
                return floatResult;
            }

            return null;
        }
        
        private bool CalculateResult(Match match, object variable1, object variable2)
        {
            bool result = true;
            if (match.Groups[6].Success)
            {
                if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(int))
                    result = (int)variable1 == (int)variable2;

                else if(variable1.GetType() == typeof(int) && variable2.GetType() == typeof(float))
                    result = (int)variable1 == (float)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(int))
                    result = (float)variable1 == (int)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(float))
                    result = (float)variable1 == (float)variable2;
            }
            else if (match.Groups[7].Success)
            {
                if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(int))
                    result = (int)variable1 >= (int)variable2;

                else if(variable1.GetType() == typeof(int) && variable2.GetType() == typeof(float))
                    result = (int)variable1 >= (float)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(int))
                    result = (float)variable1 >= (int)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(float))
                    result = (float)variable1 >= (float)variable2;
            }
            else if (match.Groups[8].Success)
            {
                if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(int))
                    result = (int)variable1 <= (int)variable2;

                else if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(float))
                    result = (int)variable1 <= (float)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(int))
                    result = (float)variable1 <= (int)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(float))
                    result = (float)variable1 <= (float)variable2;
            }
            else if (match.Groups[9].Success)
            {
                if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(int))
                    result = (int)variable1 > (int)variable2;

                else if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(float))
                    result = (int)variable1 > (float)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(int))
                    result = (float)variable1 > (int)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(float))
                    result = (float)variable1 > (float)variable2;
            }
            else if (match.Groups[10].Success)
            {
                if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(int))
                    result = (int)variable1 < (int)variable2;

                else if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(float))
                    result = (int)variable1 < (float)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(int))
                    result = (float)variable1 < (int)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(float))
                    result = (float)variable1 < (float)variable2;
            }
            else if (match.Groups[11].Success)
            {
                if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(int))
                    result = (int)variable1 != (int)variable2;

                else if (variable1.GetType() == typeof(int) && variable2.GetType() == typeof(float))
                    result = (int)variable1 != (float)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(int))
                    result = (float)variable1 != (int)variable2;

                else if (variable1.GetType() == typeof(float) && variable2.GetType() == typeof(float))
                    result = (float)variable1 != (float)variable2;
            }

            return result;
        }
    }
}
