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
            Shape = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Points = { new Point(0, 0), new Point(20, 30), new Point(100, 30), new Point(80, 0) }
            };

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
