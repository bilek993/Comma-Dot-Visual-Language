using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    abstract class Block
    {
        public int Id { get; private set; }
        public static int BlocksCounter = 0;
        public Block NextBlockPrimary { get; set; }
        public Block NextBlockOptional { get; set; }

        protected Canvas CanvasBlocks;
        protected Shape Shape;
        protected TextBlock TextBlockCommand;
        protected bool IsPressed;

        private string _command = "";

        public string Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
                TextBlockCommand.Text = value;
            }
        }

        protected Block(Canvas canvas)
        {
            Id = BlocksCounter++;
            CanvasBlocks = canvas;
        }

        protected void AddShapeToCanvas()
        {
            CanvasBlocks.Children.Add(Shape);
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        protected void AddTextBlockToCanvas()
        {
            TextBlockCommand = new TextBlock()
            {
                Text = Command,
                Foreground = new SolidColorBrush(Colors.Black)
            };

            CanvasBlocks.Children.Add(TextBlockCommand);
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        protected void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsPressed)
                return;

            var mousePosition = mouseEventArgs.GetPosition(CanvasBlocks);
            double left = mousePosition.X - (Shape.ActualWidth / 2);
            double top = mousePosition.Y - (Shape.ActualHeight / 2);
            Canvas.SetLeft(Shape, left);
            Canvas.SetTop(Shape, top);
            Canvas.SetLeft(TextBlockCommand,left + Shape.ActualWidth / 2 - TextBlockCommand.ActualWidth / 2);
            Canvas.SetTop(TextBlockCommand,top + Shape.ActualHeight / 2 - TextBlockCommand.ActualHeight / 2);
        }

        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Shape.ReleaseMouseCapture();
            IsPressed = false;
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Shape.CaptureMouse();
            IsPressed = true;
        }

        public abstract string Run();
    }
}
