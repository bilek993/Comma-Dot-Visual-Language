using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Comma_Dot_Visual_Language
{
    public abstract class Block : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; private set; }
        public static int BlocksCounter = 0;
        public Block NextBlockPrimary { get; set; }
        public Block NextBlockOptional { get; set; }

        protected Canvas CanvasBlocks;
        protected Shape Shape;
        protected TextBlock TextBlockCommand;
        protected Line ConnectionPrimary;
        protected Line ConnectionOptional;
        protected bool IsPressed;

        private string _command = "";
        private double _connectionX;
        private double _connectionY;

        public string Command
        {
            get => _command;
            set
            {
                _command = value;
                TextBlockCommand.Text = value;
            }
        }
        public double ConnectionX
        {
            get => _connectionX;
            set
            {
                _connectionX = value;
                OnPropertyChanged("ConnectionX");
            }
        }
        public double ConnectionY
        {
            get => _connectionY;
            set
            {
                _connectionY = value;
                OnPropertyChanged("ConnectionY");
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
            ChildrenAddEvents();
        }

        protected void AddTextBlockToCanvas()
        {
            TextBlockCommand = new TextBlock()
            {
                Text = Command,
                Foreground = new SolidColorBrush(Colors.Black)
            };

            CanvasBlocks.Children.Add(TextBlockCommand);
            ChildrenAddEvents();
        }

        private void ChildrenAddEvents()
        {
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            CanvasBlocks.Children[CanvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        protected void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsPressed)
                return;

            var mousePosition = mouseEventArgs.GetPosition(CanvasBlocks);
            SetPositon(mousePosition.X - (Shape.ActualWidth / 2), mousePosition.Y - (Shape.ActualHeight / 2));
        }

        public void SetPositon(double left, double top)
        {
            Canvas.SetLeft(Shape, left);
            Canvas.SetTop(Shape, top);

            if (Shape.ActualWidth != 0 && Shape.ActualHeight != 0)
            {
                Canvas.SetLeft(TextBlockCommand, left + Shape.ActualWidth / 2 - TextBlockCommand.ActualWidth / 2);
                Canvas.SetTop(TextBlockCommand, top + Shape.ActualHeight / 2 - TextBlockCommand.ActualHeight / 2);
            }
            else
            {
                Shape.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                TextBlockCommand.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Canvas.SetLeft(TextBlockCommand, left + Shape.DesiredSize.Width / 2 - TextBlockCommand.DesiredSize.Width / 2);
                Canvas.SetTop(TextBlockCommand, top + Shape.DesiredSize.Height / 2 - TextBlockCommand.DesiredSize.Height / 2);
            }

            ConnectionX = left + Shape.ActualWidth / 2;
            ConnectionY = top + Shape.ActualHeight / 2;
        }

        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Shape.ReleaseMouseCapture();
            IsPressed = false;
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (BlockManager.IsAddConectionMode)
            {
                if (BlockManager.FirstBlockForConnection == null)
                    BlockManager.FirstBlockForConnection = this;
                else
                {
                    BlockManager.SecondBlockForConnection = this;
                    BlockManager.AddConnection();
                }
            }

            Shape.CaptureMouse();
            IsPressed = true;
        }

        public void AddConnection(Block block)
        {
            ConnectionPrimary = new Line()
            {
                Stroke = Brushes.Black
            };

            CanvasBlocks.Children.Add(ConnectionPrimary);

            Binding binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ConnectionX")
            };
            ConnectionPrimary.SetBinding(Line.X1Property, binding);

            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ConnectionY")
            };
            ConnectionPrimary.SetBinding(Line.Y1Property, binding);

            binding = new Binding
            {
                Source = block,
                Path = new PropertyPath("ConnectionX")
            };
            ConnectionPrimary.SetBinding(Line.X2Property, binding);

            binding = new Binding
            {
                Source = block,
                Path = new PropertyPath("ConnectionY")
            };
            ConnectionPrimary.SetBinding(Line.Y2Property, binding);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract string Run();
    }
}
