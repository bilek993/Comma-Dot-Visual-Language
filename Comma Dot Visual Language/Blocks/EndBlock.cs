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
            Shape = new Ellipse()
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
            return NextBlockPrimary;
        }
    }
}
