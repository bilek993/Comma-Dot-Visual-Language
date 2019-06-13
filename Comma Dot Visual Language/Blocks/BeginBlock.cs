using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Blocks
{
    class BeginBlock : Block
    {
        public BeginBlock(Canvas canvas, PropertiesManager propertiesManager) : base(canvas, 1 , propertiesManager)
        {
            Shape = ImageGenerator.GenerateBlockImage("begin_block");

            AddShapeToCanvas();
            AddTextBlockToCanvas();
        }

        public BeginBlock(Canvas canvas, PropertiesManager propertiesManager, int id)
            : this(canvas, propertiesManager)
        {
            Id = id;
            _blocksCounter = Id + 1;
        }

        public override Block Run()
        {
            return NextBlockPrimary;
        }
    }
}
