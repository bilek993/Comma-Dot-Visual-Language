using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Blocks
{
    class OutputBlock : Block
    {
        public OutputBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1, propertiesManager)
        {
            Shape = ImageGenerator.GenerateBlockImage("io_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas("Output: ");
            OnMouseLeftButtonDown(null, null);
        }

        public override Block Run()
        {
            MessageBox.Show(Runner.Variables[Command].ToString(), "Output");

            return NextBlockPrimary;
        }
    }
}
