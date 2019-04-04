using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Comma_Dot_Visual_Language.Helpers;
using System;
using System.Collections.Generic;

namespace Comma_Dot_Visual_Language.Blocks
{
    public abstract class Block : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; private set; }
        public Block NextBlockPrimary { get; private set; }
        public Block NextBlockOptional { get; private set; }
        public List<Block> PreviousBlocks { get; private set; }

        protected Image Shape;

        private static readonly double ArrowLength = 20;

        private string _command = "";
        private double _connectionInputX;
        private double _connectionInputY;
        private double _connectionOutput1X;
        private double _connectionOutput1Y;
        private double _connectionOutput2X;
        private double _connectionOutput2Y;
        private double _connection1ArrowVector1X;
        private double _connection1ArrowVector1Y;
        private double _connection1ArrowVector2X;
        private double _connection1ArrowVector2Y;
        private double _connection2ArrowVector1X;
        private double _connection2ArrowVector1Y;
        private double _connection2ArrowVector2X;
        private double _connection2ArrowVector2Y;
        private readonly PropertiesManager _propertiesManager;
        private string _prefixCommand = "";
        private string _suffixCommand = "";
        private TextBlock _textBlockCommand;
        private Line _lineConnectionPrimary;
        private Line _lineConnectionOptional;
        private Line _connection1arrow1;
        private Line _connection1arrow2;
        private Line _connection2arrow1;
        private Line _connection2arrow2;
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
        public double Connection1ArrowVector1X
        {
            get => _connection1ArrowVector1X;
            set
            {
                _connection1ArrowVector1X = value;
                OnPropertyChanged("Connection1ArrowVector1X");
            }
        }
        public double Connection1ArrowVector1Y
        {
            get => _connection1ArrowVector1Y;
            set
            {
                _connection1ArrowVector1Y = value;
                OnPropertyChanged("Connection1ArrowVector1Y");
            }
        }
        public double Connection1ArrowVector2X
        {
            get => _connection1ArrowVector2X;
            set
            {
                _connection1ArrowVector2X = value;
                OnPropertyChanged("Connection1ArrowVector2X");
            }
        }
        public double Connection1ArrowVector2Y
        {
            get => _connection1ArrowVector2Y;
            set
            {
                _connection1ArrowVector2Y = value;
                OnPropertyChanged("Connection1ArrowVector2Y");
            }
        }
        public double Connection2ArrowVector1X
        {
            get => _connection2ArrowVector1X;
            set
            {
                _connection2ArrowVector1X = value;
                OnPropertyChanged("Connection2ArrowVector1X");
            }
        }
        public double Connection2ArrowVector1Y
        {
            get => _connection2ArrowVector1Y;
            set
            {
                _connection2ArrowVector1Y = value;
                OnPropertyChanged("Connection2ArrowVector1Y");
            }
        }
        public double Connection2ArrowVector2X
        {
            get => _connection2ArrowVector2X;
            set
            {
                _connection2ArrowVector2X = value;
                OnPropertyChanged("Connection2ArrowVector2X");
            }
        }
        public double Connection2ArrowVector2Y
        {
            get => _connection2ArrowVector2Y;
            set
            {
                _connection2ArrowVector2Y = value;
                OnPropertyChanged("Connection2ArrowVector2Y");
            }
        }




        protected Block(Canvas canvas, int maxConnectionsCount, PropertiesManager propertiesManager)
        {
            Id = _blocksCounter++;
            PreviousBlocks = new List<Block>();
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
                Foreground = new SolidColorBrush(Colors.White),
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

            if (Shape.ActualWidth == 0 || Shape.ActualHeight == 0)
            {
                Shape.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                _textBlockCommand.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            Canvas.SetLeft(_textBlockCommand, left + 20f);
            Canvas.SetTop(_textBlockCommand, top + 9.5f);

            SetConnectionsPositions(left, top);

            SetArrowPositions();
            foreach (Block block in PreviousBlocks)
            {
                block.SetArrowPositions();
            }
        }

        protected virtual void SetConnectionsPositions(double shapeLeft, double shapeTop)
        {

            ConnectionInputX = shapeLeft + Shape.ActualWidth / 2;
            ConnectionInputY = shapeTop;

            ConnectionOutput1X = ConnectionOutput2X = shapeLeft + Shape.ActualWidth / 2;
            ConnectionOutput1Y = ConnectionOutput2Y = shapeTop + Shape.ActualHeight;
        }

        private void SetArrowPositions()
        {
            if (NextBlockPrimary != null)
            {
                double connectionDirectionX = _connectionOutput1X - NextBlockPrimary.ConnectionInputX;
                double connectionDirectionY = _connectionOutput1Y - NextBlockPrimary.ConnectionInputY;

                double arrow1DirectionX = connectionDirectionX + connectionDirectionY;
                double arrow1DirectionY = connectionDirectionY + (-connectionDirectionX);

                double arrow2DirectionX = connectionDirectionX + (-connectionDirectionY);
                double arrow2DirectionY = connectionDirectionY + connectionDirectionX;

                double length = Math.Sqrt(arrow1DirectionX * arrow1DirectionX + arrow1DirectionY * arrow1DirectionY);

                arrow1DirectionX = arrow1DirectionX / length * ArrowLength;
                arrow1DirectionY = arrow1DirectionY / length * ArrowLength;
                arrow2DirectionX = arrow2DirectionX / length * ArrowLength;
                arrow2DirectionY = arrow2DirectionY / length * ArrowLength;

                Connection1ArrowVector1X = NextBlockPrimary.ConnectionInputX + arrow1DirectionX;
                Connection1ArrowVector1Y = NextBlockPrimary.ConnectionInputY + arrow1DirectionY;
                Connection1ArrowVector2X = NextBlockPrimary.ConnectionInputX + arrow2DirectionX;
                Connection1ArrowVector2Y = NextBlockPrimary.ConnectionInputY + arrow2DirectionY;
            }

            if (NextBlockOptional != null)
            {
                double connectionDirectionX = _connectionOutput2X - NextBlockOptional.ConnectionInputX;
                double connectionDirectionY = _connectionOutput2Y - NextBlockOptional.ConnectionInputY;

                double arrow1DirectionX = connectionDirectionX + connectionDirectionY;
                double arrow1DirectionY = connectionDirectionY + (-connectionDirectionX);

                double arrow2DirectionX = connectionDirectionX + (-connectionDirectionY);
                double arrow2DirectionY = connectionDirectionY + connectionDirectionX;

                double length = Math.Sqrt(arrow1DirectionX * arrow1DirectionX + arrow1DirectionY * arrow1DirectionY);

                arrow1DirectionX = arrow1DirectionX / length * ArrowLength;
                arrow1DirectionY = arrow1DirectionY / length * ArrowLength;
                arrow2DirectionX = arrow2DirectionX / length * ArrowLength;
                arrow2DirectionY = arrow2DirectionY / length * ArrowLength;

                Connection2ArrowVector1X = NextBlockOptional.ConnectionInputX + arrow1DirectionX;
                Connection2ArrowVector1Y = NextBlockOptional.ConnectionInputY + arrow1DirectionY;
                Connection2ArrowVector2X = NextBlockOptional.ConnectionInputX + arrow2DirectionX;
                Connection2ArrowVector2Y = NextBlockOptional.ConnectionInputY + arrow2DirectionY;
            }
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
                NextBlockPrimary.PreviousBlocks.Add(this);

                _lineConnectionPrimary = CreateConnectionLine(block, 1);
            }
            else if (_maxConnectionsCount == 2 && NextBlockOptional == null)
            {
                NextBlockOptional = block;
                NextBlockOptional.PreviousBlocks.Add(this);

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
                Stroke = Brushes.White
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

            createConnectionArrow(secondBlock, outputIndex);

            return connectionLine;
        }

        private void createConnectionArrow(Block secondBlock, int outputIndex)
        {
            var arrowLine1 = new Line()
            {
                Stroke = Brushes.White
            };

            _canvasBlocks.Children.Add(arrowLine1);


            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector1X")
            };
            arrowLine1.SetBinding(Line.X1Property, binding);

            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector1Y")
            };
            arrowLine1.SetBinding(Line.Y1Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputX")
            };
            arrowLine1.SetBinding(Line.X2Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputY")
            };
            arrowLine1.SetBinding(Line.Y2Property, binding);

            var arrowLine2 = new Line()
            {
                Stroke = Brushes.White
            };

            _canvasBlocks.Children.Add(arrowLine2);


            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector2X")
            };
            arrowLine2.SetBinding(Line.X1Property, binding);

            binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Connection" + outputIndex + "ArrowVector2Y")
            };
            arrowLine2.SetBinding(Line.Y1Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputX")
            };
            arrowLine2.SetBinding(Line.X2Property, binding);

            binding = new Binding
            {
                Source = secondBlock,
                Path = new PropertyPath("ConnectionInputY")
            };
            arrowLine2.SetBinding(Line.Y2Property, binding);

            switch (outputIndex)
            {
                case 1:
                    _connection1arrow1 = arrowLine1;
                    _connection1arrow2 = arrowLine2;
                    break;
                case 2:
                    _connection2arrow1 = arrowLine1;
                    _connection2arrow2 = arrowLine2;
                    break;
            }
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
                    _canvasBlocks.Children.Remove(_connection1arrow1);
                    _canvasBlocks.Children.Remove(_connection1arrow2);
                    NextBlockPrimary = null;
                    break;
                case 1:
                    _canvasBlocks.Children.Remove(_lineConnectionOptional);
                    _canvasBlocks.Children.Remove(_connection2arrow1);
                    _canvasBlocks.Children.Remove(_connection2arrow2);
                    NextBlockOptional = null;
                    break;
            }
        }

        public abstract Block Run();
    }
}
