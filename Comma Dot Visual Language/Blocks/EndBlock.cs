using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Blocks
{
    class EndBlock : Block
    {
        public EndBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 0, propertiesManager)
        {
            Shape = ImageGenerator.GenerateBlockImage("end_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas();
            OnMouseLeftButtonDown(null, null);
        }

        public override Block Run()
        {
            return NextBlockPrimary;
        }
    }
}
