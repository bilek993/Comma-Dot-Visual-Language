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
        public string Command { get; private set; }
        public static int BlocksCounter = 0;
        public Block NextBlockPrimary { get; set; }
        public Block NextBlockOptional { get; set; }

        protected Canvas CanvasBlocks;
        protected Shape _shape;
        protected TextBlock _textBlockCommand;
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

        protected void AddTextBlockToCanvas()
        {
            _textBlockCommand = new TextBlock()
            {
                Text = Command,
                Foreground = new SolidColorBrush(Colors.Black)
            };

            CanvasBlocks.Children.Add(_textBlockCommand);
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        public void UpdateCommand(string newCommand)
        {
            Command = newCommand;
            _textBlockCommand.Text = Command;
        }

        protected void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!_isPressed)
                return;

            var mousePosition = mouseEventArgs.GetPosition(CanvasBlocks);
            double left = mousePosition.X - (_shape.ActualWidth / 2);
            double top = mousePosition.Y - (_shape.ActualHeight / 2);
            Canvas.SetLeft(_shape, left);
            Canvas.SetTop(_shape, top);
            Canvas.SetLeft(_textBlockCommand,left + 28);
            Canvas.SetTop(_textBlockCommand,top + 5);
        }

        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _shape.ReleaseMouseCapture();
            _isPressed = false;
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _shape.CaptureMouse();
            _isPressed = true;
        }

        public abstract string Run();
    }
}
