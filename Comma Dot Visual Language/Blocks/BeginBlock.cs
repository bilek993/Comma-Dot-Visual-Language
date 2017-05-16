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
            Shape = new Ellipse()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Width = 100,
                Height = 30
            };

            AddShapeToCanvas();
            AddTextBlockToCanvas();
        }

        public override Block Run()
        {
            MessageBox.Show("Start");

            return NextBlockPrimary;
        }
    }
}
