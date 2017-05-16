using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Blocks
{
    enum VariableType
    {
        Integer,
        Float,
        String
    }

    class InputBlock : Block
    {
        public VariableType VarType { get; set; }

        public InputBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1, propertiesManager)
        {
            Shape = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Points = {new Point(0,0), new Point(20,30), new Point(100,30), new Point(80,0)}
            };

            AddShapeToCanvas();
            AddTextBlockToCanvas("Input: ");
            OnMouseLeftButtonDown(null,null);

            VarType = 0;
        }

        public override Block Run()
        {
            string variableValue;

            do
            {
                variableValue = Microsoft.VisualBasic.Interaction.InputBox("Enter variable value:", "Input");
            } while (String.IsNullOrWhiteSpace(variableValue));

            switch (VarType)
            {
                case VariableType.Integer:
                    Runner.Variables[Command] = int.Parse(variableValue);
                    break;

                case VariableType.Float:
                    Runner.Variables[Command] = float.Parse(variableValue);
                    break;

                case VariableType.String:
                    Runner.Variables[Command] = variableValue;
                    break;

            }

            return NextBlockPrimary;
        }
    }
}
