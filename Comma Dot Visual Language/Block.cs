using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    abstract class Block
    {
        public int Id { get; private set; }
        public string Command { get; set; }
        public static int BlocksCounter = 0;
        public Block NextBlockPrimary { get; set; }
        public Block NextBlockOptional { get; set; }

        protected Canvas CanvasBlocks;
        protected Polygon _shape;
        protected bool _isPressed;

        protected Block(Canvas canvas)
        {
            Id = BlocksCounter++;
            Command = "";
            CanvasBlocks = canvas;
            _isPressed = true;
        }

        protected void AddShapeToCanvas()
        {
            CanvasBlocks.Children.Add(_shape);
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        protected void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
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

        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Polygon tmpRectangle = (Polygon)sender;
            tmpRectangle.ReleaseMouseCapture();
            _isPressed = false;
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Polygon tmpRectangle = (Polygon)sender;
            tmpRectangle.CaptureMouse();
            _isPressed = true;
        }

        public abstract string Run();
    }
}
