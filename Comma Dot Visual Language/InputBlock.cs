using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    class InputBlock : Block
    {
        public InputBlock(Canvas canvas) : base(canvas)
        {
            _shape = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.White),
                Points = {new Point(0,0), new Point(20,30), new Point(100,30), new Point(80,0)}
            };

            canvas.Children.Add(_shape);
            canvas.Children[canvas.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            canvas.Children[canvas.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            canvas.Children[canvas.Children.Count - 1].MouseMove += OnMouseMove;

            _isPressed = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!_isPressed)
                return;

            Polygon tmpRectangle = (Polygon)sender;
            var mousePosition = mouseEventArgs.GetPosition(CanvasBlocks);
            double left = mousePosition.X - (tmpRectangle.ActualWidth / 2);
            double top = mousePosition.Y - (tmpRectangle.ActualHeight / 2);
            Canvas.SetLeft(tmpRectangle, left);
            Canvas.SetTop(tmpRectangle, top);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Polygon tmpRectangle = (Polygon)sender;
            tmpRectangle.ReleaseMouseCapture();
            _isPressed = false;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Polygon tmpRectangle = (Polygon)sender;
            tmpRectangle.CaptureMouse();
            _isPressed = true;
        }

        public override String Run()
        {
            throw new NotImplementedException();
        }
    }
}
