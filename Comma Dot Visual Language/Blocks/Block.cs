using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Blocks
{
    public abstract class Block : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; private set; }
        public Block NextBlockPrimary { get; private set; }
        public Block NextBlockOptional { get; private set; }

        protected Shape Shape;

        private string _command = "";
        private double _connectionInputX;
        private double _connectionInputY;
        private double _connectionOutput1X;
        private double _connectionOutput1Y;
        private double _connectionOutput2X;
        private double _connectionOutput2Y;
        private readonly PropertiesManager _propertiesManager;
        private string _prefixCommand = "";
        private string _suffixCommand = "";
        private TextBlock _textBlockCommand;
        private Line _lineConnectionPrimary;
        private Line _lineConnectionOptional;
        private bool _isPressed;
        private readonly Canvas _canvasBlocks;
        private readonly int _maxConnectionsCount;
        private static int _blocksCounter = 0;

        public string Command
        {
            get => _command;
            set
            {
                _command = value;
                _textBlockCommand.Text = _prefixCommand + value + _suffixCommand;
            }
        }

        public double ConnectionInputX
        {
            get => _connectionInputX;
            set
            {
                _connectionInputX = value;
                OnPropertyChanged("ConnectionInputX");
            }
        }
        public double ConnectionInputY
        {
            get => _connectionInputY;
            set
            {
                _connectionInputY = value;
                OnPropertyChanged("ConnectionInputY");
            }
        }
        public double ConnectionOutput1X
        {
            get => _connectionOutput1X;
            set
            {
                _connectionOutput1X = value;
                OnPropertyChanged("ConnectionOutput1X");
            }
        }
        public double ConnectionOutput1Y
        {
            get => _connectionOutput1Y;
            set
            {
                _connectionOutput1Y = value;
                OnPropertyChanged("ConnectionOutput1Y");
            }
        }
        public double ConnectionOutput2X
        {
            get => _connectionOutput2X;
            set
            {
                _connectionOutput2X = value;
                OnPropertyChanged("ConnectionOutput2X");
            }
        }
        public double ConnectionOutput2Y
        {
            get => _connectionOutput2Y;
            set
            {
                _connectionOutput2Y = value;
                OnPropertyChanged("ConnectionOutput2Y");
            }
        }

        protected Block(Canvas canvas, int maxConnectionsCount, PropertiesManager propertiesManager)
        {
            Id = _blocksCounter++;
            _canvasBlocks = canvas;
            _maxConnectionsCount = maxConnectionsCount;
            _propertiesManager = propertiesManager;
        }

        protected void AddShapeToCanvas()
        {
            _canvasBlocks.Children.Add(Shape);
            ChildrenAddEvents();
        }

        protected void AddTextBlockToCanvas(string prefix = "", string suffix = "")
        {
            _textBlockCommand = new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.Black)
            };

            _prefixCommand = prefix;
            _suffixCommand = suffix;
            _canvasBlocks.Children.Add(_textBlockCommand);
            ChildrenAddEvents();
        }

        private void ChildrenAddEvents()
        {
            _canvasBlocks.Children[_canvasBlocks.Children.Count - 1].MouseLeftButtonDown += OnMouseLeftButtonDown;
            _canvasBlocks.Children[_canvasBlocks.Children.Count - 1].MouseLeftButtonUp += OnMouseLeftButtonUp;
            _canvasBlocks.Children[_canvasBlocks.Children.Count - 1].MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!_isPressed)
                return;

            var mousePosition = mouseEventArgs.GetPosition(_canvasBlocks);
            SetPosition(mousePosition.X - (Shape.ActualWidth / 2), mousePosition.Y - (Shape.ActualHeight / 2));
        }

        public void SetPosition(double left, double top)
        {
            Canvas.SetLeft(Shape, left);
            Canvas.SetTop(Shape, top);

            if (Shape.ActualWidth != 0 && Shape.ActualHeight != 0)
            {
                Canvas.SetLeft(_textBlockCommand, left + Shape.ActualWidth / 2 - _textBlockCommand.ActualWidth / 2);
                Canvas.SetTop(_textBlockCommand, top + Shape.ActualHeight / 2 - _textBlockCommand.ActualHeight / 2);
            }
            else
            {
                Shape.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                _textBlockCommand.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Canvas.SetLeft(_textBlockCommand, left + Shape.DesiredSize.Width / 2 - _textBlockCommand.DesiredSize.Width / 2);
                Canvas.SetTop(_textBlockCommand, top + Shape.DesiredSize.Height / 2 - _textBlockCommand.DesiredSize.Height / 2);
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

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Shape.ReleaseMouseCapture();
            _isPressed = false;
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (BlockManager.IsAddConnectionMode)
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
            _isPressed = true;
        }

        public void AddConnection(Block block)
        {
            if (this == block)
            {
                MessageBox.Show("Self connection is not possible due to potential endless loop.","Connection error!",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (_maxConnectionsCount == 0)
            {
                MessageBox.Show("Outputs are not supported by this block.", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NextBlockPrimary == null)
            {
                NextBlockPrimary = block;

                _lineConnectionPrimary = CreateConnectionLine(block, 1);
            }
            else if (_maxConnectionsCount == 2 && NextBlockOptional == null)
            {
                NextBlockOptional = block;

                _lineConnectionOptional = CreateConnectionLine(block, 2);
            }
            else
            {
                MessageBox.Show("You cannot add another connection, because all outputs are already used.", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Line CreateConnectionLine(Block secondBlock, int outputIndex)
        {
            var connectionLine = new Line()
            {
                Stroke = Brushes.Black
            };

            _canvasBlocks.Children.Add(connectionLine);

            var binding = new Binding
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RemoveLine(int connectionId)
        {
            switch (connectionId)
            {
                case 0:
                    _canvasBlocks.Children.Remove(_lineConnectionPrimary);
                    NextBlockPrimary = null;
                    break;
                case 1:
                    _canvasBlocks.Children.Remove(_lineConnectionOptional);
                    NextBlockOptional = null;
                    break;
            }
        }

        public abstract Block Run();
    }
}
