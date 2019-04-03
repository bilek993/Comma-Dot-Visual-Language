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
            Shape = ImageGenerator.GenerateBlockImage("io_block");

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
            } while (string.IsNullOrWhiteSpace(variableValue));

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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return NextBlockPrimary;
        }
    }
}
