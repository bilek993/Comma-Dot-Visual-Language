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
        public Block NextBlockPrimary { get; private set; }
        public Block NextBlockOptional { get; private set; }
        public int MaxConnectionsCount { get; private set; }

        protected Canvas CanvasBlocks;
        protected Shape Shape;
        protected TextBlock TextBlockCommand;
        protected Line LineConnectionPrimary;
        protected Line LineConnectionOptional;
        protected bool IsPressed;

        private string _command = "";
        private double _connectionInputX;
        private double _connectionInputY;
        private double _connectionOutput1X;
        private double _connectionOutput1Y;
        private double _connectionOutput2X;
        private double _connectionOutput2Y;
        private PropertiesManager _propertiesManager;
        private string _prefixCommand = "";
        private string _suffixCommand = "";

        public string Command
        {
            get { return _command; }
            set
            {
                _command = value;
                TextBlockCommand.Text = _prefixCommand + value + _suffixCommand;
            }
        }

        public double ConnectionInputX
        {
            get { return _connectionInputX; }
            set
            {
                _connectionInputX = value;
                OnPropertyChanged("ConnectionInputX");
            }
        }
        public double ConnectionInputY
        {
            get { return _connectionInputY; }
            set
            {
                _connectionInputY = value;
                OnPropertyChanged("ConnectionInputY");
            }
        }
        public double ConnectionOutput1X
        {
            get { return _connectionOutput1X; }
            set
            {
                _connectionOutput1X = value;
                OnPropertyChanged("ConnectionOutput1X");
            }
        }
        public double ConnectionOutput1Y
        {
            get { return _connectionOutput1Y; }
            set
            {
                _connectionOutput1Y = value;
                OnPropertyChanged("ConnectionOutput1Y");
            }
        }
        public double ConnectionOutput2X
        {
            get { return _connectionOutput2X; }
            set
            {
                _connectionOutput2X = value;
                OnPropertyChanged("ConnectionOutput2X");
            }
        }
        public double ConnectionOutput2Y
        {
            get { return _connectionOutput2Y; }
            set
            {
                _connectionOutput2Y = value;
                OnPropertyChanged("ConnectionOutput2Y");
            }
        }

        protected Block(Canvas canvas, int maxConnectionsCount, PropertiesManager propertiesManager)
        {
            Id = BlocksCounter++;
            CanvasBlocks = canvas;
            MaxConnectionsCount = maxConnectionsCount;
            _propertiesManager = propertiesManager;
        }

        protected void AddShapeToCanvas()
        {
            CanvasBlocks.Children.Add(Shape);
            ChildrenAddEvents();
        }

        protected void AddTextBlockToCanvas(string prefix = "", string suffix = "")
        {
            TextBlockCommand = new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.Black)
            };

            _prefixCommand = prefix;
            _suffixCommand = suffix;
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

            SetConnectionsPositions(left, top);
        }

        protected virtual void SetConnectionsPositions(double shapeLeft, double shapeTop)
        {

            ConnectionInputX = shapeLeft + Shape.ActualWidth / 2;
            ConnectionInputY = shapeTop;

            ConnectionOutput1X = ConnectionOutput2X = shapeLeft + Shape.ActualWidth / 2;
            ConnectionOutput1Y = ConnectionOutput2Y = shapeTop + Shape.ActualHeight;
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
                    Mouse.OverrideCursor = null;
                    BlockManager.SecondBlockForConnection = this;
                    BlockManager.AddConnection();
                }
            }

            _propertiesManager.SelectedBlock = this;
            _propertiesManager.Update();

            Shape.CaptureMouse();
            IsPressed = true;
        }

        public void AddConnection(Block block)
        {
            if (this == block)
            {
                MessageBox.Show("Self connection is not possible due to potential endless loop.","Connection error!",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (MaxConnectionsCount == 0)
            {
                MessageBox.Show("Outputs are not supported by this block.", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NextBlockPrimary == null)
            {
                NextBlockPrimary = block;

                LineConnectionPrimary = createConnectionLine(block, 1);
            }
            else if (MaxConnectionsCount == 2 && NextBlockOptional == null)
            {
                NextBlockOptional = block;

                LineConnectionOptional = createConnectionLine(block, 2);
            }
            else
            {
                MessageBox.Show("You cannot add another connection, because all outputs are already used.", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private Line createConnectionLine(Block secondBlock, int outputIndex)
        {
            Line connectionLine = new Line()
            {
                Stroke = Brushes.Black
            };

            CanvasBlocks.Children.Add(connectionLine);

            Binding binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ConnectionOutput" + outputIndex + "X")
            };
            connectionLine.SetBinding(Line.X1Property, binding);

            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ConnectionOutput" + outputIndex + "Y")
            };
            connectionLine.SetBinding(Line.Y1Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputX")
            };
            connectionLine.SetBinding(Line.X2Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputY")
            };
            connectionLine.SetBinding(Line.Y2Property, binding);

            return connectionLine;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RemoveLine(int connectionId)
        {
            switch (connectionId)
            {
                case 0:
                    CanvasBlocks.Children.Remove(LineConnectionPrimary);
                    NextBlockPrimary = null;
                    break;
                case 1:
                    CanvasBlocks.Children.Remove(LineConnectionOptional);
                    NextBlockOptional = null;
                    break;
            }
        }

        public abstract Block Run();
    }
}
